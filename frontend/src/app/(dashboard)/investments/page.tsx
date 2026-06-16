'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { investmentService } from '@/shared/lib/services/investmentService';
import type { Investment } from '@/shared/lib/services/investmentService';
import {
    investmentSchema,
    emptyInvestmentForm,
    toInvestmentRequest,
    type InvestmentFormValues,
} from '@/features/finance/utils/investmentForm';
import { getInvestmentColumns } from '@/features/finance/utils/investmentColumns';
import { InvestmentFormFields } from '@/features/finance/components/InvestmentFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function InvestmentsPage() {
    const toast = useToast();

    const list = usePaginatedList<Investment>({
        fetcher: investmentService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách đầu tư',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Investment | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<InvestmentFormValues>({ resolver: zodResolver(investmentSchema), defaultValues: emptyInvestmentForm });

    const openCreate = () => {
        setSelected(null);
        reset(emptyInvestmentForm);
        setModalOpen(true);
    };

    const openEdit = (i: Investment) => {
        setSelected(i);
        reset({
            type: i.type,
            name: i.name,
            symbol: i.symbol ?? '',
            quantity: i.quantity != null ? String(i.quantity) : '',
            purchasePrice: i.purchasePrice != null ? String(i.purchasePrice) : '',
            currentPrice: i.currentPrice != null ? String(i.currentPrice) : '',
            isActive: i.isActive,
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getInvestmentColumns({
        onEdit: openEdit,
        onDelete: (row) => {
            setSelected(row);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: InvestmentFormValues) => {
        setSubmitting(true);
        try {
            const payload = toInvestmentRequest(values);
            if (selected) {
                await investmentService.update(selected.id, payload);
                toast.success('Đã cập nhật khoản đầu tư');
            } else {
                await investmentService.create(payload);
                toast.success('Đã tạo khoản đầu tư');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu khoản đầu tư thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await investmentService.remove(selected.id);
            toast.success('Đã xóa khoản đầu tư');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa khoản đầu tư thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Đầu tư"
                subtitle="Quản lý danh mục đầu tư của gia đình"
                action={
                    <button
                        onClick={openCreate}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm khoản đầu tư
                    </button>
                }
            />

            <SearchInput value={list.search} onChange={list.setSearch} placeholder="Tìm theo tên, mã..." />

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
                    emptyText="Chưa có khoản đầu tư nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={selected ? 'Sửa khoản đầu tư' : 'Thêm khoản đầu tư'}
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
                <InvestmentFormFields register={register} errors={errors} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa khoản đầu tư"
                message={`Bạn có chắc chắn muốn xóa "${selected?.name}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
