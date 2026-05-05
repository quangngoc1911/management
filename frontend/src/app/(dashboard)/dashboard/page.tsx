'use client';

import { useEffect, useState } from 'react';
import { Users, FileText, FolderTree, Tags, TrendingUp, Clock } from 'lucide-react';
import { api } from '@/shared/lib/axiosInstance';
import { PageLoading } from '@/components/LoadingSpinner';

interface DashboardStats {
  totalUsers: number;
  totalDocuments: number;
  totalCategories: number;
  totalTags: number;
  documentsByStatus?: {
    draft: number;
    published: number;
    archived: number;
  };
  recentDocuments?: Array<{
    id: string;
    title: string;
    status: string;
    createdAt: string;
  }>;
}

const statCards = [
  { key: 'users', label: 'Người dùng', icon: Users, color: 'bg-blue-500' },
  { key: 'documents', label: 'Tài liệu', icon: FileText, color: 'bg-green-500' },
  { key: 'categories', label: 'Danh mục', icon: FolderTree, color: 'bg-purple-500' },
  { key: 'tags', label: 'Tags', icon: Tags, color: 'bg-orange-500' },
];

export default function DashboardPage() {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState<DashboardStats>({
    totalUsers: 0,
    totalDocuments: 0,
    totalCategories: 0,
    totalTags: 0,
  });

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const res = await api.get('/dashboard/stats');
        if (res.data.success) {
          setStats(res.data.data);
        }
      } catch (error) {
        console.error('Failed to fetch stats:', error);
        // Use mock data for demo
        setStats({
          totalUsers: 12,
          totalDocuments: 156,
          totalCategories: 24,
          totalTags: 48,
          documentsByStatus: {
            draft: 23,
            published: 118,
            archived: 15,
          },
          recentDocuments: [
            { id: '1', title: 'Hướng dẫn sử dụng hệ thống', status: 'published', createdAt: '2024-01-15' },
            { id: '2', title: 'Quy trình quản lý tài liệu', status: 'published', createdAt: '2024-01-14' },
            { id: '3', title: 'Chính sách bảo mật', status: 'draft', createdAt: '2024-01-13' },
          ],
        });
      } finally {
        setLoading(false);
      }
    };

    fetchStats();
  }, []);

  if (loading) {
    return <PageLoading />;
  }

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div>
        <h1 className="text-2xl font-bold text-foreground">Tổng quan</h1>
        <p className="text-muted mt-1">Chào mừng bạn đến với hệ thống quản lý tài liệu</p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {statCards.map((card) => {
          const Icon = card.icon;
          const value = stats[card.key as keyof DashboardStats] as number;
          
          return (
            <div key={card.key} className="card p-6">
              <div className="flex items-center gap-4">
                <div className={`${card.color} p-3 rounded-lg`}>
                  <Icon className="w-6 h-6 text-white" />
                </div>
                <div>
                  <p className="text-2xl font-bold text-foreground">{value || 0}</p>
                  <p className="text-sm text-muted">{card.label}</p>
                </div>
              </div>
            </div>
          );
        })}
      </div>

      {/* Documents by Status */}
      {stats.documentsByStatus && (
        <div className="card p-6">
          <h2 className="text-lg font-semibold text-foreground mb-4">Tài liệu theo trạng thái</h2>
          <div className="grid grid-cols-3 gap-4">
            <div className="text-center p-4 bg-surface-alt rounded-lg">
              <p className="text-2xl font-bold text-foreground">{stats.documentsByStatus.draft}</p>
              <p className="text-sm text-muted">Bản nháp</p>
            </div>
            <div className="text-center p-4 bg-surface-alt rounded-lg">
              <p className="text-2xl font-bold text-success">{stats.documentsByStatus.published}</p>
              <p className="text-sm text-muted">Đã xuất bản</p>
            </div>
            <div className="text-center p-4 bg-surface-alt rounded-lg">
              <p className="text-2xl font-bold text-muted">{stats.documentsByStatus.archived}</p>
              <p className="text-sm text-muted">Lưu trữ</p>
            </div>
          </div>
        </div>
      )}

      {/* Recent Documents */}
      <div className="card overflow-hidden">
        <div className="flex items-center justify-between px-6 py-4 border-b border-border">
          <h2 className="text-lg font-semibold text-foreground">Tài liệu gần đây</h2>
          <a href="/documents" className="text-sm text-primary hover:text-primary-hover">
            Xem tất cả
          </a>
        </div>
        
        {stats.recentDocuments && stats.recentDocuments.length > 0 ? (
          <div className="divide-y divide-border">
            {stats.recentDocuments.map((doc) => (
              <div key={doc.id} className="flex items-center justify-between px-6 py-4 hover:bg-surface-alt transition">
                <div className="flex items-center gap-3">
                  <FileText className="w-5 h-5 text-muted" />
                  <div>
                    <p className="font-medium text-foreground">{doc.title}</p>
                    <div className="flex items-center gap-2 text-xs text-muted">
                      <Clock className="w-3 h-3" />
                      <span>{new Date(doc.createdAt).toLocaleDateString('vi-VN')}</span>
                    </div>
                  </div>
                </div>
                <span className={`badge ${
                  doc.status === 'published' ? 'badge-success' : 
                  doc.status === 'draft' ? 'badge-warning' : 'badge-neutral'
                }`}>
                  {doc.status === 'published' ? 'Đã xuất bản' : 
                   doc.status === 'draft' ? 'Bản nháp' : 'Lưu trữ'}
                </span>
              </div>
            ))}
          </div>
        ) : (
          <div className="px-6 py-10 text-center text-muted">
            Chưa có tài liệu nào
          </div>
        )}
      </div>
    </div>
  );
}
