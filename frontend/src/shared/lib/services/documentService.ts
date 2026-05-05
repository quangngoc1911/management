import { api } from '../axiosInstance';

export type DocumentStatus = 'draft' | 'published' | 'archived';

export interface Document {
  id: string;
  title: string;
  content: string;
  categoryId: string;
  categoryName?: string;
  tags: Array<{ id: string; name: string; slug: string }>;
  authorId: string;
  authorName?: string;
  status: DocumentStatus;
  viewCount: number;
  createdAt: string;
  updatedAt: string;
  publishedAt?: string;
}

export interface CreateDocumentRequest {
  title: string;
  content: string;
  categoryId: string;
  tagIds?: string[];
}

export interface UpdateDocumentRequest {
  title?: string;
  content?: string;
  categoryId?: string;
  tagIds?: string[];
  status?: DocumentStatus;
}

export interface DocumentQueryParams {
  page?: number;
  pageSize?: number;
  categoryId?: string;
  tagId?: string;
  status?: DocumentStatus;
  search?: string;
}

export interface PaginatedDocuments {
  items: Document[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export const documentService = {
  getDocuments: async (params: DocumentQueryParams = {}): Promise<PaginatedDocuments> => {
    const res = await api.get('/documents', { params });
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  getDocument: async (id: string): Promise<Document> => {
    const res = await api.get(`/documents/${id}`);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  createDocument: async (data: CreateDocumentRequest): Promise<Document> => {
    const res = await api.post('/documents', data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  updateDocument: async (id: string, data: UpdateDocumentRequest): Promise<Document> => {
    const res = await api.put(`/documents/${id}`, data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  deleteDocument: async (id: string): Promise<void> => {
    const res = await api.delete(`/documents/${id}`);
    if (!res.data.success) {
      throw new Error(res.data.message);
    }
  },
};

export default documentService;
