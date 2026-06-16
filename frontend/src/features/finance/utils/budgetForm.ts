import { z } from 'zod';
import type { CreateBudgetRequest } from '@/shared/lib/services/budgetService';

export interface BudgetFormValues {
    name: string;
    amount: string;
    currency: string;
    periodType: string;
    startDate: string;
    endDate: string;
    alertThreshold?: string;
    isActive?: boolean;
}

const today = () => new Date().toISOString().slice(0, 10);

export const emptyBudgetForm: BudgetFormValues = {
    name: '',
    amount: '',
    currency: 'VND',
    periodType: 'monthly',
    startDate: today(),
    endDate: today(),
    alertThreshold: '',
    isActive: true,
};

export const budgetSchema = z
    .object({
        name: z.string().min(2, 'Tên ngân sách phải có ít nhất 2 ký tự').max(200),
        amount: z.string().min(1, 'Nhập số tiền').refine((v) => Number(v) > 0, 'Số tiền phải lớn hơn 0'),
        currency: z.string().min(1, 'Nhập tiền tệ'),
        periodType: z.string().min(1, 'Chọn kỳ ngân sách'),
        startDate: z.string().min(1, 'Chọn ngày bắt đầu'),
        endDate: z.string().min(1, 'Chọn ngày kết thúc'),
        alertThreshold: z.string().optional(),
        isActive: z.boolean().optional(),
    })
    .refine((d) => d.endDate >= d.startDate, {
        message: 'Ngày kết thúc phải sau ngày bắt đầu',
        path: ['endDate'],
    });

export const toBudgetRequest = (d: BudgetFormValues): CreateBudgetRequest => ({
    name: d.name.trim(),
    amount: Number(d.amount),
    currency: d.currency.trim() || 'VND',
    periodType: d.periodType,
    startDate: d.startDate,
    endDate: d.endDate,
    alertThreshold: d.alertThreshold && d.alertThreshold.trim() !== '' ? Number(d.alertThreshold) : null,
    isActive: d.isActive ?? true,
});
