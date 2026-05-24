import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios';
import Cookies from 'js-cookie';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5218/api';

const TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const USER_KEY = 'user';

export const api = axios.create({
    baseURL: API_BASE_URL,
    headers: { 'Content-Type': 'application/json' },
    timeout: 30_000,
});

const isBrowser = globalThis.window !== undefined;

// Request interceptor - add token
api.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        if (isBrowser) {
            const token = localStorage.getItem(TOKEN_KEY) || Cookies.get(TOKEN_KEY);
            if (token && config.headers) {
                config.headers.Authorization = `Bearer ${token}`;
            }
        }
        return config;
    },
    (error) => Promise.reject(error),
);

async function refreshAccessToken(
    originalRequest: InternalAxiosRequestConfig & { _retry?: boolean },
) {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY) || Cookies.get(REFRESH_TOKEN_KEY);
    if (!refreshToken) {
        return null;
    }

    // Backend expects raw string body and endpoint '/auth/refresh'
    const response = await axios.post(
        `${API_BASE_URL}/auth/refresh`,
        JSON.stringify(refreshToken),
        {
            headers: { 'Content-Type': 'application/json' },
        },
    );

    const responseData = response.data;
    // Support different casing: AccessToken / accessToken / token
    const newToken =
        responseData?.data?.accessToken ||
        responseData?.data?.AccessToken ||
        responseData?.data?.token;
    const newRefreshToken = responseData?.data?.refreshToken || responseData?.data?.RefreshToken;

    if (!newToken) {
        return null;
    }

    localStorage.setItem(TOKEN_KEY, newToken);
    if (newRefreshToken) {
        localStorage.setItem(REFRESH_TOKEN_KEY, newRefreshToken);
    }

    if (originalRequest.headers) {
        originalRequest.headers.Authorization = `Bearer ${newToken}`;
    }

    return api(originalRequest);
}

function clearAuthData() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    Cookies.remove(TOKEN_KEY);
    Cookies.remove(REFRESH_TOKEN_KEY);

    if (isBrowser) {
        globalThis.window.location.href = '/login';
    }
}

// Response interceptor - handle errors and refresh token
api.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };
        if (!originalRequest) {
            throw error;
        }

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            try {
                const refreshed = await refreshAccessToken(originalRequest);
                if (refreshed) {
                    return refreshed;
                }
            } catch {
                clearAuthData();
            }
        }

        throw error;
    },
);

export default api;
