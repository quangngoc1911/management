"use client";
import { useState, useCallback, useEffect } from 'react';
import { useRouter } from "next/navigation";
import { LoginRequest, LoginResponse, UserInfo } from '../types/auth';
import { authApi } from "../services/auth.client";
import Cookies from 'js-cookie';

const TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const USER_KEY = 'user';

export function useAuth() {
    const router = useRouter();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserInfo | null>(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    // Check auth status on mount
    useEffect(() => {
        const token = localStorage.getItem(TOKEN_KEY);
        const userStr = localStorage.getItem(USER_KEY);

        if (token && userStr) {
            try {
                setUser(JSON.parse(userStr));
                setIsAuthenticated(true);
            } catch {
                // Invalid user data
            }
        }
    }, []);

    const login = useCallback(
        async (data: LoginRequest) => {
            setLoading(true);
            setError(null);
            try {
                const res: LoginResponse = await authApi.login(data);

                Cookies.set(TOKEN_KEY, res.token, { expires: 7 });
                if (res.refreshToken) {
                    Cookies.set(REFRESH_TOKEN_KEY, res.refreshToken, { expires: 30 });
                }
                localStorage.setItem(USER_KEY, JSON.stringify(res.user));

                setUser(res.user);
                setIsAuthenticated(true);

                const redirectTo = sessionStorage.getItem('redirect') || '/dashboard';
                sessionStorage.removeItem('redirect');
                router.replace(redirectTo);
            } catch (err: unknown) {
                setError(err instanceof Error ? err.message : 'Đăng nhập thất bại');
            } finally {
                setLoading(false);
            }
        },
        [router],
    );

    const logout = useCallback(async () => {
        try {
            await authApi.logout();
            Cookies.remove(TOKEN_KEY);
            Cookies.remove(REFRESH_TOKEN_KEY);
            localStorage.removeItem(USER_KEY);
            setUser(null);
            setIsAuthenticated(false);
            router.push('/login');
        } catch (err: unknown) {
            // Log or handle the error, but don't clear tokens or redirect on API failure
            console.error('Logout API failed:', err);
        } finally {
            setLoading(false);
        }
    }, [router]);

    const updateUser = useCallback(
        (userData: Partial<UserInfo>) => {
            if (user) {
                const updated = { ...user, ...userData };
                localStorage.setItem(USER_KEY, JSON.stringify(updated));
                setUser(updated);
            }
        },
        [user],
    );

    return {
        login,
        logout,
        loading,
        error,
        user,
        isAuthenticated,
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
