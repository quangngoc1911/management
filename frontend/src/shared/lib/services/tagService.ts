import { api } from '../axiosInstance';

export interface Tag {
  id: string;
  name: string;
  slug: string;
  color?: string;
  documentCount?: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTagRequest {
  name: string;
  slug?: string;
  color?: string;
}

export interface UpdateTagRequest {
  name?: string;
  slug?: string;
  color?: string;
}

export const tagService = {
  getTags: async (): Promise<Tag[]> => {
    const res = await api.get('/tags');
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  getTag: async (id: string): Promise<Tag> => {
    const res = await api.get(`/tags/${id}`);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  createTag: async (data: CreateTagRequest): Promise<Tag> => {
    const res = await api.post('/tags', data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  updateTag: async (id: string, data: UpdateTagRequest): Promise<Tag> => {
    const res = await api.put(`/tags/${id}`, data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  deleteTag: async (id: string): Promise<void> => {
    const res = await api.delete(`/tags/${id}`);
    if (!res.data.success) {
      throw new Error(res.data.message);
    }
  },
};

export default tagService;
