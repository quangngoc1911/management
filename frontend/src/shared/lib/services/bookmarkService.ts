import { createCrudService } from './createCrudService';

export interface Bookmark {
    id: string;
    userId: string;
    entityType: string;
    entityId: string;
    note?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateBookmarkRequest {
    entityType: string;
    entityId: string;
    note?: string | null;
}

export interface UpdateBookmarkRequest {
    note?: string | null;
}

export interface BookmarkQuery {
    entityType?: string;
    page?: number;
    pageSize?: number;
}

const BASE = '/bookmarks';

export const bookmarkService = {
    ...createCrudService<Bookmark, CreateBookmarkRequest, UpdateBookmarkRequest, BookmarkQuery>(BASE),
};

export default bookmarkService;
