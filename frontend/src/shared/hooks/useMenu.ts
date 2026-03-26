import { useState, useEffect } from "react";
import { CreateMenuRequest, Menu } from "../types/menu";
import { getErrorMessage } from "../utils/error";
import { menuApi } from "../api/menu.client";


export function useMenu() {
  const [menus, setMenus]       = useState<Menu[]>([]);
  const [loading, setLoading]   = useState(false);
  const [error, setError]       = useState<string | null>(null);

  // Lấy danh sách
  const fetchMenus = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await menuApi.getTree();
      setMenus(data);
    } catch (err: unknown) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  // Tạo menu mới
  const createMenu = async (data: CreateMenuRequest) => {
    setLoading(true);
    setError(null);
    try {
      await menuApi.create(data);
      await fetchMenus();  // Reload danh sách sau khi tạo
      return true;
    } catch (err: unknown) {
      setError(getErrorMessage(err));
      return false;
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchMenus(); }, []);

  return { menus, loading, error, createMenu, fetchMenus };
}