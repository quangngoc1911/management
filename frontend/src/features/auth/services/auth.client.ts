// src/features/auth/services/auth.client.ts

import { LoginRequest, LoginResponse } from '../types/auth';
import { api } from '@/shared/lib/axiosInstance';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';

export const authApi = {
    login: async (data: LoginRequest): Promise<LoginResponse> => {
        const res = await fetch(`${API_BASE_URL}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data),
        });

        const json = await res.json();

        if (!res.ok || !json.success) {
            throw new Error(json.message || 'Đăng nhập thất bại');
        }

        return json.data;
    },

    register: async (data: { email: string; password: string; fullName: string }) => {
        const res = await fetch(`${API_BASE_URL}/api/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data),
        });

        const json = await res.json();

        if (!res.ok || !json.success) {
            throw new Error(json.message || 'Đăng ký thất bại');
        }

        return json.data;
    },

    logout: async (): Promise<void> => {
        const refreshToken = localStorage.getItem('refresh_token');
        try {
            await api.post('/api/auth/logout', JSON.stringify(refreshToken), {
                headers: {
                    'Content-Type': 'application/json',
                },
            });
        } finally {
            localStorage.removeItem('access_token');
            localStorage.removeItem('refresh_token');
            localStorage.removeItem('user');
        }
    },
};
