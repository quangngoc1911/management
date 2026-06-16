'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { assetValuationService } from '@/shared/lib/services/assetValuationService';
import type { AssetValuation } from '@/shared/lib/services/assetValuationService';
import { assetService } from '@/shared/lib/services/assetService';
import type { Asset } from '@/shared/lib/services/assetService';
import {
    assetValuationSchema,
    emptyAssetValuationForm,
    toAssetValuationCreateRequest,
    toAssetValuationUpdateRequest,
    type AssetValuationFormValues,
} from '@/features/assets/utils/assetValuationForm';
import { getAssetValuationColumns } from '@/features/assets/utils/assetValuationColumns';
import { AssetValuationFormFields } from '@/features/assets/components/AssetValuationFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function AssetValuationsPage() {
    const toast = useToast();

    const list = usePaginatedList<AssetValuation>({
        fetcher: assetValuationService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách định giá',
    });

    const [assets, setAssets] = useState<Asset[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<AssetValuation | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    useEffect(() => {
        assetService.getAll({ pageSize: 100 })
            .then((d) => setAssets(d.items))
            .catch(() => setAssets([]));
    }, []);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<AssetValuationFormValues>({
        resolver: zodResolver(assetValuationSchema),
        defaultValues: emptyAssetValuationForm,
    });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyAssetValuationForm);
        setModalOpen(true);
    };

    const openEditModal = (valuation: AssetValuation) => {
        setSelected(valuation);
        reset({
            assetId: valuation.assetId,
            valuationDate: valuation.valuationDate ? valuation.valuationDate.slice(0, 10) : '',
            value: String(valuation.value),
            currency: valuation.currency,
            valuationMethod: valuation.valuationMethod ?? '',
            notes: valuation.notes ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getAssetValuationColumns({
        onEdit: openEditModal,
        onDelete: (valuation) => {
            setSelected(valuation);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: AssetValuationFormValues) => {
        setSubmitting(true);
        try {
            if (selected) {
                await assetValuationService.update(selected.id, toAssetValuationUpdateRequest(values));
                toast.success('Cập nhật bản định giá thành công');
            } else {
                await assetValuationService.create(toAssetValuationCreateRequest(values));
                toast.success('Thêm bản định giá thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu bản định giá thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await assetValuationService.remove(selected.id);
            toast.success('Xóa bản định giá thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa bản định giá thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Định giá tài sản"
                subtitle="Lịch sử định giá tài sản theo thời gian"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm định giá
                    </button>
                }
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
                    emptyText="Chưa có bản định giá nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa định giá' : 'Thêm định giá'}
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
                            {isEdit ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </>
                }
            >
                <AssetValuationFormFields
                    register={register}
                    errors={errors}
                    assets={assets}
                    isEdit={isEdit}
                />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa bản định giá"
                message="Bạn có chắc chắn muốn xóa bản định giá này không?"
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
