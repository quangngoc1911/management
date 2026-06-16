'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { bookmarkService } from '@/shared/lib/services/bookmarkService';
import type { Bookmark } from '@/shared/lib/services/bookmarkService';
import {
    bookmarkSchema,
    emptyBookmarkForm,
    toBookmarkRequest,
    type BookmarkFormValues,
} from '@/features/bookmarks/utils/bookmarkForm';
import { getBookmarkColumns } from '@/features/bookmarks/utils/bookmarkColumns';
import { BookmarkFormFields } from '@/features/bookmarks/components/BookmarkFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function BookmarksPage() {
    const toast = useToast();

    const list = usePaginatedList<Bookmark>({
        fetcher: bookmarkService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách đánh dấu',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Bookmark | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<BookmarkFormValues>({
        resolver: zodResolver(bookmarkSchema),
        defaultValues: emptyBookmarkForm,
    });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyBookmarkForm);
        setModalOpen(true);
    };

    const openEditModal = (bookmark: Bookmark) => {
        setSelected(bookmark);
        reset({ entityType: bookmark.entityType, entityId: bookmark.entityId, note: bookmark.note ?? '' });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getBookmarkColumns({
        onEdit: openEditModal,
        onDelete: (bookmark) => {
            setSelected(bookmark);
            setDeleteModalOpen(true);
        },
        onOpenLink: (bookmark) => {
            toast.info(`Mở liên kết: ${bookmark.entityType}/${bookmark.entityId}`);
        },
    });

    const onSubmit = async (values: BookmarkFormValues) => {
        setSubmitting(true);
        try {
            if (selected) {
                await bookmarkService.update(selected.id, { note: values.note?.trim() || null });
                toast.success('Cập nhật đánh dấu thành công');
            } else {
                await bookmarkService.create(toBookmarkRequest(values));
                toast.success('Tạo đánh dấu thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu đánh dấu thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await bookmarkService.remove(selected.id);
            toast.success('Xóa đánh dấu thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa đánh dấu thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Đánh dấu"
                subtitle="Các mục bạn đã đánh dấu để xem lại"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm đánh dấu
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
                    emptyText="Chưa có đánh dấu nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa đánh dấu' : 'Thêm đánh dấu'}
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
                <BookmarkFormFields register={register} errors={errors} isEdit={isEdit} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa đánh dấu"
                message="Bạn có chắc chắn muốn xóa đánh dấu này không?"
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
