'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Plus, Edit, Trash2, Search, FileText, Eye, Loader2 } from 'lucide-react';
import { documentService, Document, CreateDocumentRequest, DocumentStatus } from '@/shared/lib/services/documentService';
import { categoryService, Category } from '@/shared/lib/services/categoryService';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';

const documentSchema = z.object({
  title: z.string().min(2, 'Tiêu đề phải có ít nhất 2 ký tự'),
  content: z.string().min(10, 'Nội dung phải có ít nhất 10 ký tự'),
  categoryId: z.string().min(1, 'Vui lòng chọn danh mục'),
  status: z.enum(['draft', 'published', 'archived']).optional(),
});

type DocumentFormData = z.infer<typeof documentSchema>;

const statusLabels: Record<DocumentStatus, string> = {
  draft: 'Bản nháp',
  published: 'Đã xuất bản',
  archived: 'Lưu trữ',
};

const columns = [
  { key: 'title', header: 'Tiêu đề', accessor: 'title' as keyof Document },
  { key: 'categoryName', header: 'Danh mục', accessor: 'categoryName' as keyof Document },
  { key: 'status', header: 'Trạng thái', accessor: 'status' as keyof Document },
  { key: 'viewCount', header: 'Lượt xem', accessor: 'viewCount' as keyof Document },
  { key: 'actions', header: 'Thao tác', accessor: 'actions' as keyof Document },
];

export default function DocumentsPage() {
  const [documents, setDocuments] = useState<Document[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [total, setTotal] = useState(0);
  
  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedDocument, setSelectedDocument] = useState<Document | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');

  const { register, handleSubmit, reset, formState: { errors } } = useForm<DocumentFormData>({
    resolver: zodResolver(documentSchema),
  });

  const fetchDocuments = async () => {
    setLoading(true);
    try {
      const data = await documentService.getDocuments({ page, pageSize, search: searchQuery });
      setDocuments(data.items);
      setTotal(data.total);
    } catch (error) {
      // Mock data
      setDocuments([
        { id: '1', title: 'Hướng dẫn sử dụng hệ thống', content: 'Nội dung...', categoryId: '1', categoryName: 'Tài liệu kỹ thuật', tags: [], authorId: '1', status: 'published', viewCount: 156, createdAt: '2024-01-15', updatedAt: '2024-01-15' },
        { id: '2', title: 'Quy trình quản lý tài liệu', content: 'Nội dung...', categoryId: '1', categoryName: 'Tài liệu kỹ thuật', tags: [], authorId: '1', status: 'published', viewCount: 89, createdAt: '2024-01-14', updatedAt: '2024-01-14' },
        { id: '3', title: 'Chính sách bảo mật', content: 'Nội dung...', categoryId: '2', categoryName: 'Tài liệu pháp luật', tags: [], authorId: '1', status: 'draft', viewCount: 23, createdAt: '2024-01-13', updatedAt: '2024-01-13' },
      ]);
      setTotal(3);
    } finally {
      setLoading(false);
    }
  };

  const fetchCategories = async () => {
    try {
      const data = await categoryService.getCategories();
      setCategories(data);
    } catch (error) {
      setCategories([
        { id: '1', name: 'Tài liệu kỹ thuật', slug: 'ki-thuat', documentCount: 42, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
        { id: '2', name: 'Tài liệu pháp luật', slug: 'phap-luat', documentCount: 25, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
      ]);
    }
  };

  useEffect(() => {
    fetchCategories();
    fetchDocuments();
  }, [page, searchQuery]);

  const handleCreate = async (data: DocumentFormData) => {
    setSubmitting(true);
    try {
      await documentService.createDocument({
        title: data.title,
        content: data.content,
        categoryId: data.categoryId,
      });
      setModalOpen(false);
      reset();
      fetchDocuments();
    } catch (error) {
      console.error('Create document error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleUpdate = async (data: DocumentFormData) => {
    if (!selectedDocument) return;
    setSubmitting(true);
    try {
      await documentService.updateDocument(selectedDocument.id, data);
      setModalOpen(false);
      setSelectedDocument(null);
      reset();
      fetchDocuments();
    } catch (error) {
      console.error('Update document error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async () => {
    if (!selectedDocument) return;
    setSubmitting(true);
    try {
      await documentService.deleteDocument(selectedDocument.id);
      setDeleteModalOpen(false);
      setSelectedDocument(null);
      fetchDocuments();
    } catch (error) {
      console.error('Delete document error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const openCreateModal = () => {
    setSelectedDocument(null);
    reset();
    setModalOpen(true);
  };

  const openEditModal = (doc: Document) => {
    setSelectedDocument(doc);
    reset({ title: doc.title, content: doc.content, categoryId: doc.categoryId, status: doc.status });
    setModalOpen(true);
  };

  const openDeleteModal = (doc: Document) => {
    setSelectedDocument(doc);
    setDeleteModalOpen(true);
  };

  const tableData = documents.map((doc) => ({
    ...doc,
    status: (
      <span className={`badge ${
        doc.status === 'published' ? 'badge-success' : 
        doc.status === 'draft' ? 'badge-warning' : 'badge-neutral'
      }`}>
        {statusLabels[doc.status]}
      </span>
    ),
    actions: (
      <div className="flex items-center gap-2">
        <button
          onClick={() => openEditModal(doc)}
          className="p-1.5 text-muted hover:text-primary hover:bg-surface-alt rounded transition"
          title="Sửa"
        >
          <Edit className="w-4 h-4" />
        </button>
        <button
          onClick={() => openDeleteModal(doc)}
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
          <h1 className="text-2xl font-bold text-foreground">Quản lý tài liệu</h1>
          <p className="text-muted mt-1">Quản lý các tài liệu trong hệ thống</p>
        </div>
        <button onClick={openCreateModal} className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition">
          <Plus className="w-4 h-4" />
          Thêm tài liệu
        </button>
      </div>

      {/* Search */}
      <div className="card p-4">
        <div className="relative max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted" />
          <input
            type="text"
            placeholder="Tìm kiếm tài liệu..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="input pl-10"
          />
        </div>
      </div>

      {/* Documents Table */}
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
        onClose={() => { setModalOpen(false); setSelectedDocument(null); }}
        title={selectedDocument ? 'Sửa tài liệu' : 'Thêm tài liệu'}
        size="lg"
        footer={
          <>
            <button
              onClick={() => { setModalOpen(false); setSelectedDocument(null); }}
              className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
            >
              Hủy
            </button>
            <button
              onClick={handleSubmit(selectedDocument ? handleUpdate : handleCreate)}
              disabled={submitting}
              className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
            >
              {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
              {selectedDocument ? 'Cập nhật' : 'Tạo mới'}
            </button>
          </>
        }
      >
        <form className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Tiêu đề</label>
            <input
              {...register('title')}
              className={`input ${errors.title ? 'input-error' : ''}`}
              placeholder="Nhập tiêu đề"
            />
            {errors.title && <p className="text-danger text-xs mt-1">{errors.title.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Danh mục</label>
            <select
              {...register('categoryId')}
              className={`input ${errors.categoryId ? 'input-error' : ''}`}
            >
              <option value="">Chọn danh mục</option>
              {categories.map((cat) => (
                <option key={cat.id} value={cat.id}>{cat.name}</option>
              ))}
            </select>
            {errors.categoryId && <p className="text-danger text-xs mt-1">{errors.categoryId.message}</p>}
          </div>

          <div>
            <label className="block text-sm font-medium text-foreground mb-2">Nội dung</label>
            <textarea
              {...register('content')}
              className={`input resize-none h-48 ${errors.content ? 'input-error' : ''}`}
              placeholder="Nhập nội dung tài liệu"
            />
            {errors.content && <p className="text-danger text-xs mt-1">{errors.content.message}</p>}
          </div>

          {selectedDocument && (
            <div>
              <label className="block text-sm font-medium text-foreground mb-2">Trạng thái</label>
              <select {...register('status')} className="input">
                <option value="draft">Bản nháp</option>
                <option value="published">Đã xuất bản</option>
                <option value="archived">Lưu trữ</option>
              </select>
            </div>
          )}
        </form>
      </Modal>

      {/* Delete Confirmation Modal */}
      <ConfirmModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        title="Xóa tài liệu"
        message={`Bạn có chắc chắn muốn xóa tài liệu "${selectedDocument?.title}" không? Hành động này không thể hoàn tác.`}
        confirmText="Xóa"
        onConfirm={handleDelete}
        danger
      />
    </div>
  );
}
