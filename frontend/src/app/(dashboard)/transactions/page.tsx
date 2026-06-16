'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { transactionService } from '@/shared/lib/services/transactionService';
import type { Transaction } from '@/shared/lib/services/transactionService';
import { accountService } from '@/shared/lib/services/accountService';
import type { Account } from '@/shared/lib/services/accountService';
import { categoryService } from '@/shared/lib/services/categoryService';
import type { Category } from '@/shared/lib/services/categoryService';
import {
    transactionSchema,
    emptyTransactionForm,
    toTransactionRequest,
    type TransactionFormValues,
} from '@/features/finance/utils/transactionForm';
import { getTransactionColumns } from '@/features/finance/utils/transactionColumns';
import { TransactionFormFields } from '@/features/finance/components/TransactionFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

const today = () => new Date().toISOString().slice(0, 10);

export default function TransactionsPage() {
    const toast = useToast();

    const list = usePaginatedList<Transaction>({
        fetcher: transactionService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách giao dịch',
    });

    const [accounts, setAccounts] = useState<Account[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Transaction | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<TransactionFormValues>({ resolver: zodResolver(transactionSchema), defaultValues: emptyTransactionForm });

    useEffect(() => {
        accountService
            .getAll({ pageSize: 100, isActive: true })
            .then((d) => setAccounts(d.items))
            .catch(() => setAccounts([]));
        categoryService
            .getCategories()
            .then(setCategories)
            .catch(() => setCategories([]));
    }, []);

    const openCreate = () => {
        setSelected(null);
        reset(emptyTransactionForm);
        setModalOpen(true);
    };

    const openEdit = (tx: Transaction) => {
        setSelected(tx);
        reset({
            accountId: tx.accountId,
            type: tx.type,
            amount: String(tx.amount),
            currency: tx.currency,
            transactionDate: tx.transactionDate ? tx.transactionDate.slice(0, 10) : today(),
            categoryId: tx.categoryId ?? '',
            status: String(tx.status),
            description: tx.description ?? '',
            note: tx.note ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getTransactionColumns({
        onEdit: openEdit,
        onDelete: (row) => {
            setSelected(row);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: TransactionFormValues) => {
        setSubmitting(true);
        try {
            const payload = toTransactionRequest(values);
            if (selected) {
                await transactionService.update(selected.id, payload);
                toast.success('Đã cập nhật giao dịch');
            } else {
                await transactionService.create(payload);
                toast.success('Đã tạo giao dịch');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu giao dịch thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await transactionService.remove(selected.id);
            toast.success('Đã xóa giao dịch');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa giao dịch thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Giao dịch"
                subtitle="Quản lý thu, chi và chuyển khoản"
                action={
                    <button
                        onClick={openCreate}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm giao dịch
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo mô tả, ghi chú..."
            />

            <ErrorBanner message={list.error} />

            {list.loading ? (
                <PageLoading />
            ) : (
                <PaginatedTable
                    data={list.items}
                    columns={columns}
                    page={list.page}
                    pageSize={list.pageSize}
                    total={list.total}
                    onPageChange={list.setPage}
                    emptyText="Chưa có giao dịch nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={selected ? 'Sửa giao dịch' : 'Thêm giao dịch'}
                size="lg"
                footer={
                    <>
                        <button
                            onClick={closeModal}
                            className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
                        >
                            Hủy
                        </button>
                        <button
                            onClick={handleSubmit(onSubmit)}
                            disabled={submitting}
                            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
                        >
                            {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
                            {selected ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </>
                }
            >
                <TransactionFormFields
                    register={register}
                    errors={errors}
                    accounts={accounts}
                    categories={categories}
                />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa giao dịch"
                message="Bạn có chắc chắn muốn xóa giao dịch này không?"
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
