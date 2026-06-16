'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { systemConfigService } from '@/shared/lib/services/systemConfigService';
import type { SystemConfig } from '@/shared/lib/services/systemConfigService';
import {
    systemConfigSchema,
    emptySystemConfigForm,
    type SystemConfigFormValues,
} from '@/features/system/utils/systemConfigForm';
import { getSystemConfigColumns } from '@/features/system/utils/systemConfigColumns';
import { SystemConfigFormFields } from '@/features/system/components/SystemConfigFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function SystemConfigsPage() {
    const toast = useToast();

    const list = usePaginatedList<SystemConfig>({
        fetcher: systemConfigService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách cấu hình',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<SystemConfig | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<SystemConfigFormValues>({
        resolver: zodResolver(systemConfigSchema),
        defaultValues: emptySystemConfigForm,
    });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptySystemConfigForm);
        setModalOpen(true);
    };

    const openEditModal = (config: SystemConfig) => {
        setSelected(config);
        reset({
            key: config.key,
            value: config.value,
            description: config.description ?? '',
            isEncrypted: config.isEncrypted,
            isPublic: config.isPublic,
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getSystemConfigColumns({
        onEdit: openEditModal,
        onDelete: (config) => {
            setSelected(config);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: SystemConfigFormValues) => {
        setSubmitting(true);
        try {
            if (selected) {
                await systemConfigService.update(selected.id, {
                    value: values.value,
                    description: values.description?.trim() || null,
                    isEncrypted: values.isEncrypted ?? false,
                    isPublic: values.isPublic ?? false,
                });
                toast.success('Cập nhật cấu hình thành công');
            } else {
                await systemConfigService.create({
                    key: values.key.trim(),
                    value: values.value,
                    description: values.description?.trim() || null,
                    isEncrypted: values.isEncrypted ?? false,
                    isPublic: values.isPublic ?? false,
                });
                toast.success('Tạo cấu hình thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu cấu hình thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await systemConfigService.remove(selected.id);
            toast.success('Xóa cấu hình thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa cấu hình thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Cấu hình hệ thống"
                subtitle="Tham số cấu hình toàn hệ thống (chỉ Admin)"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm cấu hình
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo khoá, mô tả..."
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
                    emptyText="Chưa có cấu hình nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa cấu hình' : 'Thêm cấu hình'}
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
                <SystemConfigFormFields register={register} errors={errors} isEdit={isEdit} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa cấu hình"
                message={`Bạn có chắc chắn muốn xóa "${selected?.key}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
