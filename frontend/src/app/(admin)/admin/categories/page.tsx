// filepath: src/app/(admin)/admin/categories/page.tsx
'use client'

import { useState, useEffect } from 'react'
import { 
  Plus, 
  Search, 
  Edit2, 
  Trash2, 
  ChevronRight,
  ChevronDown,
  FolderTree,
  FileText,
  MoreVertical
} from 'lucide-react'
import { Category, CreateCategoryRequest, UpdateCategoryRequest } from '@/shared/types/api'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'

// Validation schema
const categorySchema = z.object({
  name: z.string().min(2, 'Category name is required'),
  slug: z.string().optional(),
  description: z.string().optional(),
  parentId: z.string().optional(),
})

type CategoryFormData = z.infer<typeof categorySchema>

// Mock data with tree structure
const mockCategories: Category[] = [
  {
    id: '1',
    name: 'Hướng dẫn',
    slug: 'huong-dan',
    description: 'Các tài liệu hướng dẫn sử dụng',
    parentId: undefined,
    documentCount: 15,
    createdAt: '2024-01-01T00:00:00Z',
    updatedAt: '2024-01-15T00:00:00Z',
    children: [
      {
        id: '1-1',
        name: 'Hướng dẫn sử dụng',
        slug: 'huong-dan-su-dung',
        description: 'Hướng dẫn sử dụng hệ thống',
        parentId: '1',
        documentCount: 8,
        createdAt: '2024-01-02T00:00:00Z',
        updatedAt: '2024-01-10T00:00:00Z',
      },
      {
        id: '1-2',
        name: 'Hướng dẫn cài đặt',
        slug: 'huong-dan-cai-dat',
        description: 'Hướng dẫn cài đặt',
        parentId: '1',
        documentCount: 7,
        createdAt: '2024-01-03T00:00:00Z',
        updatedAt: '2024-01-12T00:00:00Z',
      },
    ],
  },
  {
    id: '2',
    name: 'Quy trình',
    slug: 'quy-trinh',
    description: 'Các quy trình làm việc',
    parentId: undefined,
    documentCount: 20,
    createdAt: '2024-01-05T00:00:00Z',
    updatedAt: '2024-01-20T00:00:00Z',
    children: [],
  },
  {
    id: '3',
    name: 'Chính sách',
    slug: 'chinh-sach',
    description: 'Các chính sách của công ty',
    parentId: undefined,
    documentCount: 10,
    createdAt: '2024-01-08T00:00:00Z',
    updatedAt: '2024-01-18T00:00:00Z',
  },
]

interface TreeNodeProps {
  category: Category
  level: number
  onEdit: (category: Category) => void
  onDelete: (category: Category) => void
  onAddChild: (parentId: string) => void
}

function TreeNode({ category, level, onEdit, onDelete, onAddChild }: TreeNodeProps) {
  const [expanded, setExpanded] = useState(false)
  const hasChildren = category.children && category.children.length > 0

  return (
    <div>
      <div 
        className="flex items-center gap-2 py-3 px-4 hover:bg-gray-50 border-b"
        style={{ paddingLeft: `${level * 24 + 16}px` }}
      >
        {hasChildren ? (
          <button
            onClick={() => setExpanded(!expanded)}
            className="p-1 hover:bg-gray-200 rounded"
          >
            {expanded ? (
              <ChevronDown className="w-4 h-4 text-gray-500" />
            ) : (
              <ChevronRight className="w-4 h-4 text-gray-500" />
            )}
          </button>
        ) : (
          <div className="w-6"></div>
        )}
        
        <FolderTree className="w-5 h-5 text-blue-500" />
        
        <div className="flex-1 min-w-0">
          <p className="text-sm font-medium text-gray-800">{category.name}</p>
          <p className="text-xs text-gray-500">{category.description}</p>
        </div>
        
        <span className="text-xs text-gray-500 bg-gray-100 px-2 py-1 rounded">
          {category.documentCount} docs
        </span>
        
        <div className="flex items-center gap-1">
          <button
            onClick={() => onAddChild(category.id)}
            className="p-1.5 text-gray-500 hover:text-blue-600 hover:bg-blue-50 rounded"
            title="Add child category"
          >
            <Plus className="w-4 h-4" />
          </button>
          <button
            onClick={() => onEdit(category)}
            className="p-1.5 text-gray-500 hover:text-blue-600 hover:bg-blue-50 rounded"
            title="Edit"
          >
            <Edit2 className="w-4 h-4" />
          </button>
          <button
            onClick={() => onDelete(category)}
            className="p-1.5 text-gray-500 hover:text-red-600 hover:bg-red-50 rounded"
            title="Delete"
          >
            <Trash2 className="w-4 h-4" />
          </button>
        </div>
      </div>
      
      {hasChildren && expanded && (
        <div>
          {category.children!.map((child) => (
            <TreeNode
              key={child.id}
              category={child}
              level={level + 1}
              onEdit={onEdit}
              onDelete={onDelete}
              onAddChild={onAddChild}
            />
          ))}
        </div>
      )}
    </div>
  )
}

function CategoryModal({
  isOpen,
  onClose,
  category,
  categories,
  onSubmit,
}: {
  isOpen: boolean
  onClose: () => void
  category?: Category | null
  categories: Category[]
  onSubmit: (data: CategoryFormData & { parentId?: string }) => void
}) {
  const { register, handleSubmit, formState: { errors }, reset, setValue } = useForm<CategoryFormData>({
    resolver: zodResolver(categorySchema),
    defaultValues: {
      name: '',
      slug: '',
      description: '',
      parentId: undefined,
    },
  })

  useEffect(() => {
    if (category) {
      reset({
        name: category.name,
        slug: category.slug || '',
        description: category.description || '',
        parentId: category.parentId,
      })
    } else {
      reset({
        name: '',
        slug: '',
        description: '',
        parentId: undefined,
      })
    }
  }, [category, reset])

  if (!isOpen) return null

  // Flatten categories for parent selection
  const flattenCategories = (cats: Category[], depth = 0): Category[] => {
    return cats.reduce((acc, cat) => {
      acc.push({ ...cat, name: '  '.repeat(depth) + cat.name })
      if (cat.children) {
        acc.push(...flattenCategories(cat.children, depth + 1))
      }
      return acc
    }, [] as Category[])
  }

  const flatCategories = flattenCategories(categories)

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/50" onClick={onClose}></div>
      <div className="relative bg-white rounded-xl shadow-xl w-full max-w-md mx-4 p-6">
        <h2 className="text-xl font-semibold text-gray-800 mb-6">
          {category ? 'Edit Category' : 'Create New Category'}
        </h2>
        
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Category Name
            </label>
            <input
              {...register('name')}
              type="text"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Enter category name"
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
              placeholder="category-slug"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Parent Category (optional)
            </label>
            <select
              {...register('parentId')}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value="">No parent</option>
              {flatCategories.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.name}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Description
            </label>
            <textarea
              {...register('description')}
              rows={3}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Enter description"
            />
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
              {category ? 'Update' : 'Create'}
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
  categoryName,
}: {
  isOpen: boolean
  onClose: () => void
  onConfirm: () => void
  categoryName: string
}) {
  if (!isOpen) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div className="absolute inset-0 bg-black/50" onClick={onClose}></div>
      <div className="relative bg-white rounded-xl shadow-xl w-full max-w-sm mx-4 p-6">
        <h2 className="text-xl font-semibold text-gray-800 mb-2">Delete Category</h2>
        <p className="text-gray-600 mb-6">
          Are you sure you want to delete <strong>{categoryName}</strong>? This may affect child categories.
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

export default function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>(mockCategories)
  const [loading, setLoading] = useState(false)
  const [searchQuery, setSearchQuery] = useState('')

  // Modal states
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null)
  const [parentIdForNew, setParentIdForNew] = useState<string | undefined>(undefined)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [categoryToDelete, setCategoryToDelete] = useState<Category | null>(null)

  // Filter categories by search
  const filterCategories = (cats: Category[], query: string): Category[] => {
    if (!query) return cats
    return cats.filter(cat => 
      cat.name.toLowerCase().includes(query.toLowerCase()) ||
      cat.description?.toLowerCase().includes(query.toLowerCase())
    ).map(cat => ({
      ...cat,
      children: cat.children ? filterCategories(cat.children, query) : undefined,
    }))
  }

  const filteredCategories = filterCategories(categories, searchQuery)

  const handleCreateCategory = (data: CategoryFormData & { parentId?: string }) => {
    const newCategory: Category = {
      id: String(Date.now()),
      name: data.name,
      slug: data.slug || data.name.toLowerCase().replace(/\s+/g, '-'),
      description: data.description,
      parentId: data.parentId,
      documentCount: 0,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
      children: [],
    }

    if (data.parentId) {
      // Add as child
      const addToParent = (cats: Category[]): Category[] => {
        return cats.map(cat => {
          if (cat.id === data.parentId) {
            return {
              ...cat,
              children: [...(cat.children || []), newCategory],
            }
          }
          if (cat.children) {
            return { ...cat, children: addToParent(cat.children) }
          }
          return cat
        })
      }
      setCategories(addToParent(categories))
    } else {
      setCategories([...categories, newCategory])
    }
    setIsModalOpen(false)
  }

  const handleUpdateCategory = (data: CategoryFormData & { parentId?: string }) => {
    if (selectedCategory) {
      const updateCategory = (cats: Category[]): Category[] => {
        return cats.map(cat => {
          if (cat.id === selectedCategory.id) {
            return {
              ...cat,
              ...data,
              updatedAt: new Date().toISOString(),
            }
          }
          if (cat.children) {
            return { ...cat, children: updateCategory(cat.children) }
          }
          return cat
        })
      }
      setCategories(updateCategory(categories))
    }
    setIsModalOpen(false)
    setSelectedCategory(null)
  }

  const handleDeleteCategory = () => {
    if (categoryToDelete) {
      const deleteFromTree = (cats: Category[]): Category[] => {
        return cats
          .filter(cat => cat.id !== categoryToDelete.id)
          .map(cat => ({
            ...cat,
            children: cat.children ? deleteFromTree(cat.children) : undefined,
          }))
      }
      setCategories(deleteFromTree(categories))
    }
    setIsDeleteModalOpen(false)
    setCategoryToDelete(null)
  }

  const openEditModal = (category: Category) => {
    setSelectedCategory(category)
    setParentIdForNew(undefined)
    setIsModalOpen(true)
  }

  const openAddChildModal = (parentId: string) => {
    setSelectedCategory(null)
    setParentIdForNew(parentId)
    setIsModalOpen(true)
  }

  const openDeleteModal = (category: Category) => {
    setCategoryToDelete(category)
    setIsDeleteModalOpen(true)
  }

  const handleSubmit = (data: CategoryFormData & { parentId?: string }) => {
    if (selectedCategory) {
      handleUpdateCategory(data)
    } else {
      handleCreateCategory({ ...data, parentId: data.parentId || parentIdForNew })
    }
  }

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-800">Categories Management</h1>
          <p className="text-gray-500 mt-1">Organize documents with hierarchical categories</p>
        </div>
        <button
          onClick={() => {
            setSelectedCategory(null)
            setParentIdForNew(undefined)
            setIsModalOpen(true)
          }}
          className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        >
          <Plus className="w-4 h-4" />
          Add Category
        </button>
      </div>

      {/* Search */}
      <div className="bg-white rounded-xl border border-gray-200 p-4">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
          <input
            type="text"
            placeholder="Search categories..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>
      </div>

      {/* Categories Tree */}
      <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
        <div className="p-4 border-b bg-gray-50">
          <h2 className="text-sm font-medium text-gray-600">Category Tree</h2>
        </div>
        
        {loading ? (
          <div className="flex items-center justify-center h-64">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
          </div>
        ) : filteredCategories.length === 0 ? (
          <div className="flex flex-col items-center justify-center h-64 text-gray-500">
            <FolderTree className="w-12 h-12 mb-4 text-gray-300" />
            <p>No categories found</p>
          </div>
        ) : (
          <div>
            {filteredCategories.map((category) => (
              <TreeNode
                key={category.id}
                category={category}
                level={0}
                onEdit={openEditModal}
                onDelete={openDeleteModal}
                onAddChild={openAddChildModal}
              />
            ))}
          </div>
        )}
      </div>

      {/* Create/Edit Modal */}
      <CategoryModal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false)
          setSelectedCategory(null)
          setParentIdForNew(undefined)
        }}
        category={selectedCategory}
        categories={categories}
        onSubmit={handleSubmit}
      />

      {/* Delete Confirmation Modal */}
      <DeleteConfirmModal
        isOpen={isDeleteModalOpen}
        onClose={() => {
          setIsDeleteModalOpen(false)
          setCategoryToDelete(null)
        }}
        onConfirm={handleDeleteCategory}
        categoryName={categoryToDelete?.name || ''}
      />
    </div>
  )
}