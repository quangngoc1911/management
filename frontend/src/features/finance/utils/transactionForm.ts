import { z } from 'zod';
import type { CreateTransactionRequest } from '@/shared/lib/services/transactionService';

export interface TransactionFormValues {
    accountId: string;
    type: string;
    amount: string;
    currency: string;
    transactionDate: string;
    categoryId?: string;
    status?: string;
    description?: string;
    note?: string;
}

const today = () => new Date().toISOString().slice(0, 10);

export const emptyTransactionForm: TransactionFormValues = {
    accountId: '',
    type: 'expense',
    amount: '',
    currency: 'VND',
    transactionDate: today(),
    categoryId: '',
    status: '2',
    description: '',
    note: '',
};

export const transactionSchema = z.object({
    accountId: z.string().min(1, 'Vui lòng chọn tài khoản'),
    type: z.string().min(1, 'Vui lòng chọn loại giao dịch'),
    amount: z.string().min(1, 'Vui lòng nhập số tiền').refine((v) => Number(v) > 0, 'Số tiền phải lớn hơn 0'),
    currency: z.string().min(1, 'Nhập đơn vị tiền tệ'),
    transactionDate: z.string().min(1, 'Vui lòng chọn ngày'),
    categoryId: z.string().optional(),
    status: z.string().optional(),
    description: z.string().optional(),
    note: z.string().optional(),
});

export const toTransactionRequest = (data: TransactionFormValues): CreateTransactionRequest => ({
    accountId: data.accountId,
    categoryId: data.categoryId || null,
    type: data.type,
    amount: Number(data.amount),
    currency: data.currency.trim() || 'VND',
    transactionDate: data.transactionDate,
    status: data.status ? Number(data.status) : 2,
    description: data.description?.trim() || null,
    note: data.note?.trim() || null,
});
