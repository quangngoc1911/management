import { z } from 'zod';
import type {
    CreateAssetValuationRequest,
    UpdateAssetValuationRequest,
} from '@/shared/lib/services/assetValuationService';

export interface AssetValuationFormValues {
    assetId: string;
    valuationDate: string;
    value: string;
    currency: string;
    valuationMethod?: string;
    notes?: string;
}

const today = () => new Date().toISOString().slice(0, 10);

export const emptyAssetValuationForm: AssetValuationFormValues = {
    assetId: '',
    valuationDate: today(),
    value: '',
    currency: 'VND',
    valuationMethod: '',
    notes: '',
};

export const assetValuationSchema = z.object({
    assetId: z.string().min(1, 'Chọn tài sản'),
    valuationDate: z.string().min(1, 'Chọn ngày định giá'),
    value: z.string().min(1, 'Nhập giá trị').refine((v) => Number(v) >= 0, 'Giá trị không hợp lệ'),
    currency: z.string().min(1, 'Nhập tiền tệ'),
    valuationMethod: z.string().optional(),
    notes: z.string().optional(),
});

export const toAssetValuationCreateRequest = (d: AssetValuationFormValues): CreateAssetValuationRequest => ({
    assetId: d.assetId,
    valuationDate: d.valuationDate,
    value: Number(d.value),
    currency: d.currency.trim() || 'VND',
    valuationMethod: d.valuationMethod?.trim() || null,
    notes: d.notes?.trim() || null,
});

export const toAssetValuationUpdateRequest = (d: AssetValuationFormValues): UpdateAssetValuationRequest => ({
    valuationDate: d.valuationDate,
    value: Number(d.value),
    currency: d.currency.trim() || 'VND',
    valuationMethod: d.valuationMethod?.trim() || null,
    notes: d.notes?.trim() || null,
});
