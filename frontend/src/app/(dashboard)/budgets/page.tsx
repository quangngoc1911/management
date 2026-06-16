'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { budgetService } from '@/shared/lib/services/budgetService';
import type { Budget } from '@/shared/lib/services/budgetService';
import {
    budgetSchema,
    emptyBudgetForm,
    toBudgetRequest,
    type BudgetFormValues,
} from '@/features/finance/utils/budgetForm';
import { getBudgetColumns } from '@/features/finance/utils/budgetColumns';
import { BudgetFormFields } from '@/features/finance/components/BudgetFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

const today = () => new Date().toISOString().slice(0, 10);

export default function BudgetsPage() {
    const toast = useToast();

    const list = usePaginatedList<Budget>({
        fetcher: budgetService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách ngân sách',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Budget | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<BudgetFormValues>({ resolver: zodResolver(budgetSchema), defaultValues: emptyBudgetForm });

    const openCreate = () => {
        setSelected(null);
        reset(emptyBudgetForm);
        setModalOpen(true);
    };

    const openEdit = (b: Budget) => {
        setSelected(b);
        reset({
            name: b.name,
            amount: String(b.amount),
            currency: b.currency,
            periodType: b.periodType,
            startDate: b.startDate ? b.startDate.slice(0, 10) : today(),
            endDate: b.endDate ? b.endDate.slice(0, 10) : today(),
            alertThreshold: b.alertThreshold != null ? String(b.alertThreshold) : '',
            isActive: b.isActive,
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getBudgetColumns({
        onEdit: openEdit,
        onDelete: (row) => {
            setSelected(row);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: BudgetFormValues) => {
        setSubmitting(true);
        try {
            const payload = toBudgetRequest(values);
            if (selected) {
                await budgetService.update(selected.id, payload);
                toast.success('Đã cập nhật ngân sách');
            } else {
                await budgetService.create(payload);
                toast.success('Đã tạo ngân sách');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu ngân sách thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await budgetService.remove(selected.id);
            toast.success('Đã xóa ngân sách');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa ngân sách thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Ngân sách"
                subtitle="Quản lý hạn mức chi tiêu theo kỳ"
                action={
                    <button
                        onClick={openCreate}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm ngân sách
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
                    emptyText="Chưa có ngân sách nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={selected ? 'Sửa ngân sách' : 'Thêm ngân sách'}
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
                <BudgetFormFields register={register} errors={errors} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa ngân sách"
                message={`Bạn có chắc chắn muốn xóa "${selected?.name}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
