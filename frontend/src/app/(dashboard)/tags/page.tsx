'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { tagService } from '@/shared/lib/services/tagService';
import type { Tag } from '@/shared/lib/services/tagService';
import { tagSchema, emptyTagForm, toTagRequest, type TagFormValues } from '@/features/documents/utils/tagForm';
import { getTagColumns } from '@/features/documents/utils/tagColumns';
import { TagFormFields } from '@/features/documents/components/TagFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function TagsPage() {
    const toast = useToast();

    const list = usePaginatedList<Tag>({
        fetcher: tagService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách tag',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Tag | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<TagFormValues>({ resolver: zodResolver(tagSchema), defaultValues: emptyTagForm });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyTagForm);
        setModalOpen(true);
    };

    const openEditModal = (tag: Tag) => {
        setSelected(tag);
        reset({ name: tag.name, slug: tag.slug, color: tag.color ?? '' });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getTagColumns({
        onEdit: openEditModal,
        onDelete: (tag) => {
            setSelected(tag);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: TagFormValues) => {
        setSubmitting(true);
        try {
            const payload = toTagRequest(values);
            if (selected) {
                await tagService.update(selected.id, payload);
                toast.success('Đã cập nhật tag');
            } else {
                await tagService.create(payload);
                toast.success('Đã tạo tag mới');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Không thể lưu tag');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await tagService.remove(selected.id);
            toast.success('Đã xóa tag');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Không thể xóa tag');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Quản lý Tags"
                subtitle="Quản lý các tag cho tài liệu"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm tag
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm kiếm tag..."
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
                    emptyText="Chưa có tag nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa tag' : 'Thêm tag'}
                size="sm"
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
                <TagFormFields register={register} errors={errors} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa tag"
                message={`Bạn có chắc chắn muốn xóa tag "${selected?.name}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
