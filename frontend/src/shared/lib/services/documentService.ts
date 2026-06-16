import { createCrudService, unwrap } from './createCrudService';
import type { ListParams } from './createCrudService';
import { api } from '../axiosInstance';

export type DocumentStatus = 'draft' | 'published' | 'archived';

export interface Document {
    id: string;
    title: string;
    content: string;
    categoryId: string;
    categoryName?: string;
    tags: Array<{ id: string; name: string; slug: string }>;
    authorId: string;
    authorName?: string;
    status: DocumentStatus;
    viewCount: number;
    createdAt: string;
    updatedAt: string;
    publishedAt?: string;
}

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

export interface DocumentQueryParams extends ListParams {
    categoryId?: string;
    tagId?: string;
    status?: DocumentStatus;
}

/** @deprecated Use documentService.getAll — kept for backwards compat. */
export interface PaginatedDocuments {
    items: Document[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
}

export const documentService = {
    ...createCrudService<Document, CreateDocumentRequest, UpdateDocumentRequest, DocumentQueryParams>(
        '/documents',
    ),

    /**
     * @deprecated Use documentService.getAll — kept for backwards compat with search page.
     * Returns the paginated result directly from the backend envelope.
     */
    getDocuments: async (params: DocumentQueryParams = {}): Promise<PaginatedDocuments> =>
        unwrap(await api.get('/documents', { params })),
};

export default documentService;
