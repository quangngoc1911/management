import { z } from 'zod';
import type { CreateSystemConfigRequest, UpdateSystemConfigRequest } from '@/shared/lib/services/systemConfigService';

export interface SystemConfigFormValues {
    key: string;
    value: string;
    description?: string;
    isEncrypted?: boolean;
    isPublic?: boolean;
}

export const emptySystemConfigForm: SystemConfigFormValues = {
    key: '',
    value: '{}',
    description: '',
    isEncrypted: false,
    isPublic: false,
};

export const systemConfigSchema = z.object({
    key: z.string().min(1, 'Khoá là bắt buộc').max(200, 'Khoá tối đa 200 ký tự'),
    value: z.string().min(1, 'Giá trị là bắt buộc'),
    description: z.string().optional(),
    isEncrypted: z.boolean().optional().default(false),
    isPublic: z.boolean().optional().default(false),
});

export const toSystemConfigRequest = (
    data: SystemConfigFormValues,
    isEdit: boolean,
): CreateSystemConfigRequest | UpdateSystemConfigRequest => {
    const base: UpdateSystemConfigRequest = {
        value: data.value,
        description: data.description?.trim() || null,
        isEncrypted: data.isEncrypted ?? false,
        isPublic: data.isPublic ?? false,
    };
    if (isEdit) return base;
    return { ...base, key: data.key.trim() } as CreateSystemConfigRequest;
};
