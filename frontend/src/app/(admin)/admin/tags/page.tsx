// filepath: src/app/(admin)/admin/tags/page.tsx
'use client'

import { useState, useEffect } from 'react'
import { 
  Plus, 
  Search, 
  Edit2, 
  Trash2, 
  Tag as TagIcon,
  FileText
} from 'lucide-react'
import { Tag, CreateTagRequest, UpdateTagRequest } from '@/shared/types/api'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'

// Validation schema
const tagSchema = z.object({
  name: z.string().min(2, 'Tag name is required'),
  slug: z.string().optional(),
  color: z.string().optional(),
})

type TagFormData = z.infer<typeof tagSchema>

// Predefined colors
const colorOptions = [
  '#ef4444', // red
  '#f97316', // orange
  '#f59e0b', // amber
  '#84cc16', // lime
  '#22c55e', // green
  '#14b8a6', // teal
  '#06b6d4', // cyan
  '#3b82f6', // blue
  '#6366f1', // indigo
  '#8b5cf6', // violet
  '#a855f7', // purple
  '#ec4899', // pink
]

// Mock data
const mockTags: Tag[] = [
  { id: '1', name: 'Important', slug: 'important', color: '#ef4444', documentCount: 15, createdAt: '2024-01-01T00:00:00Z', updatedAt: '2024-01-15T00:00:00Z' },
  { id: '2', name: 'Urgent', slug: 'urgent', color: '#f97316', documentCount: 8, createdAt: '2024-01-05T00:00:00Z', updatedAt: '2024-01-10T00:00:00Z' },
  { id: '3', name: 'Reference', slug: 'reference', color: '#3b82f6', documentCount: 25, createdAt: '2024-01-08T00:00:00Z', updatedAt: '2024-01-18T00:00:00Z' },
  { id: '4', name: 'Draft', slug: 'draft', color: '#6b7280', documentCount: 12, createdAt: '2024-01-10T00:00:00Z', updatedAt: '2024-01-12T00:00:00Z' },
  { id: '5', name: 'Review', slug: 'review', color: '#8b5cf6', documentCount: 5, createdAt: '2024-01-12T00:00:00Z', updatedAt: '2024-01-14T00:00:00Z' },
  { id: '6', name: 'Approved', slug: 'approved', color: '#22c55e', documentCount: 20, createdAt: '2024-01-14T00:00:00Z', updatedAt: '2024-01-16T00:00:00Z' },
]

function TagModal({
  isOpen,
  onClose,
  tag,
  onSubmit,
}: {
  isOpen: boolean
  onClose: () => void
  tag?: Tag | null
  onSubmit: (data: TagFormData) => void
}) {
  const { register, handleSubmit, formState: { errors }, reset, setValue, watch } = useForm<TagFormData>({
    resolver: zodResolver(tagSchema),
    defaultValues: {
      name: '',
      slug: '',
      color: '#3b82f6',
    },
  })

  const selectedColor = watch('color')

  useEffect(() => {
    if (tag) {
      reset({
        name: tag.name,
        slug: tag.slug || '',
        color: tag.color || '#3b82f6',
      })
    } else {
      reset({
        name: '',
        slug: '',
        color: '#3b82f6',
      })
    }
  }, [tag, reset])

  if (!isOpen) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/50" onClick={onClose}></div>
      <div className="relative bg-white rounded-xl shadow-xl w-full max-w-md mx-4 p-6">
        <h2 className="text-xl font-semibold text-gray-800 mb-6">
          {tag ? 'Edit Tag' : 'Create New Tag'}
        </h2>
        
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Tag Name
            </label>
            <input
              {...register('name')}
              type="text"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Enter tag name"
            />
            {errors.name && (
              <p className="text-sm text-red-600 mt-1">{errors.name.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Slug
            </label>
            <input
              {...register('slug')}
              type="text"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="tag-slug"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Color
            </label>
            <div className="flex flex-wrap gap-2">
              {colorOptions.map((color) => (
                <button
                  key={color}
                  type="button"
                  onClick={() => setValue('color', color)}
                  className={`
                    w-8 h-8 rounded-full transition-transform
                    ${selectedColor === color ? 'ring-2 ring-offset-2 ring-gray-400 scale-110' : ''}
                  `}
                  style={{ backgroundColor: color }}
                />
              ))}
            </div>
            <input type="hidden" {...register('color')} value={selectedColor} />
          </div>

          {/* Preview */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Preview
            </label>
            <div className="flex items-center gap-2 p-3 border border-gray-200 rounded-lg">
              <span 
                className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium"
                style={{ 
                  backgroundColor: `${selectedColor}20`,
                  color: selectedColor 
                }}
              >
                <TagIcon className="w-3 h-3 mr-1" />
                {watch('name') || 'Tag name'}
              </span>
            </div>
          </div>

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
              {tag ? 'Update' : 'Create'}
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
  tagName,
}: {
  isOpen: boolean
  onClose: () => void
  onConfirm: () => void
  tagName: string
}) {
  if (!isOpen) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/50" onClick={onClose}></div>
      <div className="relative bg-white rounded-xl shadow-xl w-full max-w-sm mx-4 p-6">
        <h2 className="text-xl font-semibold text-gray-800 mb-2">Delete Tag</h2>
        <p className="text-gray-600 mb-6">
          Are you sure you want to delete <strong>&quot;{tagName}&quot;</strong>? This will remove the tag from all documents.
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

export default function TagsPage() {
  const [tags, setTags] = useState<Tag[]>(mockTags)
  const [loading, setLoading] = useState(false)
  const [searchQuery, setSearchQuery] = useState('')

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [selectedTag, setSelectedTag] = useState<Tag | null>(null)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [tagToDelete, setTagToDelete] = useState<Tag | null>(null)

  // Filter tags
  const filteredTags = tags.filter(tag =>
    tag.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    tag.slug.toLowerCase().includes(searchQuery.toLowerCase())
  )

  const handleCreateTag = (data: TagFormData) => {
    const newTag: Tag = {
      id: String(Date.now()),
      name: data.name,
      slug: data.slug || data.name.toLowerCase().replace(/\s+/g, '-'),
      color: data.color || '#3b82f6',
      documentCount: 0,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    }
    setTags([...tags, newTag])
    setIsModalOpen(false)
  }

  const handleUpdateTag = (data: TagFormData) => {
    if (selectedTag) {
      setTags(tags.map(tag => 
        tag.id === selectedTag.id 
          ? { ...tag, ...data, updatedAt: new Date().toISOString() }
          : tag
      ))
    }
    setIsModalOpen(false)
    setSelectedTag(null)
  }

  const handleDeleteTag = () => {
    if (tagToDelete) {
      setTags(tags.filter(tag => tag.id !== tagToDelete.id))
    }
    setIsDeleteModalOpen(false)
    setTagToDelete(null)
  }

  const openEditModal = (tag: Tag) => {
    setSelectedTag(tag)
    setIsModalOpen(true)
  }

  const openDeleteModal = (tag: Tag) => {
    setTagToDelete(tag)
    setIsDeleteModalOpen(true)
  }

  const handleSubmit = (data: TagFormData) => {
    if (selectedTag) {
      handleUpdateTag(data)
    } else {
      handleCreateTag(data)
    }
  }

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-800">Tags Management</h1>
          <p className="text-gray-500 mt-1">Organize documents with tags</p>
        </div>
        <button
          onClick={() => {
            setSelectedTag(null)
            setIsModalOpen(true)
          }}
          className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        >
          <Plus className="w-4 h-4" />
          Add Tag
        </button>
      </div>

      {/* Search */}
      <div className="bg-white rounded-xl border border-gray-200 p-4">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
          <input
            type="text"
            placeholder="Search tags..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>
      </div>

      {/* Tags Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
        {loading ? (
          Array.from({ length: 8 }).map((_, i) => (
            <div key={i} className="bg-white rounded-xl border border-gray-200 p-4 animate-pulse">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-full bg-gray-200"></div>
                <div className="flex-1">
                  <div className="h-4 bg-gray-200 rounded w-24 mb-2"></div>
                  <div className="h-3 bg-gray-200 rounded w-16"></div>
                </div>
              </div>
            </div>
          ))
        ) : filteredTags.length === 0 ? (
          <div className="col-span-full bg-white rounded-xl border border-gray-200 p-12 text-center">
            <TagIcon className="w-12 h-12 mx-auto text-gray-300 mb-4" />
            <p className="text-gray-600">No tags found</p>
            <button
              onClick={() => setIsModalOpen(true)}
              className="mt-4 text-blue-600 hover:text-blue-700 font-medium"
            >
              Create your first tag
            </button>
          </div>
        ) : (
          filteredTags.map((tag) => (
            <div 
              key={tag.id} 
              className="bg-white rounded-xl border border-gray-200 p-4 hover:shadow-md transition-shadow"
            >
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-3">
                  <div 
                    className="w-10 h-10 rounded-full flex items-center justify-center"
                    style={{ backgroundColor: `${tag.color}20` }}
                  >
                    <TagIcon className="w-5 h-5" style={{ color: tag.color }} />
                  </div>
                  <div>
                    <p className="text-sm font-medium text-gray-800">{tag.name}</p>
                    <p className="text-xs text-gray-500">{tag.slug}</p>
                  </div>
                </div>
                <div className="flex items-center gap-1">
                  <button
                    onClick={() => openEditModal(tag)}
                    className="p-1.5 text-gray-500 hover:text-blue-600 hover:bg-blue-50 rounded"
                    title="Edit"
                  >
                    <Edit2 className="w-4 h-4" />
                  </button>
                  <button
                    onClick={() => openDeleteModal(tag)}
                    className="p-1.5 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded"
                    title="Delete"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
              </div>
              
              <div className="mt-4 pt-4 border-t flex items-center justify-between">
                <div className="flex items-center gap-1 text-sm text-gray-500">
                  <FileText className="w-4 h-4" />
                  {tag.documentCount} documents
                </div>
                <span className="text-xs text-gray-400">
                  {formatDate(tag.updatedAt)}
                </span>
              </div>
            </div>
          ))
        )}
      </div>

      {/* Create/Edit Modal */}
      <TagModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false)
          setSelectedTag(null)
        }}
        tag={selectedTag}
        onSubmit={handleSubmit}
      />

      {/* Delete Confirmation Modal */}
      <DeleteConfirmModal
        isOpen={isDeleteModalOpen}
        onClose={() => {
          setIsDeleteModalOpen(false)
          setTagToDelete(null)
        }}
        onConfirm={handleDeleteTag}
        tagName={tagToDelete?.name || ''}
      />
    </div>
  )
}