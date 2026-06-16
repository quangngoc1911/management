import { createCrudService, type ListParams } from './createCrudService';

export interface SystemConfig {
    id: string;
    key: string;
    value: string;
    description?: string | null;
    isEncrypted: boolean;
    isPublic: boolean;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateSystemConfigRequest {
    key: string;
    value: string;
    description?: string | null;
    isEncrypted: boolean;
    isPublic: boolean;
}

export type UpdateSystemConfigRequest = Omit<CreateSystemConfigRequest, 'key'>;

export interface SystemConfigQuery extends ListParams {
    search?: string;
    isPublic?: boolean;
    page?: number;
    pageSize?: number;
}

export interface PaginatedSystemConfigs {
    items: SystemConfig[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/system-configs';

export const systemConfigService = createCrudService<
    SystemConfig,
    CreateSystemConfigRequest,
    UpdateSystemConfigRequest,
    SystemConfigQuery
>(BASE);

export default systemConfigService;
