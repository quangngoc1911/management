import { z } from 'zod';
import type { CreateDocumentRequest, DocumentStatus } from '@/shared/lib/services/documentService';

export interface DocumentFormValues {
    title: string;
    content: string;
    categoryId: string;
    status?: DocumentStatus;
}

export const emptyDocumentForm: DocumentFormValues = {
    title: '',
    content: '',
    categoryId: '',
    status: 'draft',
};

export const documentSchema = z.object({
    title: z.string().min(2, 'Tiêu đề phải có ít nhất 2 ký tự'),
    content: z.string().min(10, 'Nội dung phải có ít nhất 10 ký tự'),
    categoryId: z.string().min(1, 'Vui lòng chọn danh mục'),
    status: z.enum(['draft', 'published', 'archived']).optional(),
});

export const toDocumentRequest = (data: DocumentFormValues): CreateDocumentRequest => ({
    title: data.title.trim(),
    content: data.content.trim(),
    categoryId: data.categoryId,
});
