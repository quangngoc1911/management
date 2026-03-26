// lib/api/menu.client.ts
"use client";
import { getAuthHeaders } from "../lib/token";
import { CreateMenuRequest, Menu } from "../types/menu";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL;

const getHeaders = () => ({
  "Content-Type": "application/json",
  Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
});

export const menuApi = {
  getTree: async (): Promise<Menu[]> => {
    const res = await fetch(`${BASE_URL}/api/menus/tree`, {
      cache: "no-store",
      headers: getAuthHeaders(),
    });
    if (!res.ok) throw new Error("Lấy danh sách menu thất bại");
    return res.json();
  },

  create: async (data: CreateMenuRequest): Promise<Menu> => {
    const res = await fetch(`${BASE_URL}/api/menus`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(data),
    });
    if (!res.ok) {
      const err = await res.json();
      throw new Error(err.message || "Tạo menu thất bại");
    }
    return res.json();
  },
};