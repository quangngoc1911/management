'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { documentService } from '@/shared/lib/services/documentService';
import type { Document } from '@/shared/lib/services/documentService';
import { categoryService } from '@/shared/lib/services/categoryService';
import type { Category } from '@/shared/lib/services/categoryService';
import {
    documentSchema,
    emptyDocumentForm,
    toDocumentRequest,
    type DocumentFormValues,
} from '@/features/documents/utils/documentForm';
import { getDocumentColumns } from '@/features/documents/utils/documentColumns';
import { DocumentFormFields } from '@/features/documents/components/DocumentFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function DocumentsPage() {
    const toast = useToast();

    const list = usePaginatedList<Document>({
        fetcher: documentService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách tài liệu',
    });

    const [categories, setCategories] = useState<Category[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Document | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<DocumentFormValues>({
        resolver: zodResolver(documentSchema),
        defaultValues: emptyDocumentForm,
    });

    // Load categories for the dropdown (non-paginated tree list).
    useEffect(() => {
        categoryService.getCategories().then(setCategories).catch(() => setCategories([]));
    }, []);

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyDocumentForm);
        setModalOpen(true);
    };

    const openEditModal = (doc: Document) => {
        setSelected(doc);
        reset({
            title: doc.title,
            content: doc.content,
            categoryId: doc.categoryId,
            status: doc.status,
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getDocumentColumns({
        onEdit: openEditModal,
        onDelete: (doc) => {
            setSelected(doc);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: DocumentFormValues) => {
        setSubmitting(true);
        try {
            if (selected) {
                await documentService.update(selected.id, {
                    title: values.title,
                    content: values.content,
                    categoryId: values.categoryId,
                    status: values.status,
                });
                toast.success('Đã cập nhật tài liệu');
            } else {
                await documentService.create(toDocumentRequest(values));
                toast.success('Đã tạo tài liệu mới');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Không thể lưu tài liệu');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await documentService.remove(selected.id);
            toast.success('Đã xóa tài liệu');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Không thể xóa tài liệu');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Quản lý tài liệu"
                subtitle="Quản lý các tài liệu trong hệ thống"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm tài liệu
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm kiếm tài liệu..."
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
                    emptyText="Chưa có tài liệu nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa tài liệu' : 'Thêm tài liệu'}
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
                <DocumentFormFields
                    register={register}
                    errors={errors}
                    categories={categories}
                    isEdit={isEdit}
                />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa tài liệu"
                message={`Bạn có chắc chắn muốn xóa tài liệu "${selected?.title}" không? Hành động này không thể hoàn tác.`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
