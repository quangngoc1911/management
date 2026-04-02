"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";
import { LoginRequest } from "../types/auth";
import { clearTokens } from "../../../shared/lib/token";
import { getErrorMessage } from "../../../shared/utils/error";
import { authApi } from "../services/auth.client";

export function useAuth() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError]     = useState<string | null>(null);
  console.log("LOGIN SUCCESS → redirect menus");
  
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
      const redirectTo = sessionStorage.getItem("redirect") || "/users";
      router.replace(redirectTo);
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