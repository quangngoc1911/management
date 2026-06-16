import { z } from 'zod';
import type { Translate } from '@/shared/i18n/useTranslation';
import type { CreateFamilyMemberRequest } from '../types/family';

export interface MemberFormValues {
    fullName: string;
    nickname?: string;
    gender?: string;
    relationToHead?: string;
    dateOfBirth?: string;
    phone?: string;
    email?: string;
    isHouseholdHead?: boolean;
    notes?: string;
}

export const emptyMemberForm: MemberFormValues = {
    fullName: '',
    nickname: '',
    gender: '',
    relationToHead: '',
    dateOfBirth: '',
    phone: '',
    email: '',
    isHouseholdHead: false,
    notes: '',
};

export const buildMemberSchema = (t: Translate) =>
    z.object({
        fullName: z
            .string()
            .min(2, t('validation.fullNameMin'))
            .max(200, t('validation.fullNameMax')),
        nickname: z.string().max(100, t('validation.nicknameMax')).optional(),
        gender: z.string().optional(),
        relationToHead: z.string().optional(),
        dateOfBirth: z.string().optional(),
        phone: z.string().max(20, t('validation.phoneMax')).optional(),
        email: z.union([z.string().email(t('validation.email')), z.literal('')]).optional(),
        isHouseholdHead: z.boolean().optional(),
        notes: z.string().optional(),
    });

/** Maps form values to the API payload. A non-empty select value (incl. "0") is truthy; "" -> null. */
export const toMemberRequest = (data: MemberFormValues): CreateFamilyMemberRequest => ({
    fullName: data.fullName.trim(),
    nickname: data.nickname?.trim() || null,
    gender: data.gender ? Number(data.gender) : null,
    relationToHead: data.relationToHead ? Number(data.relationToHead) : null,
    dateOfBirth: data.dateOfBirth || null,
    phone: data.phone?.trim() || null,
    email: data.email?.trim() || null,
    isHouseholdHead: data.isHouseholdHead ?? false,
    notes: data.notes?.trim() || null,
});
