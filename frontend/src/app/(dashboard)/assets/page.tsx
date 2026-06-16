'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { assetService } from '@/shared/lib/services/assetService';
import type { Asset } from '@/shared/lib/services/assetService';
import {
    assetSchema,
    emptyAssetForm,
    toAssetRequest,
    type AssetFormValues,
} from '@/features/assets/utils/assetForm';
import { getAssetColumns } from '@/features/assets/utils/assetColumns';
import { AssetFormFields } from '@/features/assets/components/AssetFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function AssetsPage() {
    const toast = useToast();

    const list = usePaginatedList<Asset>({
        fetcher: assetService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách tài sản',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Asset | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<AssetFormValues>({
        resolver: zodResolver(assetSchema),
        defaultValues: emptyAssetForm,
    });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyAssetForm);
        setModalOpen(true);
    };

    const openEditModal = (asset: Asset) => {
        setSelected(asset);
        reset({
            name: asset.name,
            assetType: asset.assetType,
            purchasePrice: asset.purchasePrice != null ? String(asset.purchasePrice) : '',
            purchaseDate: asset.purchaseDate ? asset.purchaseDate.slice(0, 10) : '',
            currency: asset.currency ?? 'VND',
            location: asset.location ?? '',
            serialNumber: asset.serialNumber ?? '',
            status: String(asset.status),
            isInsured: asset.isInsured ?? false,
            insuranceInfo: asset.insuranceInfo ?? '',
            description: asset.description ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getAssetColumns({
        onEdit: openEditModal,
        onDelete: (asset) => {
            setSelected(asset);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: AssetFormValues) => {
        setSubmitting(true);
        try {
            const payload = toAssetRequest(values);
            if (selected) {
                await assetService.update(selected.id, payload);
                toast.success('Cập nhật tài sản thành công');
            } else {
                await assetService.create(payload);
                toast.success('Thêm tài sản thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu tài sản thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await assetService.remove(selected.id);
            toast.success('Xóa tài sản thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa tài sản thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Tài sản"
                subtitle="Quản lý tài sản của gia đình"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm tài sản
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo tên, mô tả..."
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
                    emptyText="Chưa có tài sản nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa tài sản' : 'Thêm tài sản'}
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
                            {isEdit ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </>
                }
            >
                <AssetFormFields register={register} errors={errors} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa tài sản"
                message={`Bạn có chắc chắn muốn xóa "${selected?.name}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
