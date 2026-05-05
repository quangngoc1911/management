'use client';

import { useEffect, useState } from 'react';
import { Search, FileText, Filter, Calendar, User, Eye, FolderTree } from 'lucide-react';
import { documentService, Document, DocumentStatus } from '@/shared/lib/services/documentService';
import { categoryService, Category } from '@/shared/lib/services/categoryService';
import { tagService, Tag } from '@/shared/lib/services/tagService';
import { PageLoading } from '@/components/LoadingSpinner';

const statusLabels: Record<DocumentStatus, string> = {
  draft: 'Bản nháp',
  published: 'Đã xuất bản',
  archived: 'Lưu trữ',
};

export default function SearchDocumentsPage() {
  const [documents, setDocuments] = useState<Document[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [tags, setTags] = useState<Tag[]>([]);
  const [loading, setLoading] = useState(true);
  
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [selectedTag, setSelectedTag] = useState('');
  const [selectedStatus, setSelectedStatus] = useState<DocumentStatus | ''>('');

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

  const fetchTags = async () => {
    try {
      const data = await tagService.getTags();
      setTags(data);
    } catch (error) {
      setTags([
        { id: '1', name: 'Quan trọng', slug: 'quan-trong', color: 'red', documentCount: 12, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
        { id: '2', name: 'Hướng dẫn', slug: 'huong-dan', color: 'blue', documentCount: 8, createdAt: '2024-01-01', updatedAt: '2024-01-01' },
      ]);
    }
  };

  const searchDocuments = async () => {
    setLoading(true);
    try {
      const data = await documentService.getDocuments({
        search: searchQuery,
        categoryId: selectedCategory || undefined,
        tagId: selectedTag || undefined,
        status: selectedStatus || undefined,
      });
      setDocuments(data.items);
    } catch (error) {
      // Mock data with search filter
      const mockDocs = [
        { id: '1', title: 'Hướng dẫn sử dụng hệ thống', content: 'Nội dung hướng dẫn...', categoryId: '1', categoryName: 'Tài liệu kỹ thuật', tags: [{ id: '2', name: 'Hướng dẫn', slug: 'huong-dan' }], authorId: '1', authorName: 'Admin', status: 'published' as DocumentStatus, viewCount: 156, createdAt: '2024-01-15', updatedAt: '2024-01-15' },
        { id: '2', title: 'Quy trình quản lý tài liệu', content: 'Quy trình...', categoryId: '1', categoryName: 'Tài liệu kỹ thuật', tags: [{ id: '1', name: 'Quan trọng', slug: 'quan-trong' }], authorId: '1', authorName: 'Admin', status: 'published' as DocumentStatus, viewCount: 89, createdAt: '2024-01-14', updatedAt: '2024-01-14' },
        { id: '3', title: 'Chính sách bảo mật', content: 'Chính sách...', categoryId: '2', categoryName: 'Tài liệu pháp luật', tags: [{ id: '1', name: 'Quan trọng', slug: 'quan-trong' }], authorId: '1', authorName: 'Admin', status: 'draft' as DocumentStatus, viewCount: 23, createdAt: '2024-01-13', updatedAt: '2024-01-13' },
      ];
      
      let filtered = mockDocs;
      if (searchQuery) {
        filtered = filtered.filter(d => d.title.toLowerCase().includes(searchQuery.toLowerCase()));
      }
      if (selectedCategory) {
        filtered = filtered.filter(d => d.categoryId === selectedCategory);
      }
      if (selectedStatus) {
        filtered = filtered.filter(d => d.status === selectedStatus);
      }
      
      setDocuments(filtered);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCategories();
    fetchTags();
    searchDocuments();
  }, []);

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    searchDocuments();
  };

  const clearFilters = () => {
    setSearchQuery('');
    setSelectedCategory('');
    setSelectedTag('');
    setSelectedStatus('');
    searchDocuments();
  };

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div>
        <h1 className="text-2xl font-bold text-foreground">Tìm kiếm tài liệu</h1>
        <p className="text-muted mt-1">Tìm kiếm và lọc tài liệu theo nhiều tiêu chí</p>
      </div>

      {/* Search Form */}
      <div className="card p-6">
        <form onSubmit={handleSearch} className="space-y-4">
          {/* Main Search */}
          <div className="flex gap-4">
            <div className="flex-1 relative">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-muted" />
              <input
                type="text"
                placeholder="Nhập từ khóa tìm kiếm..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="input pl-11"
              />
            </div>
            <button
              type="submit"
              className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-6 py-2 rounded-md font-medium transition"
            >
              <Search className="w-4 h-4" />
              Tìm kiếm
            </button>
          </div>

          {/* Filters */}
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <div>
              <label className="block text-sm font-medium text-foreground mb-2">Danh mục</label>
              <select
                value={selectedCategory}
                onChange={(e) => setSelectedCategory(e.target.value)}
                className="input"
              >
                <option value="">Tất cả danh mục</option>
                {categories.map((cat) => (
                  <option key={cat.id} value={cat.id}>{cat.name}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-foreground mb-2">Tag</label>
              <select
                value={selectedTag}
                onChange={(e) => setSelectedTag(e.target.value)}
                className="input"
              >
                <option value="">Tất cả tag</option>
                {tags.map((tag) => (
                  <option key={tag.id} value={tag.id}>{tag.name}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-foreground mb-2">Trạng thái</label>
              <select
                value={selectedStatus}
                onChange={(e) => setSelectedStatus(e.target.value as DocumentStatus)}
                className="input"
              >
                <option value="">Tất cả trạng thái</option>
                <option value="draft">Bản nháp</option>
                <option value="published">Đã xuất bản</option>
                <option value="archived">Lưu trữ</option>
              </select>
            </div>
          </div>

          {/* Clear Filters */}
          {(searchQuery || selectedCategory || selectedTag || selectedStatus) && (
            <button
              type="button"
              onClick={clearFilters}
              className="text-sm text-muted hover:text-foreground"
            >
              Xóa bộ lọc
            </button>
          )}
        </form>
      </div>

      {/* Results */}
      {loading ? (
        <PageLoading />
      ) : documents.length > 0 ? (
        <div className="space-y-4">
          <p className="text-sm text-muted">Tìm thấy {documents.length} kết quả</p>
          
          {documents.map((doc) => (
            <div key={doc.id} className="card p-6 hover:shadow-card transition">
              <div className="flex items-start gap-4">
                <div className="p-3 bg-surface-alt rounded-lg">
                  <FileText className="w-6 h-6 text-primary" />
                </div>
                
                <div className="flex-1 min-w-0">
                  <div className="flex items-start justify-between gap-4">
                    <div>
                      <h3 className="font-semibold text-foreground hover:text-primary transition">
                        <a href={`/documents/${doc.id}`}>{doc.title}</a>
                      </h3>
                      <p className="text-sm text-muted mt-1 line-clamp-2">{doc.content}</p>
                    </div>
                    <span className={`badge shrink-0 ${
                      doc.status === 'published' ? 'badge-success' : 
                      doc.status === 'draft' ? 'badge-warning' : 'badge-neutral'
                    }`}>
                      {statusLabels[doc.status]}
                    </span>
                  </div>
                  
                  <div className="flex flex-wrap items-center gap-4 mt-3 text-sm text-muted">
<span className="flex items-center gap-1">
                      <FolderTree className="w-4 h-4" />
                      {doc.categoryName}
                    </span>
                    
                    {doc.tags && doc.tags.length > 0 && (
                      <div className="flex items-center gap-1">
                        {doc.tags.map((tag) => (
                          <span key={tag.id} className="badge badge-neutral text-xs">
                            {tag.name}
                          </span>
                        ))}
                      </div>
                    )}
                    
                    <span className="flex items-center gap-1">
                      <User className="w-4 h-4" />
                      {doc.authorName}
                    </span>
                    
                    <span className="flex items-center gap-1">
                      <Calendar className="w-4 h-4" />
                      {new Date(doc.createdAt).toLocaleDateString('vi-VN')}
                    </span>
                    
                    <span className="flex items-center gap-1">
                      <Eye className="w-4 h-4" />
                      {doc.viewCount} lượt xem
                    </span>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="card p-10 text-center">
          <Search className="w-12 h-12 text-muted mx-auto mb-4" />
          <p className="text-lg font-medium text-foreground">Không tìm thấy kết quả</p>
          <p className="text-sm text-muted mt-1">Thử thay đổi từ khóa hoặc bộ lọc</p>
        </div>
      )}
    </div>
  );
}
