import { createCrudService } from './createCrudService';

export const INVESTMENT_TYPE_OPTIONS = [
    { value: 'stock', label: 'Cổ phiếu' },
    { value: 'bond', label: 'Trái phiếu' },
    { value: 'fund', label: 'Quỹ' },
    { value: 'crypto', label: 'Tiền mã hoá' },
    { value: 'realestate', label: 'Bất động sản' },
    { value: 'gold', label: 'Vàng' },
    { value: 'other', label: 'Khác' },
];

export const investmentTypeLabel = (value?: string | null): string =>
    INVESTMENT_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface Investment {
    id: string;
    accountId?: string | null;
    accountName?: string | null;
    memberId?: string | null;
    memberName?: string | null;
    type: string;
    name: string;
    symbol?: string | null;
    quantity?: number | null;
    purchasePrice?: number | null;
    currentPrice?: number | null;
    isActive: boolean;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateInvestmentRequest {
    accountId?: string | null;
    memberId?: string | null;
    type: string;
    name: string;
    symbol?: string | null;
    quantity?: number | null;
    purchasePrice?: number | null;
    currentPrice?: number | null;
    isActive: boolean;
}

export type UpdateInvestmentRequest = CreateInvestmentRequest;

export interface InvestmentQuery {
    search?: string;
    accountId?: string;
    type?: string;
    isActive?: boolean;
    page?: number;
    pageSize?: number;
}

export interface PaginatedInvestments {
    items: Investment[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/investments';

export const investmentService = createCrudService<
    Investment,
    CreateInvestmentRequest,
    UpdateInvestmentRequest,
    InvestmentQuery
>(BASE);

export default investmentService;
