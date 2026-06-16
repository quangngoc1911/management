'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { recurringTransactionService } from '@/shared/lib/services/recurringTransactionService';
import type { RecurringTransaction } from '@/shared/lib/services/recurringTransactionService';
import { accountService } from '@/shared/lib/services/accountService';
import type { Account } from '@/shared/lib/services/accountService';
import {
    recurringTransactionSchema,
    emptyRecurringTransactionForm,
    toRecurringTransactionRequest,
    type RecurringTransactionFormValues,
} from '@/features/finance/utils/recurringTransactionForm';
import { getRecurringTransactionColumns } from '@/features/finance/utils/recurringTransactionColumns';
import { RecurringTransactionFormFields } from '@/features/finance/components/RecurringTransactionFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

const today = () => new Date().toISOString().slice(0, 10);

export default function RecurringTransactionsPage() {
    const toast = useToast();

    const list = usePaginatedList<RecurringTransaction>({
        fetcher: recurringTransactionService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách giao dịch định kỳ',
    });

    const [accounts, setAccounts] = useState<Account[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<RecurringTransaction | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<RecurringTransactionFormValues>({
        resolver: zodResolver(recurringTransactionSchema),
        defaultValues: emptyRecurringTransactionForm,
    });

    useEffect(() => {
        accountService
            .getAll({ pageSize: 100, isActive: true })
            .then((d) => setAccounts(d.items))
            .catch(() => setAccounts([]));
    }, []);

    const openCreate = () => {
        setSelected(null);
        reset(emptyRecurringTransactionForm);
        setModalOpen(true);
    };

    const openEdit = (r: RecurringTransaction) => {
        setSelected(r);
        reset({
            accountId: r.accountId,
            name: r.name,
            type: r.type,
            amount: String(r.amount),
            frequency: r.frequency,
            startDate: r.startDate ? r.startDate.slice(0, 10) : today(),
            endDate: r.endDate ? r.endDate.slice(0, 10) : '',
            nextDueDate: r.nextDueDate ? r.nextDueDate.slice(0, 10) : '',
            isActive: r.isActive,
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getRecurringTransactionColumns({
        onEdit: openEdit,
        onDelete: (row) => {
            setSelected(row);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: RecurringTransactionFormValues) => {
        setSubmitting(true);
        try {
            const payload = toRecurringTransactionRequest(values);
            if (selected) {
                await recurringTransactionService.update(selected.id, payload);
                toast.success('Đã cập nhật giao dịch định kỳ');
            } else {
                await recurringTransactionService.create(payload);
                toast.success('Đã tạo giao dịch định kỳ');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu giao dịch định kỳ thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await recurringTransactionService.remove(selected.id);
            toast.success('Đã xóa giao dịch định kỳ');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa giao dịch định kỳ thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Giao dịch định kỳ"
                subtitle="Thu/chi lặp lại theo lịch"
                action={
                    <button
                        onClick={openCreate}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm định kỳ
                    </button>
                }
            />

            <SearchInput value={list.search} onChange={list.setSearch} placeholder="Tìm theo tên..." />

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
                    emptyText="Chưa có giao dịch định kỳ nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={selected ? 'Sửa giao dịch định kỳ' : 'Thêm giao dịch định kỳ'}
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
                <RecurringTransactionFormFields register={register} errors={errors} accounts={accounts} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa giao dịch định kỳ"
                message={`Bạn có chắc chắn muốn xóa "${selected?.name}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
