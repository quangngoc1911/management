'use client';

import { useCallback, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Plus, Edit, Trash2, ChevronRight, ChevronDown, Folder, FolderOpen, Loader2 } from 'lucide-react';
import { categoryService, Category, CreateCategoryRequest } from '@/shared/lib/services/categoryService';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';

const categorySchema = z.object({
  name: z.string().min(2, 'Tên danh mục phải có ít nhất 2 ký tự'),
  slug: z.string().optional(),
  description: z.string().optional(),
  parentId: z.string().optional(),
});

type CategoryFormData = z.infer<typeof categorySchema>;

export default function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [expandedIds, setExpandedIds] = useState<Set<string>>(new Set());
  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const { register, handleSubmit, reset, formState: { errors } } = useForm<CategoryFormData>({
    resolver: zodResolver(categorySchema),
  });

  const fetchCategories = async () => {
    setLoading(true);
    try {
      const data = await categoryService.getCategories();
      setCategories(data);
    } catch (error) {
      // Mock data
      setCategories([
        { id: '1', name: 'Tài liệu pháp luật', slug: 'phap-luat', description: 'Các văn bản pháp luật', documentCount: 25, createdAt: '2024-01-01', updatedAt: '2024-01-01', children: [
          { id: '1-1', name: 'Luật doanh nghiệp', slug: 'luat-doanh-nghiep', documentCount: 10, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
          { id: '1-2', name: 'Luật thuế', slug: 'luat-thue', documentCount: 15, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
        ]},
        { id: '2', name: 'Tài liệu kỹ thuật', slug: 'ki-thuat', description: 'Các tài liệu kỹ thuật', documentCount: 42, createdAt: '2024-01-02', updatedAt: '2024-01-02' },
        { id: '3', name: 'Hướng dẫn sử dụng', slug: 'huong-dan', description: 'Sách hướng dẫn', documentCount: 18, createdAt: '2024-01-03', updatedAt: '2024-01-03' },
      ]);
      setExpandedIds(new Set(['1']));
    } finally {
      setLoading(false);
    }
  };

  const fetchCategoriesCallback = useCallback(async () => {
    setLoading(true);
    try {
      const data = await categoryService.getCategories();
      setCategories(data);
    } catch (error) {
      // Mock data
      setCategories([
        { id: '1', name: 'Tài liệu pháp luật', slug: 'phap-luat', description: 'Các văn bản pháp luật', documentCount: 25, createdAt: '2024-01-01', updatedAt: '2024-01-01', children: [
          { id: '1-1', name: 'Luật doanh nghiệp', slug: 'luat-doanh-nghiep', documentCount: 10, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
          { id: '1-2', name: 'Luật thuế', slug: 'luat-thue', documentCount: 15, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
        ]},
        { id: '2', name: 'Tài liệu kỹ thuật', slug: 'ki-thuat', description: 'Các tài liệu kỹ thuật', documentCount: 42, createdAt: '2024-01-02', updatedAt: '2024-01-02' },
        { id: '3', name: 'Hướng dẫn sử dụng', slug: 'huong-dan', description: 'Sách hướng dẫn', documentCount: 18, createdAt: '2024-01-03', updatedAt: '2024-01-03' },
      ]);
      setExpandedIds(new Set(['1']));
    } finally {
      setLoading(false);
    }
  }, [setCategories, setLoading, setExpandedIds]);

  useEffect(() => {
    fetchCategoriesCallback();
  }, [fetchCategoriesCallback]);

  const toggleExpand = (id: string) => {
    const newExpanded = new Set(expandedIds);
    if (newExpanded.has(id)) {
      newExpanded.delete(id);
    } else {
      newExpanded.add(id);
    }
    setExpandedIds(newExpanded);
  };

  const handleCreate = async (data: CategoryFormData) => {
    setSubmitting(true);
    try {
      await categoryService.createCategory(data);
      setModalOpen(false);
      reset();
      fetchCategories();
    } catch (error) {
      console.error('Create category error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleUpdate = async (data: CategoryFormData) => {
    if (!selectedCategory) return;
    setSubmitting(true);
    try {
      await categoryService.updateCategory(selectedCategory.id, data);
      setModalOpen(false);
      setSelectedCategory(null);
      reset();
      fetchCategories();
    } catch (error) {
      console.error('Update category error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async () => {
    if (!selectedCategory) return;
    setSubmitting(true);
    try {
      await categoryService.deleteCategory(selectedCategory.id);
      setDeleteModalOpen(false);
      setSelectedCategory(null);
      fetchCategories();
    } catch (error) {
      console.error('Delete category error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const openCreateModal = (parentId?: string) => {
    setSelectedCategory(null);
    reset({ parentId });
    setModalOpen(true);
  };

  const openEditModal = (category: Category) => {
    setSelectedCategory(category);
    reset({ name: category.name, slug: category.slug, description: category.description, parentId: category.parentId });
    setModalOpen(true);
  };

  const openDeleteModal = (category: Category) => {
    setSelectedCategory(category);
    setDeleteModalOpen(true);
  };

  const renderTree = (items: Category[], level = 0) => {
    return items.map((category) => {
      const hasChildren = category.children && category.children.length > 0;
      const isExpanded = expandedIds.has(category.id);

      return (
        <div key={category.id}>
          <div 
            className={`flex items-center gap-2 px-4 py-3 hover:bg-surface-alt transition ${
              level > 0 ? `ml-${level * 6}` : ''
            }`}
          >
            {/* Expand/Collapse */}
            {hasChildren ? (
              <button
                onClick={() => toggleExpand(category.id)}
                className="p-1 text-muted hover:text-foreground"
              >
                {isExpanded ? <ChevronDown className="w-4 h-4" /> : <ChevronRight className="w-4 h-4" />}
              </button>
            ) : (
              <span className="w-6" />
            )}

            {/* Icon */}
            {hasChildren || isExpanded ? (
              <FolderOpen className="w-5 h-5 text-primary" />
            ) : (
              <Folder className="w-5 h-5 text-muted" />
            )}

            {/* Name */}
            <div className="flex-1 min-w-0">
              <p className="font-medium text-foreground truncate">{category.name}</p>
              {category.description && (
                <p className="text-xs text-muted truncate">{category.description}</p>
              )}
            </div>

            {/* Document Count */}
            <span className="badge badge-neutral">
              {category.documentCount || 0} tài liệu
            </span>

            {/* Actions */}
            <div className="flex items-center gap-1">
              <button
                onClick={() => openCreateModal(category.id)}
                className="p-1.5 text-muted hover:text-primary hover:bg-surface-alt rounded transition"
                title="Thêm danh mục con"
              >
                <Plus className="w-4 h-4" />
              </button>
              <button
                onClick={() => openEditModal(category)}
                className="p-1.5 text-muted hover:text-primary hover:bg-surface-alt rounded transition"
                title="Sửa"
              >
                <Edit className="w-4 h-4" />
              </button>
              <button
                onClick={() => openDeleteModal(category)}
                className="p-1.5 text-muted hover:text-danger hover:bg-surface-alt rounded transition"
                title="Xóa"
              >
                <Trash2 className="w-4 h-4" />
              </button>
            </div>
          </div>

          {/* Children */}
          {hasChildren && isExpanded && (
            <div className="border-l border-border ml-3">
              {renderTree(category.children!, level + 1)}
            </div>
          )}
        </div>
      );
    });
  };

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div className="page-header">
        <div>
          <h1 className="text-2xl font-bold text-foreground">Quản lý danh mục</h1>
          <p className="text-muted mt-1">Quản lý cây thư mục tài liệu</p>
        </div>
        <button 
          onClick={() => openCreateModal()} 
          className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
        >
          <Plus className="w-4 h-4" />
          Thêm danh mục
        </button>
      </div>

      {/* Categories Tree */}
      <div className="card overflow-hidden">
        {loading ? (
          <div className="p-10 text-center">
            <PageLoading />
          </div>
        ) : categories.length > 0 ? (
          <div className="divide-y divide-border">
            {renderTree(categories)}
          </div>
        ) : (
          <div className="p-10 text-center text-muted">
            Chưa có danh mục nào
          </div>
        )}
      </div>

      {/* Create/Edit Modal */}
      <Modal
        open={modalOpen}
        onClose={() => { setModalOpen(false); setSelectedCategory(null); }}
        title={selectedCategory ? 'Sửa danh mục' : 'Thêm danh mục'}
        footer={
          <>
            <button
              onClick={() => { setModalOpen(false); setSelectedCategory(null); }}
              className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
            >
              Hủy
            </button>
            <button
              onClick={handleSubmit(selectedCategory ? handleUpdate : handleCreate)}
              disabled={submitting}
              className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
            >
              {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
              {selectedCategory ? 'Cập nhật' : 'Tạo mới'}
            </button>
          </>
        }
      >
        <form className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Tên danh mục</label>
            <input
              {...register('name')}
              className={`input ${errors.name ? 'input-error' : ''}`}
              placeholder="Nhập tên danh mục"
            />
            {errors.name && <p className="text-danger text-xs mt-1">{errors.name.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Slug (URL)</label>
            <input
              {...register('slug')}
              className="input"
              placeholder="duong-dan-url (tùy chọn)"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Mô tả</label>
            <textarea
              {...register('description')}
              className="input resize-none h-24"
              placeholder="Nhập mô tả (tùy chọn)"
            />
          </div>

          {selectedCategory && (
            <div>
              <label className="block text-sm font-medium text-foreground mb-2">Danh mục cha</label>
              <select {...register('parentId')} className="input">
                <option value="">-- Không có --</option>
                {categories.map((cat) => (
                  <option key={cat.id} value={cat.id}>{cat.name}</option>
                ))}
              </select>
            </div>
          )}
        </form>
      </Modal>

      {/* Delete Confirmation Modal */}
      <ConfirmModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        title="Xóa danh mục"
        message={`Bạn có chắc chắn muốn xóa danh mục "${selectedCategory?.name}" không? Tất cả tài liệu trong danh mục này sẽ bị ảnh hưởng.`}
        confirmText="Xóa"
        onConfirm={handleDelete}
        danger
      />
    </div>
  );
}
