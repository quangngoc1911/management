// src/shared/types/auth.ts
export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiry: string;
  user: {
    id: number;
    name: string;
    email: string;
    role: string;
  };
}