import { z } from 'zod';
import type { Translate } from '@/shared/i18n/useTranslation';
import type { CreateUserRequest, UpdateUserRequest } from '../types/user';

export interface UserFormValues {
    fullName: string;
    email: string;
    role: string;
    password?: string;
}

export const emptyUserForm: UserFormValues = {
    fullName: '',
    email: '',
    role: '',
    password: '',
};

/**
 * Builds the localized zod schema. Password is only required on create
 * (enforced via superRefine so the inferred type stays stable for both modes).
 */
export const buildUserSchema = (t: Translate, isEdit: boolean) =>
    z
        .object({
            fullName: z.string().min(2, t('validation.fullNameMin')),
            email: z.string().email(t('validation.email')),
            role: z.string().min(1, t('validation.roleRequired')),
            password: z.string().optional(),
        })
        .superRefine((values, ctx) => {
            if (!isEdit && (!values.password || values.password.length < 6)) {
                ctx.addIssue({
                    code: z.ZodIssueCode.custom,
                    path: ['password'],
                    message: t('validation.passwordMin'),
                });
            }
        });

export const toCreateUser = (v: UserFormValues): CreateUserRequest => ({
    email: v.email.trim(),
    fullName: v.fullName.trim(),
    role: v.role,
    password: v.password ?? '',
});

export const toUpdateUser = (v: UserFormValues): UpdateUserRequest => ({
    fullName: v.fullName.trim(),
    role: v.role,
});
