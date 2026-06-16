import { z } from 'zod';
import type { CreateAccountRequest } from '@/shared/lib/services/accountService';

export interface AccountFormValues {
    name: string;
    accountType: string;
    bankName?: string;
    accountNumber?: string;
    currency: string;
    isActive?: boolean;
}

export const emptyAccountForm: AccountFormValues = {
    name: '',
    accountType: '',
    bankName: '',
    accountNumber: '',
    currency: 'VND',
    isActive: true,
};

export const accountSchema = z.object({
    name: z.string().min(2, 'Tên tài khoản phải có ít nhất 2 ký tự').max(200),
    accountType: z.string().min(1, 'Vui lòng chọn loại tài khoản'),
    bankName: z.string().max(200).optional(),
    accountNumber: z.string().max(100).optional(),
    currency: z.string().min(1, 'Vui lòng nhập đơn vị tiền tệ').max(10),
    isActive: z.boolean().optional(),
});

export const toAccountRequest = (data: AccountFormValues): CreateAccountRequest => ({
    name: data.name.trim(),
    accountType: data.accountType,
    bankName: data.bankName?.trim() || null,
    accountNumber: data.accountNumber?.trim() || null,
    currency: data.currency.trim() || 'VND',
    isActive: data.isActive ?? true,
});
