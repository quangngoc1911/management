// filepath: src/app/(admin)/admin/dashboard/page.tsx
'use client'

import { useState, useEffect } from 'react'
import { 
  Users, 
  FileText, 
  FolderTree, 
  Tags, 
  TrendingUp,
  Clock,
  Eye
} from 'lucide-react'
import Link from 'next/link'
import { api } from '@/shared/lib/axiosInstance'
import { ApiResponse, DashboardStats, Document } from '@/shared/types/api'

// Mock data for demo
const mockStats: DashboardStats = {
  totalUsers: 156,
  totalDocuments: 1243,
  totalCategories: 28,
  totalTags: 85,
  recentDocuments: [],
  documentsByStatus: {
    draft: 45,
    published: 1156,
    archived: 42,
  },
}

const mockRecentDocuments: Document[] = [
  {
    id: '1',
    title: 'Hướng dẫn sử dụng hệ thống',
    content: '',
    categoryId: '1',
    categoryName: 'Hướng dẫn',
    tags: [],
    authorId: '1',
    authorName: 'Admin',
    status: 'published',
    viewCount: 1250,
    createdAt: '2024-01-15T10:30:00Z',
    updatedAt: '2024-01-15T10:30:00Z',
  },
  {
    id: '2',
    title: 'Quy trình phê duyệt tài liệu',
    content: '',
    categoryId: '2',
    categoryName: 'Quy trình',
    tags: [],
    authorId: '1',
    authorName: 'Admin',
    status: 'published',
    viewCount: 890,
    createdAt: '2024-01-14T09:00:00Z',
    updatedAt: '2024-01-14T09:00:00Z',
  },
  {
    id: '3',
    title: 'Chính sách bảo mật thông tin',
    content: '',
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
]

interface StatCardProps {
  title: string
  value: number
  icon: React.ElementType
  color: string
  href?: string
}

function StatCard({ title, value, icon: Icon, color, href }: StatCardProps) {
  const content = (
    <div className="bg-white rounded-xl border border-gray-200 p-6 hover:shadow-md transition-shadow">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm text-gray-500 mb-1">{title}</p>
          <p className="text-3xl font-bold text-gray-800">{value.toLocaleString()}</p>
        </div>
        <div className={`w-12 h-12 rounded-lg flex items-center justify-center ${color}`}>
          <Icon className="w-6 h-6 text-white" />
        </div>
      </div>
    </div>
  )

  if (href) {
    return <Link href={href}>{content}</Link>
  }
  return content
}

function formatDate(dateString: string) {
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}

export default function DashboardPage() {
  const [stats, setStats] = useState<DashboardStats>(mockStats)
  const [recentDocs, setRecentDocs] = useState<Document[]>(mockRecentDocuments)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    // Simulate API call
    const fetchDashboardData = async () => {
      try {
        // In production, uncomment this:
        // const response = await api.get<ApiResponse<DashboardStats>>('/dashboard/stats')
        // setStats(response.data.data)
        
        // Use mock data for now
        setTimeout(() => {
          setStats(mockStats)
          setRecentDocs(mockRecentDocuments)
          setLoading(false)
        }, 500)
      } catch (error) {
        console.error('Failed to fetch dashboard data:', error)
        setLoading(false)
      }
    }

    fetchDashboardData()
  }, [])

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
      </div>
    )
  }

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div>
        <h1 className="text-2xl font-bold text-gray-800">Dashboard</h1>
        <p className="text-gray-500 mt-1">Welcome back! Here&apos;s an overview of your system.</p>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <StatCard
          title="Total Users"
          value={stats.totalUsers}
          icon={Users}
          color="bg-blue-500"
          href="/admin/users"
        />
        <StatCard
          title="Total Documents"
          value={stats.totalDocuments}
          icon={FileText}
          color="bg-green-500"
          href="/admin/documents"
        />
        <StatCard
          title="Categories"
          value={stats.totalCategories}
          icon={FolderTree}
          color="bg-purple-500"
          href="/admin/categories"
        />
        <StatCard
          title="Tags"
          value={stats.totalTags}
          icon={Tags}
          color="bg-orange-500"
          href="/admin/tags"
        />
      </div>

      {/* Document Status Overview */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="bg-white rounded-xl border border-gray-200 p-6">
          <div className="flex items-center gap-3 mb-4">
            <div className="w-3 h-3 rounded-full bg-green-500"></div>
            <h3 className="font-medium text-gray-800">Published</h3>
          </div>
          <p className="text-3xl font-bold text-gray-800">{stats.documentsByStatus.published}</p>
          <p className="text-sm text-gray-500 mt-1">Active documents</p>
        </div>
        <div className="bg-white rounded-xl border border-gray-200 p-6">
          <div className="flex items-center gap-3 mb-4">
            <div className="w-3 h-3 rounded-full bg-yellow-500"></div>
            <h3 className="font-medium text-gray-800">Draft</h3>
          </div>
          <p className="text-3xl font-bold text-gray-800">{stats.documentsByStatus.draft}</p>
          <p className="text-sm text-gray-500 mt-1">Pending review</p>
        </div>
        <div className="bg-white rounded-xl border border-gray-200 p-6">
          <div className="flex items-center gap-3 mb-4">
            <div className="w-3 h-3 rounded-full bg-gray-400"></div>
            <h3 className="font-medium text-gray-800">Archived</h3>
          </div>
          <p className="text-3xl font-bold text-gray-800">{stats.documentsByStatus.archived}</p>
          <p className="text-sm text-gray-500 mt-1">Old documents</p>
        </div>
      </div>

      {/* Recent Documents */}
      <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
        <div className="flex items-center justify-between p-6 border-b">
          <h2 className="text-lg font-semibold text-gray-800">Recent Documents</h2>
          <Link 
            href="/admin/documents" 
            className="text-sm text-blue-600 hover:text-blue-700 font-medium"
          >
            View all →
          </Link>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Title</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Category</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Views</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Date</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {recentDocs.map((doc) => (
                <tr key={doc.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4">
                    <Link 
                      href={`/admin/documents/${doc.id}`}
                      className="text-sm font-medium text-gray-800 hover:text-blue-600"
                    >
                      {doc.title}
                    </Link>
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
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
}