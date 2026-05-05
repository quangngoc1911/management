// filepath: src/app/(admin)/admin/documents/page.tsx
'use client'

import { useState, useEffect } from 'react'
import { 
  Plus, 
  Search, 
  Edit2, 
  Trash2, 
  Eye,
  FileText,
  Filter,
  MoreVertical,
  Send,
  Archive,
  RotateCcw
} from 'lucide-react'
import Link from 'next/link'
import { Document, CreateDocumentRequest, UpdateDocumentRequest, Category, Tag } from '@/shared/types/api'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'

// Validation schema
const documentSchema = z.object({
  title: z.string().min(2, 'Title is required'),
  content: z.string().min(10, 'Content must be at least 10 characters'),
  categoryId: z.string().min(1, 'Category is required'),
  status: z.enum(['draft', 'published', 'archived']).optional(),
})

type DocumentFormData = z.infer<typeof documentSchema>

// Mock data
const mockCategories: Category[] = [
  { id: '1', name: 'Hướng dẫn', slug: 'huong-dan', createdAt: '', updatedAt: '' },
  { id: '2', name: 'Quy trình', slug: 'quy-trinh', createdAt: '', updatedAt: '' },
  { id: '3', name: 'Chính sách', slug: 'chinh-sach', createdAt: '', updatedAt: '' },
]

const mockTags: Tag[] = [
  { id: '1', name: 'Important', slug: 'important', color: '#ef4444', createdAt: '', updatedAt: '' },
  { id: '2', name: 'Urgent', slug: 'urgent', color: '#f97316', createdAt: '', updatedAt: '' },
  { id: '3', name: 'Reference', slug: 'reference', color: '#3b82f6', createdAt: '', updatedAt: '' },
]

const mockDocuments: Document[] = [
  {
    id: '1',
    title: 'Hướng dẫn sử dụng hệ thống quản lý tài liệu',
    content: 'Tài liệu hướng dẫn chi tiết cách sử dụng hệ thống quản lý tài liệu...',
    categoryId: '1',
    categoryName: 'Hướng dẫn',
    tags: [mockTags[0], mockTags[2]],
    authorId: '1',
    authorName: 'Admin',
    status: 'published',
    viewCount: 1250,
    createdAt: '2024-01-15T10:30:00Z',
    updatedAt: '2024-01-15T10:30:00Z',
  },
  {
    id: '2',
    title: 'Quy trình phê duyệt tài liệu mới',
    content: 'Quy trình chi tiết để phê duyệt các tài liệu mới trong hệ thống...',
    categoryId: '2',
    categoryName: 'Quy trình',
    tags: [mockTags[1]],
    authorId: '1',
    authorName: 'Admin',
    status: 'published',
    viewCount: 890,
    createdAt: '2024-01-14T09:00:00Z',
    updatedAt: '2024-01-14T09:00:00Z',
  },
  {
    id: '3',
    title: 'Chính sách bảo mật thông tin năm 2024',
    content: 'Chính sách bảo mật thông tin mới nhất của công ty...',
    categoryId: '3',
    categoryName: 'Chính sách',
    tags: [],
    authorId: '2',
    authorName: 'Manager',
    status: 'draft',
    viewCount: 0,
    createdAt: '2024-01-13T14:20:00Z',
    updatedAt: '2024-01-13T14:20:00Z',
  },
  {
    id: '4',
    title: 'Biểu mẫu yêu cầu cấp quyền truy cập',
    content: 'Biểu mẫu chuẩn để yêu cầu cấp quyền truy cập hệ thống...',
    categoryId: '1',
    categoryName: 'Hướng dẫn',
    tags: [mockTags[2]],
    authorId: '1',
    authorName: 'Admin',
    status: 'archived',
    viewCount: 456,
    createdAt: '2024-01-10T08:00:00Z',
    updatedAt: '2024-01-12T10:00:00Z',
  },
]

function DocumentModal({
  isOpen,
  onClose,
  document,
  categories,
  onSubmit,
}: {
  isOpen: boolean
  onClose: () => void
  document?: Document | null
  categories: Category[]
  onSubmit: (data: DocumentFormData) => void
}) {
  const { register, handleSubmit, formState: { errors }, reset } = useForm<DocumentFormData>({
    resolver: zodResolver(documentSchema),
    defaultValues: document ? {
      title: document.title,
      content: document.content,
      categoryId: document.categoryId,
      status: document.status,
    } : {
      title: '',
      content: '',
      categoryId: '',
      status: 'draft',
    },
  })

  useEffect(() => {
    if (document) {
      reset({
        title: document.title,
        content: document.content,
        categoryId: document.categoryId,
        status: document.status,
      })
    } else {
      reset({
        title: '',
        content: '',
        categoryId: '',
        status: 'draft',
      })
    }
  }, [document, reset])

  if (!isOpen) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/50" onClick={onClose}></div>
      <div className="relative bg-white rounded-xl shadow-xl w-full max-w-2xl mx-4 p-6 max-h-[90vh] overflow-y-auto">
        <h2 className="text-xl font-semibold text-gray-800 mb-6">
          {document ? 'Edit Document' : 'Create New Document'}
        </h2>
        
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Title
            </label>
            <input
              {...register('title')}
              type="text"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Enter document title"
            />
            {errors.title && (
              <p className="text-sm text-red-600 mt-1">{errors.title.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Category
            </label>
            <select
              {...register('categoryId')}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value="">Select category</option>
              {categories.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.name}
                </option>
              ))}
            </select>
            {errors.categoryId && (
              <p className="text-sm text-red-600 mt-1">{errors.categoryId.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Content
            </label>
            <textarea
              {...register('content')}
              rows={10}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Enter document content"
            />
            {errors.content && (
              <p className="text-sm text-red-600 mt-1">{errors.content.message}</p>
            )}
          </div>

          {document && (
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Status
              </label>
              <select
                {...register('status')}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              >
                <option value="draft">Draft</option>
                <option value="published">Published</option>
                <option value="archived">Archived</option>
              </select>
            </div>
          )}

          <div className="flex gap-3 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
            >
              {document ? 'Update' : 'Create'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}

function DeleteConfirmModal({
  isOpen,
  onClose,
  onConfirm,
  documentTitle,
}: {
  isOpen: boolean
  onClose: () => void
  onConfirm: () => void
  documentTitle: string
}) {
  if (!isOpen) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/50" onClick={onClose}></div>
      <div className="relative bg-white rounded-xl shadow-xl w-full max-w-sm mx-4 p-6">
        <h2 className="text-xl font-semibold text-gray-800 mb-2">Delete Document</h2>
        <p className="text-gray-600 mb-6">
          Are you sure you want to delete <strong>&quot;{documentTitle}&quot;</strong>? This action cannot be undone.
        </p>
        <div className="flex gap-3">
          <button
            onClick={onClose}
            className="flex-1 px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50"
          >
            Cancel
          </button>
          <button
            onClick={onConfirm}
            className="flex-1 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  )
}

function formatDate(dateString: string) {
  return new Date(dateString).toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}

export default function DocumentsPage() {
  const [documents, setDocuments] = useState<Document[]>(mockDocuments)
  const [categories] = useState<Category[]>(mockCategories)
  const [loading, setLoading] = useState(false)
  const [searchQuery, setSearchQuery] = useState('')
  const [statusFilter, setStatusFilter] = useState<string>('all')
  const [currentPage, setCurrentPage] = useState(1)
  const [pageSize] = useState(10)

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [selectedDocument, setSelectedDocument] = useState<Document | null>(null)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [documentToDelete, setDocumentToDelete] = useState<Document | null>(null)

  // Filter documents
  const filteredDocuments = documents.filter(doc => {
    const matchesSearch = doc.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      doc.content.toLowerCase().includes(searchQuery.toLowerCase())
    const matchesStatus = statusFilter === 'all' || doc.status === statusFilter
    return matchesSearch && matchesStatus
  })

  const paginatedDocuments = filteredDocuments.slice(
    (currentPage - 1) * pageSize,
    currentPage * pageSize
  )

  const handleCreateDocument = (data: DocumentFormData) => {
    const category = categories.find(c => c.id === data.categoryId)
    const newDocument: Document = {
      id: String(Date.now()),
      title: data.title,
      content: data.content,
      categoryId: data.categoryId,
      categoryName: category?.name,
      tags: [],
      authorId: '1',
      authorName: 'Admin',
      status: data.status || 'draft',
      viewCount: 0,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    }
    setDocuments([newDocument, ...documents])
    setIsModalOpen(false)
  }

  const handleUpdateDocument = (data: DocumentFormData) => {
    if (selectedDocument) {
      const category = categories.find(c => c.id === data.categoryId)
      setDocuments(documents.map(doc => 
        doc.id === selectedDocument.id 
          ? { 
              ...doc, 
              ...data, 
              categoryName: category?.name,
              updatedAt: new Date().toISOString() 
            }
          : doc
      ))
    }
    setIsModalOpen(false)
    setSelectedDocument(null)
  }

  const handleDeleteDocument = () => {
    if (documentToDelete) {
      setDocuments(documents.filter(doc => doc.id !== documentToDelete.id))
    }
    setIsDeleteModalOpen(false)
    setDocumentToDelete(null)
  }

  const handlePublish = (doc: Document) => {
    setDocuments(documents.map(d => 
      d.id === doc.id ? { ...d, status: 'published' as const, updatedAt: new Date().toISOString() } : d
    ))
  }

  const handleArchive = (doc: Document) => {
    setDocuments(documents.map(d => 
      d.id === doc.id ? { ...d, status: 'archived' as const, updatedAt: new Date().toISOString() } : d
    ))
  }

  const openEditModal = (document: Document) => {
    setSelectedDocument(document)
    setIsModalOpen(true)
  }

  const openDeleteModal = (document: Document) => {
    setDocumentToDelete(document)
    setIsDeleteModalOpen(true)
  }

  const handleSubmit = (data: DocumentFormData) => {
    if (selectedDocument) {
      handleUpdateDocument(data)
    } else {
      handleCreateDocument(data)
    }
  }

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-800">Documents Management</h1>
          <p className="text-gray-500 mt-1">Create, edit, and manage your documents</p>
        </div>
        <button
          onClick={() => {
            setSelectedDocument(null)
            setIsModalOpen(true)
          }}
          className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        >
          <Plus className="w-4 h-4" />
          Create Document
        </button>
      </div>

      {/* Search and Filters */}
      <div className="bg-white rounded-xl border border-gray-200 p-4">
        <div className="flex flex-col sm:flex-row gap-4">
          <div className="flex-1 relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
            <input
              type="text"
              placeholder="Search documents..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
          <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
            <option value="all">All Status</option>
            <option value="draft">Draft</option>
            <option value="published">Published</option>
            <option value="archived">Archived</option>
          </select>
        </div>
      </div>

      {/* Documents Table */}
      <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Title</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Category</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Views</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Date</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {paginatedDocuments.map((doc) => (
                <tr key={doc.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4">
                    <div className="flex items-center gap-3">
                      <FileText className="w-5 h-5 text-gray-400" />
                      <div>
                        <p className="text-sm font-medium text-gray-800">{doc.title}</p>
                        <p className="text-xs text-gray-500 truncate max-w-xs">
                          {doc.content.substring(0, 50)}...
                        </p>
                      </div>
                    </div>
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-600">
                    {doc.categoryName}
                  </td>
                  <td className="px-6 py-4">
                    <span className={`
                      inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium
                      ${doc.status === 'published' ? 'bg-green-100 text-green-800' : ''}
                      ${doc.status === 'draft' ? 'bg-yellow-100 text-yellow-800' : ''}
                      ${doc.status === 'archived' ? 'bg-gray-100 text-gray-800' : ''}
                    `}>
                      {doc.status}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-600">
                    <div className="flex items-center gap-1">
                      <Eye className="w-4 h-4" />
                      {doc.viewCount}
                    </div>
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-600">
                    {formatDate(doc.createdAt)}
                  </td>
                  <td className="px-6 py-4">
                    <div className="flex items-center justify-end gap-1">
                      {doc.status === 'draft' && (
                        <button
                          onClick={() => handlePublish(doc)}
                          className="p-1.5 text-gray-500 hover:text-green-600 hover:bg-green-50 rounded"
                          title="Publish"
                        >
                          <Send className="w-4 h-4" />
                        </button>
                      )}
                      {(doc.status === 'published' || doc.status === 'archived') && (
                        <button
                          onClick={() => handleArchive(doc)}
                          className="p-1.5 text-gray-500 hover:text-gray-600 hover:bg-gray-100 rounded"
                          title={doc.status === 'archived' ? 'Unarchive' : 'Archive'}
                        >
                          {doc.status === 'archived' ? (
                            <RotateCcw className="w-4 h-4" />
                          ) : (
                            <Archive className="w-4 h-4" />
                          )}
                        </button>
                      )}
                      <button
                        onClick={() => openEditModal(doc)}
                        className="p-1.5 text-gray-500 hover:text-blue-600 hover:bg-blue-50 rounded"
                        title="Edit"
                      >
                        <Edit2 className="w-4 h-4" />
                      </button>
                      <button
                        onClick={() => openDeleteModal(doc)}
                        className="p-1.5 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded"
                        title="Delete"
                      >
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Pagination */}
        <div className="px-6 py-4 border-t flex items-center justify-between">
          <p className="text-sm text-gray-600">
            Showing {((currentPage - 1) * pageSize) + 1} to {Math.min(currentPage * pageSize, filteredDocuments.length)} of {filteredDocuments.length} results
          </p>
          <div className="flex gap-2">
            <button
              onClick={() => setCurrentPage(p => Math.max(1, p - 1))}
              disabled={currentPage === 1}
              className="px-3 py-1 text-sm border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50"
            >
              Previous
            </button>
            <button
              onClick={() => setCurrentPage(p => Math.min(Math.ceil(filteredDocuments.length / pageSize), p + 1))}
              disabled={currentPage >= Math.ceil(filteredDocuments.length / pageSize)}
              className="px-3 py-1 text-sm border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50"
            >
              Next
            </button>
          </div>
        </div>
      </div>

      {/* Create/Edit Modal */}
      <DocumentModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false)
          setSelectedDocument(null)
        }}
        document={selectedDocument}
        categories={categories}
        onSubmit={handleSubmit}
      />

      {/* Delete Confirmation Modal */}
      <DeleteConfirmModal
        isOpen={isDeleteModalOpen}
        onClose={() => {
          setIsDeleteModalOpen(false)
          setDocumentToDelete(null)
        }}
        onConfirm={handleDeleteDocument}
        documentTitle={documentToDelete?.title || ''}
      />
    </div>
  )
}