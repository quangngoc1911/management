import { z } from 'zod';
import type { CreateTagRequest } from '@/shared/lib/services/tagService';

export interface TagFormValues {
    name: string;
    slug?: string;
    color?: string;
}

export const emptyTagForm: TagFormValues = {
    name: '',
    slug: '',
    color: '',
};

export const tagSchema = z.object({
    name: z.string().min(2, 'Tên tag phải có ít nhất 2 ký tự'),
    slug: z.string().optional(),
    color: z.string().optional(),
});

export const toTagRequest = (data: TagFormValues): CreateTagRequest => ({
    name: data.name.trim(),
    slug: data.slug?.trim() || undefined,
    color: data.color || undefined,
});
