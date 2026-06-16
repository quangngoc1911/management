'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { accountService } from '@/shared/lib/services/accountService';
import type { Account } from '@/shared/lib/services/accountService';
import {
    accountSchema,
    emptyAccountForm,
    toAccountRequest,
    type AccountFormValues,
} from '@/features/finance/utils/accountForm';
import { getAccountColumns } from '@/features/finance/utils/accountColumns';
import { AccountFormFields } from '@/features/finance/components/AccountFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function AccountsPage() {
    const toast = useToast();

    const list = usePaginatedList<Account>({
        fetcher: accountService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách tài khoản',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Account | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<AccountFormValues>({ resolver: zodResolver(accountSchema), defaultValues: emptyAccountForm });

    const openCreate = () => {
        setSelected(null);
        reset(emptyAccountForm);
        setModalOpen(true);
    };

    const openEdit = (account: Account) => {
        setSelected(account);
        reset({
            name: account.name,
            accountType: account.accountType,
            bankName: account.bankName ?? '',
            accountNumber: account.accountNumber ?? '',
            currency: account.currency,
            isActive: account.isActive,
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getAccountColumns({
        onEdit: openEdit,
        onDelete: (row) => {
            setSelected(row);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: AccountFormValues) => {
        setSubmitting(true);
        try {
            const payload = toAccountRequest(values);
            if (selected) {
                await accountService.update(selected.id, payload);
                toast.success('Đã cập nhật tài khoản');
            } else {
                await accountService.create(payload);
                toast.success('Đã tạo tài khoản');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu tài khoản thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await accountService.remove(selected.id);
            toast.success('Đã xóa tài khoản');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa tài khoản thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Tài khoản tài chính"
                subtitle="Quản lý tài khoản, ví, thẻ của gia đình"
                action={
                    <button
                        onClick={openCreate}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm tài khoản
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo tên, ngân hàng..."
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
                    emptyText="Chưa có tài khoản nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={selected ? 'Sửa tài khoản' : 'Thêm tài khoản'}
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
                <AccountFormFields register={register} errors={errors} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa tài khoản"
                message={`Bạn có chắc chắn muốn xóa "${selected?.name}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
