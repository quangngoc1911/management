'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Plus, Edit, Trash2, Search, Tag as TagIcon, Loader2 } from 'lucide-react';
import { tagService, Tag, CreateTagRequest } from '@/shared/lib/services/tagService';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';

const tagSchema = z.object({
  name: z.string().min(2, 'Tên tag phải có ít nhất 2 ký tự'),
  slug: z.string().optional(),
  color: z.string().optional(),
});

type TagFormData = z.infer<typeof tagSchema>;

const colorOptions = [
  { value: '', label: 'Mặc định' },
  { value: 'blue', label: 'Xanh dương' },
  { value: 'green', label: 'Xanh lá' },
  { value: 'red', label: 'Đỏ' },
  { value: 'yellow', label: 'Vàng' },
  { value: 'purple', label: 'Tím' },
  { value: 'pink', label: 'Hồng' },
  { value: 'gray', label: 'Xám' },
];

const getBadgeClass = (color?: string) => {
  switch (color) {
    case 'blue': return 'badge-info';
    case 'green': return 'badge-success';
    case 'red': return 'badge-danger';
    case 'yellow': return 'badge-warning';
    case 'purple': return 'bg-purple-100 text-purple-700 dark:bg-purple-900 dark:text-purple-300';
    case 'pink': return 'bg-pink-100 text-pink-700 dark:bg-pink-900 dark:text-pink-300';
    default: return 'badge-neutral';
  }
};

const columns = [
  { key: 'name', header: 'Tên', accessor: 'name' as keyof Tag },
  { key: 'slug', header: 'Slug', accessor: 'slug' as keyof Tag },
  { key: 'color', header: 'Màu', accessor: 'color' as keyof Tag },
  { key: 'documentCount', header: 'Số tài liệu', accessor: 'documentCount' as keyof Tag },
  { key: 'actions', header: 'Thao tác', accessor: 'actions' as keyof Tag },
];

export default function TagsPage() {
  const [tags, setTags] = useState<Tag[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [total, setTotal] = useState(0);
  
  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedTag, setSelectedTag] = useState<Tag | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');

  const { register, handleSubmit, reset, formState: { errors } } = useForm<TagFormData>({
    resolver: zodResolver(tagSchema),
  });

  const fetchTags = async () => {
    setLoading(true);
    try {
      const data = await tagService.getTags();
      setTags(data);
      setTotal(data.length);
    } catch (error) {
      // Mock data
      setTags([
        { id: '1', name: 'Quan trọng', slug: 'quan-trong', color: 'red', documentCount: 12, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
        { id: '2', name: 'Hướng dẫn', slug: 'huong-dan', color: 'blue', documentCount: 8, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
        { id: '3', name: 'Cập nhật', slug: 'cap-nhat', color: 'green', documentCount: 5, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
        { id: '4', name: 'Thông báo', slug: 'thong-bao', color: 'yellow', documentCount: 3, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
      ]);
      setTotal(4);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTags();
  }, [page]);

  const handleCreate = async (data: TagFormData) => {
    setSubmitting(true);
    try {
      await tagService.createTag(data);
      setModalOpen(false);
      reset();
      fetchTags();
    } catch (error) {
      console.error('Create tag error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleUpdate = async (data: TagFormData) => {
    if (!selectedTag) return;
    setSubmitting(true);
    try {
      await tagService.updateTag(selectedTag.id, data);
      setModalOpen(false);
      setSelectedTag(null);
      reset();
      fetchTags();
    } catch (error) {
      console.error('Update tag error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async () => {
    if (!selectedTag) return;
    setSubmitting(true);
    try {
      await tagService.deleteTag(selectedTag.id);
      setDeleteModalOpen(false);
      setSelectedTag(null);
      fetchTags();
    } catch (error) {
      console.error('Delete tag error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const openCreateModal = () => {
    setSelectedTag(null);
    reset();
    setModalOpen(true);
  };

  const openEditModal = (tag: Tag) => {
    setSelectedTag(tag);
    reset({ name: tag.name, slug: tag.slug, color: tag.color });
    setModalOpen(true);
  };

  const openDeleteModal = (tag: Tag) => {
    setSelectedTag(tag);
    setDeleteModalOpen(true);
  };

  const filteredTags = tags.filter(tag => 
    tag.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    tag.slug.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const tableData = filteredTags.map((tag) => ({
    ...tag,
    color: (
      <span className={`badge ${getBadgeClass(tag.color)}`}>
        {colorOptions.find(c => c.value === tag.color)?.label || 'Mặc định'}
      </span>
    ),
    actions: (
      <div className="flex items-center gap-2">
        <button
          onClick={() => openEditModal(tag)}
          className="p-1.5 text-muted hover:text-primary hover:bg-surface-alt rounded transition"
          title="Sửa"
        >
          <Edit className="w-4 h-4" />
        </button>
        <button
          onClick={() => openDeleteModal(tag)}
          className="p-1.5 text-muted hover:text-danger hover:bg-surface-alt rounded transition"
          title="Xóa"
        >
          <Trash2 className="w-4 h-4" />
        </button>
      </div>
    ),
  }));

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div className="page-header">
        <div>
          <h1 className="text-2xl font-bold text-foreground">Quản lý Tags</h1>
          <p className="text-muted mt-1">Quản lý các tag cho tài liệu</p>
        </div>
        <button onClick={openCreateModal} className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition">
          <Plus className="w-4 h-4" />
          Thêm tag
        </button>
      </div>

      {/* Search */}
      <div className="card p-4">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted" />
          <input
            type="text"
            placeholder="Tìm kiếm tag..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="input pl-10"
          />
        </div>
      </div>

      {/* Tags Table */}
      {loading ? <PageLoading /> : (
        <PaginatedTable
          data={tableData}
          columns={columns}
          page={page}
          pageSize={pageSize}
          total={total}
          onPageChange={setPage}
        />
      )}

      {/* Create/Edit Modal */}
      <Modal
        open={modalOpen}
        onClose={() => { setModalOpen(false); setSelectedTag(null); }}
        title={selectedTag ? 'Sửa tag' : 'Thêm tag'}
        size="sm"
        footer={
          <>
            <button
              onClick={() => { setModalOpen(false); setSelectedTag(null); }}
              className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
            >
              Hủy
            </button>
            <button
              onClick={handleSubmit(selectedTag ? handleUpdate : handleCreate)}
              disabled={submitting}
              className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
            >
              {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
              {selectedTag ? 'Cập nhật' : 'Tạo mới'}
            </button>
          </>
        }
      >
        <form className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Tên tag</label>
            <input
              {...register('name')}
              className={`input ${errors.name ? 'input-error' : ''}`}
              placeholder="Nhập tên tag"
            />
            {errors.name && <p className="text-danger text-xs mt-1">{errors.name.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Slug (URL)</label>
            <input
              {...register('slug')}
              className="input"
              placeholder="ten-tag-url (tùy chọn)"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Màu sắc</label>
            <select {...register('color')} className="input">
              {colorOptions.map((opt) => (
                <option key={opt.value} value={opt.value}>{opt.label}</option>
              ))}
            </select>
          </div>
        </form>
      </Modal>

      {/* Delete Confirmation Modal */}
      <ConfirmModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        title="Xóa tag"
        message={`Bạn có chắc chắn muốn xóa tag "${selectedTag?.name}" không?`}
        confirmText="Xóa"
        onConfirm={handleDelete}
        danger
      />
    </div>
  );
}
