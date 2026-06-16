import { z } from 'zod';
import type { CreateInvestmentRequest } from '@/shared/lib/services/investmentService';

export interface InvestmentFormValues {
    type: string;
    name: string;
    symbol?: string;
    quantity?: string;
    purchasePrice?: string;
    currentPrice?: string;
    isActive?: boolean;
}

export const emptyInvestmentForm: InvestmentFormValues = {
    type: 'stock',
    name: '',
    symbol: '',
    quantity: '',
    purchasePrice: '',
    currentPrice: '',
    isActive: true,
};

export const investmentSchema = z.object({
    type: z.string().min(1, 'Chọn loại đầu tư'),
    name: z.string().min(2, 'Tên phải có ít nhất 2 ký tự').max(200),
    symbol: z.string().max(20).optional(),
    quantity: z.string().optional(),
    purchasePrice: z.string().optional(),
    currentPrice: z.string().optional(),
    isActive: z.boolean().optional(),
});

const num = (v?: string) => (v && v.trim() !== '' ? Number(v) : null);

export const toInvestmentRequest = (d: InvestmentFormValues): CreateInvestmentRequest => ({
    type: d.type,
    name: d.name.trim(),
    symbol: d.symbol?.trim() || null,
    quantity: num(d.quantity),
    purchasePrice: num(d.purchasePrice),
    currentPrice: num(d.currentPrice),
    isActive: d.isActive ?? true,
});
