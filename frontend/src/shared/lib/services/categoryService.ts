import { api } from '../axiosInstance';

export interface Category {
  id: string;
  name: string;
  slug: string;
  description?: string;
  parentId?: string;
  children?: Category[];
  documentCount?: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCategoryRequest {
  name: string;
  slug?: string;
  description?: string;
  parentId?: string;
}

export interface UpdateCategoryRequest {
  name?: string;
  slug?: string;
  description?: string;
  parentId?: string;
}

export const categoryService = {
  getCategories: async (): Promise<Category[]> => {
    const res = await api.get('/categories');
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  getCategory: async (id: string): Promise<Category> => {
    const res = await api.get(`/categories/${id}`);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  createCategory: async (data: CreateCategoryRequest): Promise<Category> => {
    const res = await api.post('/categories', data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  updateCategory: async (id: string, data: UpdateCategoryRequest): Promise<Category> => {
    const res = await api.put(`/categories/${id}`, data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  deleteCategory: async (id: string): Promise<void> => {
    const res = await api.delete(`/categories/${id}`);
    if (!res.data.success) {
      throw new Error(res.data.message);
    }
  },
};

export default categoryService;
