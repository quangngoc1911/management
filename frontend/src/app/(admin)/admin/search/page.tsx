// filepath: src/app/(admin)/admin/search/page.tsx
'use client'

import { useState, useEffect, useCallback } from 'react'
import { 
  Search as SearchIcon, 
  FileText, 
  FolderTree, 
  Tag,
  Calendar,
  Eye,
  Filter,
  X
} from 'lucide-react'
import Link from 'next/link'
import { Document, Category, Tag as TagType } from '@/shared/types/api'

// Mock data
const mockCategories: Category[] = [
  { id: '1', name: 'Hướng dẫn', slug: 'huong-dan', createdAt: '', updatedAt: '' },
  { id: '2', name: 'Quy trình', slug: 'quy-trinh', createdAt: '', updatedAt: '' },
  { id: '3', name: 'Chính sách', slug: 'chinh-sach', createdAt: '', updatedAt: '' },
]

const mockTags: TagType[] = [
  { id: '1', name: 'Important', slug: 'important', color: '#ef4444', createdAt: '', updatedAt: '' },
  { id: '2', name: 'Urgent', slug: 'urgent', color: '#f97316', createdAt: '', updatedAt: '' },
  { id: '3', name: 'Reference', slug: 'reference', color: '#3b82f6', createdAt: '', updatedAt: '' },
]

const mockDocuments: Document[] = [
  {
    id: '1',
    title: 'Hướng dẫn sử dụng hệ thống quản lý tài liệu',
    content: 'Tài liệu hướng dẫn chi tiết cách sử dụng hệ thống quản lý tài liệu. Bao gồm các tính năng chính, cách thao tác và xử lý sự cố.',
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
    content: 'Quy trình chi tiết để phê duyệt các tài liệu mới trong hệ thống. Bao gồm các bước từ tạo, review đến phê duyệt.',
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
    content: 'Chính sách bảo mật thông tin mới nhất của công ty. Quy định về bảo vệ dữ liệu và an toàn thông tin.',
    categoryId: '3',
    categoryName: 'Chính sách',
    tags: [],
    authorId: '2',
    authorName: 'Manager',
    status: 'published',
    viewCount: 567,
    createdAt: '2024-01-13T14:20:00Z',
    updatedAt: '2024-01-13T14:20:00Z',
  },
  {
    id: '4',
    title: 'Biểu mẫu yêu cầu cấp quyền truy cập',
    content: 'Biểu mẫu chuẩn để yêu cầu cấp quyền truy cập hệ thống. Hướng dẫn cách điền và nộp biểu mẫu.',
    categoryId: '1',
    categoryName: 'Hướng dẫn',
    tags: [mockTags[2]],
    authorId: '1',
    authorName: 'Admin',
    status: 'published',
    viewCount: 456,
    createdAt: '2024-01-10T08:00:00Z',
    updatedAt: '2024-01-12T10:00:00Z',
  },
  {
    id: '5',
    title: 'Hướng dẫn cài đặt và cấu hình hệ thống',
    content: 'Tài liệu hướng dẫn chi tiết cách cài đặt và cấu hình hệ thống từ đầu đến cuối.',
    categoryId: '1',
    categoryName: 'Hướng dẫn',
    tags: [mockTags[0]],
    authorId: '1',
    authorName: 'Admin',
    status: 'published',
    viewCount: 789,
    createdAt: '2024-01-08T11:00:00Z',
    updatedAt: '2024-01-08T11:00:00Z',
  },
]

function formatDate(dateString: string) {
  return new Date(dateString).toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}

function highlightMatch(text: string, query: string) {
  if (!query) return text
  const regex = new RegExp(`(${query})`, 'gi')
  const parts = text.split(regex)
  return parts.map((part, i) => 
    regex.test(part) ? <mark key={i} className="bg-yellow-200 px-0.5 rounded">{part}</mark> : part
  )
}

export default function SearchPage() {
  const [searchQuery, setSearchQuery] = useState('')
  const [results, setResults] = useState<Document[]>([])
  const [loading, setLoading] = useState(false)
  const [hasSearched, setHasSearched] = useState(false)
  
  // Filters
  const [selectedCategory, setSelectedCategory] = useState<string>('all')
  const [selectedTag, setSelectedTag] = useState<string>('all')
  const [dateRange, setDateRange] = useState<string>('all')

  // Perform search
  const performSearch = useCallback(() => {
    if (!searchQuery.trim()) {
      setResults([])
      setHasSearched(false)
      return
    }

    setLoading(true)
    setHasSearched(true)

    // Simulate API call
    setTimeout(() => {
      const query = searchQuery.toLowerCase()
      const filtered = mockDocuments.filter(doc => {
        const matchesQuery = doc.title.toLowerCase().includes(query) ||
          doc.content.toLowerCase().includes(query) ||
          doc.categoryName?.toLowerCase().includes(query)
        
        const matchesCategory = selectedCategory === 'all' || doc.categoryId === selectedCategory
        const matchesTag = selectedTag === 'all' || doc.tags.some(t => t.id === selectedTag)
        
        return matchesQuery && matchesCategory && matchesTag
      })
      
      setResults(filtered)
      setLoading(false)
    }, 300)
  }, [searchQuery, selectedCategory, selectedTag])

  useEffect(() => {
    performSearch()
  }, [performSearch])

  const clearFilters = () => {
    setSelectedCategory('all')
    setSelectedTag('all')
    setDateRange('all')
  }

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div>
        <h1 className="text-2xl font-bold text-gray-800">Search Documents</h1>
        <p className="text-gray-500 mt-1">Find documents by title, content, or category</p>
      </div>

      {/* Search Box */}
      <div className="bg-white rounded-xl border border-gray-200 p-6">
        <div className="relative">
          <SearchIcon className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400" />
          <input
            type="text"
            placeholder="Search for documents..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full pl-12 pr-4 py-3 text-lg border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
          {searchQuery && (
            <button
              onClick={() => setSearchQuery('')}
              className="absolute right-4 top-1/2 -translate-y-1/2 p-1 hover:bg-gray-100 rounded"
            >
              <X className="w-5 h-5 text-gray-400" />
            </button>
          )}
        </div>

        {/* Filters */}
        <div className="flex flex-wrap gap-4 mt-4">
          <div className="flex items-center gap-2">
            <Filter className="w-4 h-4 text-gray-500" />
            <span className="text-sm text-gray-600">Filters:</span>
          </div>
          
          <select
            value={selectedCategory}
            onChange={(e) => setSelectedCategory(e.target.value)}
            className="px-3 py-1.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
          >
            <option value="all">All Categories</option>
            {mockCategories.map(cat => (
              <option key={cat.id} value={cat.id}>{cat.name}</option>
            ))}
          </select>

          <select
            value={selectedTag}
            onChange={(e) => setSelectedTag(e.target.value)}
            className="px-3 py-1.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
          >
            <option value="all">All Tags</option>
            {mockTags.map(tag => (
              <option key={tag.id} value={tag.id}>{tag.name}</option>
            ))}
          </select>

          <select
            value={dateRange}
            onChange={(e) => setDateRange(e.target.value)}
            className="px-3 py-1.5 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
          >
            <option value="all">Any Time</option>
            <option value="today">Today</option>
            <option value="week">This Week</option>
            <option value="month">This Month</option>
          </select>

          {(selectedCategory !== 'all' || selectedTag !== 'all' || dateRange !== 'all') && (
            <button
              onClick={clearFilters}
              className="text-sm text-blue-600 hover:text-blue-700"
            >
              Clear filters
            </button>
          )}
        </div>
      </div>

      {/* Results */}
      {loading ? (
        <div className="flex items-center justify-center h-64">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        </div>
      ) : hasSearched ? (
        <div className="space-y-4">
          <p className="text-sm text-gray-600">
            Found <strong>{results.length}</strong> result{results.length !== 1 ? 's' : ''} for &quot;<strong>{searchQuery}</strong>&quot;
          </p>

          {results.length === 0 ? (
            <div className="bg-white rounded-xl border border-gray-200 p-12 text-center">
              <SearchIcon className="w-12 h-12 mx-auto text-gray-300 mb-4" />
              <p className="text-gray-600">No documents found matching your search.</p>
              <p className="text-sm text-gray-500 mt-2">Try different keywords or adjust your filters.</p>
            </div>
          ) : (
            <div className="space-y-4">
              {results.map((doc) => (
                <div 
                  key={doc.id} 
                  className="bg-white rounded-xl border border-gray-200 p-6 hover:shadow-md transition-shadow"
                >
                  <div className="flex items-start justify-between">
                    <div className="flex-1 min-w-0">
                      <Link 
                        href={`/admin/documents/${doc.id}`}
                        className="text-lg font-medium text-gray-800 hover:text-blue-600"
                      >
                        {highlightMatch(doc.title, searchQuery)}
                      </Link>
                      
                      <p className="text-sm text-gray-600 mt-2 line-clamp-2">
                        {highlightMatch(doc.content, searchQuery)}
                      </p>

                      <div className="flex flex-wrap items-center gap-4 mt-4">
                        <div className="flex items-center gap-1 text-sm text-gray-500">
                          <FolderTree className="w-4 h-4" />
                          {doc.categoryName}
                        </div>
                        
                        {doc.tags.length > 0 && (
                          <div className="flex items-center gap-2">
                            <Tag className="w-4 h-4 text-gray-400" />
                            {doc.tags.map(tag => (
                              <span 
                                key={tag.id}
                                className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium"
                                style={{ 
                                  backgroundColor: `${tag.color}20`,
                                  color: tag.color 
                                }}
                              >
                                {tag.name}
                              </span>
                            ))}
                          </div>
                        )}

                        <div className="flex items-center gap-1 text-sm text-gray-500">
                          <Eye className="w-4 h-4" />
                          {doc.viewCount} views
                        </div>

                        <div className="flex items-center gap-1 text-sm text-gray-500">
                          <Calendar className="w-4 h-4" />
                          {formatDate(doc.createdAt)}
                        </div>
                      </div>
                    </div>

                    <Link
                      href={`/admin/documents/${doc.id}`}
                      className="ml-4 p-2 text-gray-400 hover:text-blue-600 hover:bg-blue-50 rounded-lg"
                    >
                      <FileText className="w-5 h-5" />
                    </Link>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      ) : (
        <div className="bg-white rounded-xl border border-gray-200 p-12 text-center">
          <SearchIcon className="w-12 h-12 mx-auto text-gray-300 mb-4" />
          <p className="text-gray-600">Enter a search term to find documents</p>
          <p className="text-sm text-gray-500 mt-2">Search by title, content, or category</p>
        </div>
      )}
    </div>
  )
}