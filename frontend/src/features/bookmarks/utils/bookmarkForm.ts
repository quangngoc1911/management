import { z } from 'zod';
import type { CreateBookmarkRequest } from '@/shared/lib/services/bookmarkService';

export const ENTITY_TYPE_OPTIONS = [
    { value: 'document', label: 'Tài liệu' },
    { value: 'family-member', label: 'Thành viên' },
    { value: 'asset', label: 'Tài sản' },
    { value: 'event', label: 'Sự kiện' },
    { value: 'other', label: 'Khác' },
];

export interface BookmarkFormValues {
    entityType: string;
    entityId: string;
    note?: string;
}

export const emptyBookmarkForm: BookmarkFormValues = {
    entityType: 'document',
    entityId: '',
    note: '',
};

export const bookmarkSchema = z.object({
    entityType: z.string().min(1, 'Chọn loại đối tượng'),
    entityId: z.string().uuid('ID đối tượng phải là GUID hợp lệ'),
    note: z.string().optional(),
});

export const toBookmarkRequest = (d: BookmarkFormValues): CreateBookmarkRequest => ({
    entityType: d.entityType,
    entityId: d.entityId,
    note: d.note?.trim() || null,
});
