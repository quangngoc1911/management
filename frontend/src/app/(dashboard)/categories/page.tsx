'use client';

import { useCallback, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { categoryService } from '@/shared/lib/services/categoryService';
import type { Category } from '@/shared/lib/services/categoryService';
import {
    categorySchema,
    emptyCategoryForm,
    toCategoryRequest,
    type CategoryFormValues,
} from '@/features/documents/utils/categoryForm';
import { renderCategoryTree } from '@/features/documents/utils/categoryColumns';
import { CategoryFormFields } from '@/features/documents/components/CategoryFormFields';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { ErrorBanner } from '@/components/ErrorBanner';
import { useToast } from '@/shared/hooks/useToast';

export default function CategoriesPage() {
    const toast = useToast();

    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [expandedIds, setExpandedIds] = useState<Set<string>>(new Set());
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Category | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<CategoryFormValues>({
        resolver: zodResolver(categorySchema),
        defaultValues: emptyCategoryForm,
    });

    const loadCategories = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await categoryService.getCategories();
            setCategories(data);
        } catch (err) {
            setCategories([]);
            setError(err instanceof Error ? err.message : 'Không thể tải danh mục');
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        loadCategories();
    }, [loadCategories]);

    const toggleExpand = (id: string) => {
        setExpandedIds((prev) => {
            const next = new Set(prev);
            if (next.has(id)) {
                next.delete(id);
            } else {
                next.add(id);
            }
            return next;
        });
    };

    const openCreateModal = (parentId?: string) => {
        setSelected(null);
        reset({ ...emptyCategoryForm, parentId: parentId ?? '' });
        setModalOpen(true);
    };

    const openEditModal = (category: Category) => {
        setSelected(category);
        reset({
            name: category.name,
            slug: category.slug,
            description: category.description ?? '',
            parentId: category.parentId ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const onSubmit = async (values: CategoryFormValues) => {
        setSubmitting(true);
        try {
            const payload = toCategoryRequest(values);
            if (selected) {
                await categoryService.update(selected.id, payload);
                toast.success('Đã cập nhật danh mục');
            } else {
                await categoryService.create(payload);
                toast.success('Đã tạo danh mục mới');
            }
            closeModal();
            await loadCategories();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Không thể lưu danh mục');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await categoryService.remove(selected.id);
            toast.success('Đã xóa danh mục');
            setDeleteModalOpen(false);
            setSelected(null);
            await loadCategories();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Không thể xóa danh mục');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Quản lý danh mục"
                subtitle="Quản lý cây thư mục tài liệu"
                action={
                    <button
                        onClick={() => openCreateModal()}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm danh mục
                    </button>
                }
            />

            <ErrorBanner message={error} />

            <div className="card overflow-hidden">
                {loading ? (
                    <div className="p-10 text-center">
                        <PageLoading />
                    </div>
                ) : categories.length > 0 ? (
                    <div className="divide-y divide-border">
                        {renderCategoryTree(categories, {
                            onEdit: openEditModal,
                            onDelete: (cat) => {
                                setSelected(cat);
                                setDeleteModalOpen(true);
                            },
                            onAddChild: (cat) => openCreateModal(cat.id),
                            expandedIds,
                            onToggleExpand: toggleExpand,
                        })}
                    </div>
                ) : (
                    <div className="p-10 text-center text-muted">Chưa có danh mục nào</div>
                )}
            </div>

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa danh mục' : 'Thêm danh mục'}
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
                <CategoryFormFields
                    register={register}
                    errors={errors}
                    categories={categories}
                    isEdit={isEdit}
                />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa danh mục"
                message={`Bạn có chắc chắn muốn xóa danh mục "${selected?.name}" không? Tất cả tài liệu trong danh mục này sẽ bị ảnh hưởng.`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
