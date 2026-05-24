// src/features/auth/services/auth.client.ts

import { LoginRequest, LoginResponse, UserInfo } from '../types/auth';
import { api } from '@/shared/lib/axiosInstance';

export const authApi = {
    login: async (data: LoginRequest): Promise<LoginResponse> => {
        const res = await api.post('/auth/login', data);
        const json = res.data;
        console.log('[authApi.login] response:', json);

        if (!json || !json.success) {
            throw new Error(json?.message || 'Đăng nhập thất bại');
        }

        const payload = json.data;

        // Map backend AuthResponse to frontend LoginResponse
        const token = payload?.accessToken || payload?.AccessToken || payload?.token;
        const refreshToken = payload?.refreshToken || payload?.RefreshToken;
        const userRaw = payload?.user || payload;

        if (!token) {
            console.error('[authApi.login] missing token payload:', payload);
            throw new Error('Không nhận được access token từ server. Vui lòng kiểm tra lại thông tin đăng nhập.');
        }

        console.log('[authApi.login] mapped login result:', { token, refreshToken, user: userRaw });

        const user: UserInfo = {
            id: userRaw?.id || userRaw?.userId || userRaw?.UserId || userRaw?.Id || '',
            email: userRaw?.email || userRaw?.Email || '',
            fullName:
                userRaw?.name || userRaw?.Name || userRaw?.userName || userRaw?.UserName || '',
            role: userRaw?.role || userRaw?.Role || '',
            avatar: userRaw?.avatarUrl || userRaw?.AvatarUrl || userRaw?.avatar || undefined,
        };

        return { token, refreshToken, user } as LoginResponse;
    },
 
    register: async (data: { email: string; password: string; fullName: string }) => {
        const res = await api.post('/auth/register', data);
        const json = res.data;
        if (!json || !json.success) throw new Error(json?.message || 'Đăng ký thất bại');
        return json.data;
    },

    logout: async (): Promise<void> => {
        const refreshToken = localStorage.getItem('refresh_token');
        try {
            // backend expects raw string body for logout in some implementations
            await api.post('/auth/logout', JSON.stringify(refreshToken), {
                headers: { 'Content-Type': 'application/json' },
            });
        } finally {
            localStorage.removeItem('access_token');
            localStorage.removeItem('refresh_token');
            localStorage.removeItem('user');
        }
    },
};

export default authApi;
