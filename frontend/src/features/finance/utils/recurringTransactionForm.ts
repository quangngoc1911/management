import { z } from 'zod';
import type { CreateRecurringTransactionRequest } from '@/shared/lib/services/recurringTransactionService';

export interface RecurringTransactionFormValues {
    accountId: string;
    name: string;
    type: string;
    amount: string;
    frequency: string;
    startDate: string;
    endDate?: string;
    nextDueDate?: string;
    isActive?: boolean;
}

const today = () => new Date().toISOString().slice(0, 10);

export const emptyRecurringTransactionForm: RecurringTransactionFormValues = {
    accountId: '',
    name: '',
    type: 'expense',
    amount: '',
    frequency: 'monthly',
    startDate: today(),
    endDate: '',
    nextDueDate: '',
    isActive: true,
};

export const recurringTransactionSchema = z.object({
    accountId: z.string().min(1, 'Chọn tài khoản'),
    name: z.string().min(2, 'Tên phải có ít nhất 2 ký tự').max(200),
    type: z.string().min(1, 'Chọn loại'),
    amount: z.string().min(1, 'Nhập số tiền').refine((v) => Number(v) > 0, 'Số tiền phải lớn hơn 0'),
    frequency: z.string().min(1, 'Chọn tần suất'),
    startDate: z.string().min(1, 'Chọn ngày bắt đầu'),
    endDate: z.string().optional(),
    nextDueDate: z.string().optional(),
    isActive: z.boolean().optional(),
});

export const toRecurringTransactionRequest = (
    d: RecurringTransactionFormValues,
): CreateRecurringTransactionRequest => ({
    accountId: d.accountId,
    name: d.name.trim(),
    type: d.type,
    amount: Number(d.amount),
    frequency: d.frequency,
    startDate: d.startDate,
    endDate: d.endDate && d.endDate.trim() !== '' ? d.endDate : null,
    nextDueDate: d.nextDueDate && d.nextDueDate.trim() !== '' ? d.nextDueDate : null,
    isActive: d.isActive ?? true,
});
