// filepath: src/shared/lib/services/authService.ts
import { api } from '../axiosInstance'
import { ApiResponse, LoginRequest, LoginResponse, UserInfo } from '@/shared/types/api'

export const authService = {
  login: async (data: LoginRequest): Promise<ApiResponse<LoginResponse>> => {
    const response = await api.post<ApiResponse<LoginResponse>>('/auth/login', data)
    return response.data
  },

  register: async (data: { email: string; password: string; fullName: string }): Promise<ApiResponse<LoginResponse>> => {
    const response = await api.post<ApiResponse<LoginResponse>>('/auth/register', data)
    return response.data
  },

  logout: async (): Promise<void> => {
    await api.post('/auth/logout')
  },

  getCurrentUser: async (): Promise<ApiResponse<UserInfo>> => {
    const response = await api.get<ApiResponse<UserInfo>>('/auth/me')
    return response.data
  },

  refreshToken: async (refreshToken: string): Promise<ApiResponse<LoginResponse>> => {
    const response = await api.post<ApiResponse<LoginResponse>>('/auth/refresh-token', { refreshToken })
    return response.data
  },

  changePassword: async (data: { currentPassword: string; newPassword: string }): Promise<ApiResponse<void>> => {
    const response = await api.post<ApiResponse<void>>('/auth/change-password', data)
    return response.data
  },
}

export default authService