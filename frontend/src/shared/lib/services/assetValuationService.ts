import { createCrudService } from './createCrudService';

export interface AssetValuation {
    id: string;
    assetId: string;
    assetName: string;
    valuationDate: string;
    value: number;
    currency: string;
    valuationMethod?: string | null;
    notes?: string | null;
    createdByUserId?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateAssetValuationRequest {
    assetId: string;
    valuationDate: string;
    value: number;
    currency: string;
    valuationMethod?: string | null;
    notes?: string | null;
}

export type UpdateAssetValuationRequest = Omit<CreateAssetValuationRequest, 'assetId'>;

export interface AssetValuationQuery {
    assetId?: string;
    page?: number;
    pageSize?: number;
}

const BASE = '/asset-valuations';

export const assetValuationService = createCrudService<
    AssetValuation,
    CreateAssetValuationRequest,
    UpdateAssetValuationRequest,
    AssetValuationQuery
>(BASE);

export default assetValuationService;
