import api from '@/shared/lib/axiosInstance'
import type { User, CreateUserDto } from '../types'

export const userService = {
  getAll: () =>
    api.get<User[]>('/api/backend/User').then(r => r.data),

  create: (dto: CreateUserDto) =>
    api.post<User>('/api/backend/User', dto).then(r => r.data),
}