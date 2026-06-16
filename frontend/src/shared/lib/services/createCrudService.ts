import type { AxiosResponse } from 'axios';
import { api } from '../axiosInstance';

/** Mirrors backend PaginatedResultDto<T> (note: TotalCount serialized as totalCount). */
export interface Paginated<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage?: boolean;
    hasNextPage?: boolean;
}

/** Common list query shape understood by every paginated endpoint. */
export interface ListParams {
    page?: number;
    pageSize?: number;
    search?: string;
    sortBy?: string;
    isDescending?: boolean;
}

/** Backend envelope: { success, message, data }. Unwraps `data` or throws `message`. */
export function unwrap<T>(res: AxiosResponse<{ success: boolean; message: string; data: T }>): T {
    if (res.data?.success) return res.data.data;
    throw new Error(res.data?.message || 'Request failed');
}

/**
 * Builds a typed CRUD service for a REST resource, removing the repeated
 * `if (res.data.success) ... throw` boilerplate duplicated across every service file.
 *
 * @example
 *   export const userService = createCrudService<User, CreateUserRequest, UpdateUserRequest>('/users');
 */
export function createCrudService<
    TEntity,
    TCreate,
    TUpdate = Partial<TCreate>,
    TQuery extends ListParams = ListParams,
>(basePath: string) {
    return {
        getAll: async (query: TQuery = {} as TQuery): Promise<Paginated<TEntity>> =>
            unwrap(await api.get(basePath, { params: query })),

        getById: async (id: string): Promise<TEntity> => unwrap(await api.get(`${basePath}/${id}`)),

        create: async (data: TCreate): Promise<TEntity> => unwrap(await api.post(basePath, data)),

        update: async (id: string, data: TUpdate): Promise<TEntity> =>
            unwrap(await api.put(`${basePath}/${id}`, data)),

        remove: async (id: string): Promise<void> => {
            const res = await api.delete(`${basePath}/${id}`);
            if (!res.data?.success) throw new Error(res.data?.message || 'Request failed');
        },
    };
}

export type CrudService<TEntity, TCreate, TUpdate = Partial<TCreate>> = ReturnType<
    typeof createCrudService<TEntity, TCreate, TUpdate>
>;
