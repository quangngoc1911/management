import api from '@/shared/lib/axiosInstance'
import type { WeatherForecast } from '../types'

export const weatherService = {
  getAll: () =>
    api.get<WeatherForecast[]>('/WeatherForecast').then(r => r.data),
}