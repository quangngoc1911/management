export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
    token: string;
    refreshToken?: string;
    user: UserInfo;
}

export interface UserInfo {
    id: string;
    email: string;
    fullName: string;
    role: string;
    avatar?: string;
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
