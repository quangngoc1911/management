import { cookies } from "next/headers";  // ← Next.js server cookie
import { Menu } from "../types/menu";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL; 

export const menuServerApi = {
  getTree: async (): Promise<Menu[]> => {
    const token = (await cookies()).get("accessToken")?.value ?? "";

    const res = await fetch(`${BASE_URL}/api/menus/tree`, {
      cache: "no-store",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,  // ← đọc từ server cookie
      },
    });
    if (!res.ok) return [];
    return res.json();
  },
};