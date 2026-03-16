'use client'
import { useQuery } from '@tanstack/react-query'
import { weatherService } from '../services/weatherService'

export function useWeather() {
  return useQuery({
    queryKey: ['weather'],
    queryFn: weatherService.getAll,
  })
}