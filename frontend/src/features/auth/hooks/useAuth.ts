"use client";

import { useCallback, useState } from 'react';
import { useRouter } from 'next/navigation';
import Cookies from 'js-cookie';
import { LoginRequest, LoginResponse, UserInfo } from '../types/auth';
import authApi from '../services/auth.client';
import { useAuthContext } from '@/shared/lib/store/authStore';

const TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const USER_KEY = 'user';

export function useAuth() {
    const router = useRouter();
    const ctx = useAuthContext();
    const [loginLoading, setLoginLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const login = useCallback(
        async (data: LoginRequest) => {
            console.log('[useAuth] login start', data);
            setLoginLoading(true);
            setError(null);
            try {
                const res: LoginResponse = await authApi.login(data as any);

                const token = (res as any).token || (res as any).accessToken;
                const refreshToken = (res as any).refreshToken;
                console.log('[useAuth] login success api response', { res, token, refreshToken });

                // Persist via AuthProvider helper
                ctx.setAuth(res.user as UserInfo, token, refreshToken);
                Cookies.set('access_token', token, { path: '/' });
                if (refreshToken) {
                    Cookies.set('refresh_token', refreshToken, { path: '/' });
                }
                console.log('[useAuth] auth state updated', { user: res.user, token, refreshToken });

                sessionStorage.removeItem('redirect');
                return true;
            } catch (err: unknown) {
                console.error('[useAuth] login error', err);
                setError(err instanceof Error ? err.message : 'Đăng nhập thất bại');
                return false;
            } finally {
                setLoginLoading(false);
            }
        },
        [ctx],
    );

    const logout = useCallback(async () => {
        try {
            await authApi.logout();
            ctx.logout();
            router.push('/login');
        } catch (err: unknown) {
            console.error('Logout API failed:', err);
        }
    }, [ctx, router]);

    const updateUser = useCallback(
        (userData: Partial<UserInfo>) => {
            ctx.updateUser(userData);
        },
        [ctx],
    );

    return {
        login,
        logout,
        loading: loginLoading,
        authLoading: ctx.loading,
        error,
        user: ctx.user,
        isAuthenticated: ctx.isAuthenticated,
        updateUser,
    };
}

export function useUser() {
    const { user } = useAuth();
    return user;
}

export function useIsAuthenticated() {
    const { isAuthenticated } = useAuth();
    return isAuthenticated;
}

export function useLogout() {
    const { logout } = useAuth();
    return logout;
}
