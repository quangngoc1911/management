// filepath: src/shared/lib/store/authStore.ts
'use client'

import { createContext, useContext, ReactNode, useState, useEffect, useCallback } from 'react'
import { UserInfo } from '@/features/auth/types/auth'

interface AuthContextType {
  user: UserInfo | null
  isAuthenticated: boolean
  loading: boolean
  setAuth: (user: UserInfo, token: string, refreshToken?: string) => void
  logout: () => void
  updateUser: (user: Partial<UserInfo>) => void
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

const TOKEN_KEY = "access_token";
const REFRESH_TOKEN_KEY = "refresh_token";
const USER_KEY = "user";

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserInfo | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  // Check auth status on mount
  useEffect(() => {
    const token = typeof window !== 'undefined' ? localStorage.getItem(TOKEN_KEY) : null;
    const userStr = typeof window !== 'undefined' ? localStorage.getItem(USER_KEY) : null;
    
    if (token && userStr) {
      try {
        setUser(JSON.parse(userStr));
        setIsAuthenticated(true);
      } catch {
        // Invalid user data
      }
    }
    setLoading(false);
  }, []);

  const setAuth = useCallback((userData: UserInfo, token: string, refreshToken?: string) => {
    if (typeof window !== 'undefined') {
      localStorage.setItem(TOKEN_KEY, token);
      if (refreshToken) {
        localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
      }
      localStorage.setItem(USER_KEY, JSON.stringify(userData));
    }
    setUser(userData);
    setIsAuthenticated(true);
  }, []);

  const logout = useCallback(async () => {
    if (typeof window !== 'undefined') {
      localStorage.removeItem(TOKEN_KEY);
      localStorage.removeItem(REFRESH_TOKEN_KEY);
      localStorage.removeItem(USER_KEY);
      window.location.href = '/login';
    }
  }, []);

  const updateUser = useCallback((userData: Partial<UserInfo>) => {
    if (user) {
      const updatedUser = { ...user, ...userData };
      if (typeof window !== 'undefined') {
        localStorage.setItem(USER_KEY, JSON.stringify(updatedUser));
      }
      setUser(updatedUser);
    }
  }, [user]);

  return (
    <AuthContext.Provider value={{ user, isAuthenticated, loading, setAuth, logout, updateUser }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuthContext() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuthContext must be used within an AuthProvider');
  }
  return context;
}

// Alias for useAuth
export function useAuth() {
  return useAuthContext();
}

export function useUser() {
  const { user } = useAuthContext();
  return user;
}

export function useIsAuthenticated() {
  const { isAuthenticated } = useAuthContext();
  return isAuthenticated;
}

export function useLogout() {
  const { logout } = useAuthContext();
  return logout;
}
