import { z } from 'zod';
import type { CreateAssetRequest, UpdateAssetRequest } from '@/shared/lib/services/assetService';

export interface AssetFormValues {
    name: string;
    assetType: string;
    purchasePrice?: string;
    purchaseDate?: string;
    currency?: string;
    location?: string;
    serialNumber?: string;
    status?: string;
    isInsured?: boolean;
    insuranceInfo?: string;
    description?: string;
}

export const emptyAssetForm: AssetFormValues = {
    name: '',
    assetType: 'electronics',
    purchasePrice: '',
    purchaseDate: '',
    currency: 'VND',
    location: '',
    serialNumber: '',
    status: '1',
    isInsured: false,
    insuranceInfo: '',
    description: '',
};

export const assetSchema = z.object({
    name: z.string().min(2, 'Tên tài sản phải có ít nhất 2 ký tự').max(300),
    assetType: z.string().min(1, 'Chọn loại tài sản'),
    purchasePrice: z.string().optional(),
    purchaseDate: z.string().optional(),
    currency: z.string().optional(),
    location: z.string().optional(),
    serialNumber: z.string().optional(),
    status: z.string().optional(),
    isInsured: z.boolean().optional(),
    insuranceInfo: z.string().optional(),
    description: z.string().optional(),
});

const s = (v?: string) => (v && v.trim() !== '' ? v.trim() : null);

export const toAssetRequest = (d: AssetFormValues): CreateAssetRequest & UpdateAssetRequest => ({
    name: d.name.trim(),
    assetType: d.assetType,
    purchasePrice: d.purchasePrice && d.purchasePrice.trim() !== '' ? Number(d.purchasePrice) : null,
    purchaseDate: d.purchaseDate && d.purchaseDate.trim() !== '' ? d.purchaseDate : null,
    currency: s(d.currency) ?? 'VND',
    location: s(d.location),
    serialNumber: s(d.serialNumber),
    status: d.status ? Number(d.status) : 1,
    isInsured: d.isInsured ?? false,
    insuranceInfo: s(d.insuranceInfo),
    description: s(d.description),
});
