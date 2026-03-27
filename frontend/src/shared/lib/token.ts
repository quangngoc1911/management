import Cookies from "js-cookie";

// Lấy access token
export const getAccessToken = (): string => 
  Cookies.get("accessToken") ?? "";

// Tạo header Authorization cho mọi API call
export const getAuthHeaders = (): HeadersInit => ({
  "Content-Type": "application/json",
  "Authorization": `Bearer ${getAccessToken()}`,
});

// Xóa cookie khi logout
export const clearTokens = () => {
  Cookies.remove("accessToken", { path: "/" });
  Cookies.remove("refreshToken", { path: "/" });
};