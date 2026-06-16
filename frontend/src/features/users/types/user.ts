import type { ListParams, Paginated } from '@/shared/lib/services/createCrudService';

// Real Users domain model (matches the backend api/users contract).
// NOTE: features/users/types/index.ts holds an unrelated legacy scaffold model — see report.
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

export type UserQuery = ListParams;
export type PaginatedUsers = Paginated<User>;

/** Role identifiers; labels are localized via `users.roles.<value>`. */
export const ROLE_VALUES = ['admin', 'editor', 'user'] as const;
export type RoleValue = (typeof ROLE_VALUES)[number];
