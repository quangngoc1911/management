import { createCrudService } from './createCrudService';

export const ASSET_STATUS_OPTIONS = [
    { value: 1, label: 'Đang sở hữu' },
    { value: 2, label: 'Đã bán' },
    { value: 3, label: 'Đã thanh lý' },
    { value: 4, label: 'Bị mất' },
    { value: 5, label: 'Đang thế chấp' },
];

export const ASSET_TYPE_OPTIONS = [
    { value: 'realestate', label: 'Bất động sản' },
    { value: 'vehicle', label: 'Phương tiện' },
    { value: 'electronics', label: 'Điện tử' },
    { value: 'jewelry', label: 'Trang sức' },
    { value: 'furniture', label: 'Nội thất' },
    { value: 'other', label: 'Khác' },
];

export const assetStatusLabel = (value?: number | null): string =>
    ASSET_STATUS_OPTIONS.find((o) => o.value === value)?.label ?? '—';

export const assetTypeLabel = (value?: string | null): string =>
    ASSET_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface Asset {
    id: string;
    memberId?: string | null;
    memberName?: string | null;
    categoryId?: string | null;
    categoryName?: string | null;
    name: string;
    description?: string | null;
    assetType: string;
    purchasePrice?: number | null;
    purchaseDate?: string | null;
    currency?: string | null;
    location?: string | null;
    serialNumber?: string | null;
    status: number;
    isInsured?: boolean | null;
    insuranceInfo?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateAssetRequest {
    memberId?: string | null;
    categoryId?: string | null;
    name: string;
    description?: string | null;
    assetType: string;
    purchasePrice?: number | null;
    purchaseDate?: string | null;
    currency?: string | null;
    location?: string | null;
    serialNumber?: string | null;
    status: number;
    isInsured?: boolean | null;
    insuranceInfo?: string | null;
}

export type UpdateAssetRequest = CreateAssetRequest;

export interface AssetQuery {
    search?: string;
    assetType?: string;
    status?: number;
    page?: number;
    pageSize?: number;
}

const BASE = '/assets';

export const assetService = createCrudService<
    Asset,
    CreateAssetRequest,
    UpdateAssetRequest,
    AssetQuery
>(BASE);

export default assetService;
