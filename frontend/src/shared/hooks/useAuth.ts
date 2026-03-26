// src/shared/hooks/useAuth.ts
"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";  // ← đổi next/router → next/navigation
import Cookies from "js-cookie";
import { authApi } from "../api/auth.client";
import { LoginRequest } from "../types/auth";
import { clearTokens } from "../lib/token";
import { getErrorMessage } from "../utils/error";

export function useAuth() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError]     = useState<string | null>(null);

  const login = async (data: LoginRequest) => {
    setLoading(true);
    setError(null);
    try {
      const res = await authApi.login(data);

      Cookies.set("accessToken", res.accessToken, {
        expires: 1, path: "/", sameSite: "Lax",
      });
      Cookies.set("refreshToken", res.refreshToken, {
        expires: 7, path: "/", sameSite: "Lax",
      });

      localStorage.setItem("user", JSON.stringify(res.user));
      router.push("/menus");
    } catch (err: unknown) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    clearTokens();
    localStorage.removeItem("user");
    router.push("/login");
  };

  return { login, logout, loading, error };  // ← export ra để component dùng
}