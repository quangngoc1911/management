import { createCrudService, unwrap } from './createCrudService';
import { api } from '../axiosInstance';

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

// NOTE: The categories endpoint returns a tree (Category[]) not a paginated result,
// so getAll from createCrudService is intentionally unused for the list view.
// getCategories is kept as the canonical non-paginated fetcher for the tree page and dropdowns.
const _crud = createCrudService<Category, CreateCategoryRequest, UpdateCategoryRequest>('/categories');

export const categoryService = {
    getById: _crud.getById,
    create: _crud.create,
    update: _crud.update,
    remove: _crud.remove,

    /** Fetch all categories as a tree (non-paginated). Used by tree view and dropdowns. */
    getCategories: async (): Promise<Category[]> => unwrap(await api.get('/categories')),
};

export default categoryService;
