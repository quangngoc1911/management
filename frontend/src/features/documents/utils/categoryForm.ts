import { z } from 'zod';
import type { CreateCategoryRequest } from '@/shared/lib/services/categoryService';

export interface CategoryFormValues {
    name: string;
    slug?: string;
    description?: string;
    parentId?: string;
}

export const emptyCategoryForm: CategoryFormValues = {
    name: '',
    slug: '',
    description: '',
    parentId: '',
};

export const categorySchema = z.object({
    name: z.string().min(2, 'Tên danh mục phải có ít nhất 2 ký tự'),
    slug: z.string().optional(),
    description: z.string().optional(),
    parentId: z.string().optional(),
});

export const toCategoryRequest = (data: CategoryFormValues): CreateCategoryRequest => ({
    name: data.name.trim(),
    slug: data.slug?.trim() || undefined,
    description: data.description?.trim() || undefined,
    parentId: data.parentId || undefined,
});
