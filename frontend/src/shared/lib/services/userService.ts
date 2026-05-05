import { api } from '../axiosInstance';

export interface User {
  id: string;
  email: string;
  fullName: string;
  role: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateUserRequest {
  email: string;
  password: string;
  fullName: string;
  role: string;
}

export interface UpdateUserRequest {
  fullName?: string;
  role?: string;
  isActive?: boolean;
}

export interface PaginatedUsers {
  items: User[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export const userService = {
  getUsers: async (page = 1, pageSize = 10): Promise<PaginatedUsers> => {
    const res = await api.get('/users', { params: { page, pageSize } });
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  getUser: async (id: string): Promise<User> => {
    const res = await api.get(`/users/${id}`);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  createUser: async (data: CreateUserRequest): Promise<User> => {
    const res = await api.post('/users', data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  updateUser: async (id: string, data: UpdateUserRequest): Promise<User> => {
    const res = await api.put(`/users/${id}`, data);
    if (res.data.success) {
      return res.data.data;
    }
    throw new Error(res.data.message);
  },

  deleteUser: async (id: string): Promise<void> => {
    const res = await api.delete(`/users/${id}`);
    if (!res.data.success) {
      throw new Error(res.data.message);
    }
  },
};

export default userService;
