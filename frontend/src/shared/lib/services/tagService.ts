import { createCrudService, unwrap } from './createCrudService';
import { api } from '../axiosInstance';

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

export const tagService = {
    ...createCrudService<Tag, CreateTagRequest, UpdateTagRequest>('/tags'),

    /** Fetch all tags as a flat list (non-paginated). Used by dropdowns. */
    getTags: async (): Promise<Tag[]> => unwrap(await api.get('/tags')),
};

export default tagService;
