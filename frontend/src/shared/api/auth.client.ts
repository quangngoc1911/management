// src/shared/api/auth.client.ts
import { AuthResponse, LoginRequest } from "../types/auth";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL;

export const authApi = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const res = await fetch(`${BASE_URL}/api/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    if (!res.ok) {
      const err = await res.json();
      throw new Error(err.message || "Đăng nhập thất bại");
    }

    return res.json();
  },

  logout: () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("user");
  },
};