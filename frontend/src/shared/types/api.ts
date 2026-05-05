// filepath: src/shared/types/api.ts
export interface ApiResponse<T> {
    success: boolean;
    message: string;
    data: T;
    errors?: string[];
}

export interface PaginatedResponse<T> {
    items: T[];
    total: number;
    page: number;
    pageSize: number;
    totalPages: number;
}

// Auth Types
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

// User Types
export interface User {
    id: string;
    email: string;
    fullName: string;
    role: string;
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface CreateUserRequest {
    email: string;
    password: string;
    fullName: string;
    role: string;
}

export interface UpdateUserRequest {
    fullName?: string;
    role?: string;
    isActive?: boolean;
}

// Category Types
export interface Category {
    id: string;
    name: string;
    slug: string;
    description?: string;
    parentId?: string;
    children?: Category[];
    documentCount?: number;
    createdAt: string;
    updatedAt: string;
}

export interface CreateCategoryRequest {
    name: string;
    slug?: string;
    description?: string;
    parentId?: string;
}

export interface UpdateCategoryRequest {
    name?: string;
    slug?: string;
    description?: string;
    parentId?: string;
}

// Document Types
export interface Document {
    id: string;
    title: string;
    content: string;
    categoryId: string;
    categoryName?: string;
    tags: Tag[];
    authorId: string;
    authorName?: string;
    status: DocumentStatus;
    viewCount: number;
    createdAt: string;
    updatedAt: string;
    publishedAt?: string;
}

export type DocumentStatus = 'draft' | 'published' | 'archived';

export interface CreateDocumentRequest {
    title: string;
    content: string;
    categoryId: string;
    tagIds?: string[];
}

export interface UpdateDocumentRequest {
    title?: string;
    content?: string;
    categoryId?: string;
    tagIds?: string[];
    status?: DocumentStatus;
}

// Tag Types
export interface Tag {
    id: string;
    name: string;
    slug: string;
    color?: string;
    documentCount?: number;
    createdAt: string;
    updatedAt: string;
}

export interface CreateTagRequest {
    name: string;
    slug?: string;
    color?: string;
}

export interface UpdateTagRequest {
    name?: string;
    slug?: string;
    color?: string;
}

// Menu Types
export interface Menu {
    id: string;
    name: string;
    url: string;
    icon?: string;
    parentId?: string;
    order: number;
    isActive: boolean;
    children?: Menu[];
}

// Dashboard Stats
export interface DashboardStats {
    totalUsers: number;
    totalDocuments: number;
    totalCategories: number;
    totalTags: number;
    recentDocuments: Document[];
    documentsByStatus: Record<DocumentStatus, number>;
}
