import { createCrudService } from './createCrudService';

export const PERIOD_TYPE_OPTIONS = [
    { value: 'weekly', label: 'Hàng tuần' },
    { value: 'monthly', label: 'Hàng tháng' },
    { value: 'quarterly', label: 'Hàng quý' },
    { value: 'yearly', label: 'Hàng năm' },
];

export const periodTypeLabel = (value?: string | null): string =>
    PERIOD_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface Budget {
    id: string;
    categoryId?: string | null;
    categoryName?: string | null;
    memberId?: string | null;
    memberName?: string | null;
    name: string;
    amount: number;
    currency: string;
    periodType: string;
    startDate: string;
    endDate: string;
    alertThreshold?: number | null;
    isActive: boolean;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateBudgetRequest {
    categoryId?: string | null;
    memberId?: string | null;
    name: string;
    amount: number;
    currency: string;
    periodType: string;
    startDate: string;
    endDate: string;
    alertThreshold?: number | null;
    isActive: boolean;
}

export type UpdateBudgetRequest = CreateBudgetRequest;

export interface BudgetQuery {
    search?: string;
    categoryId?: string;
    periodType?: string;
    isActive?: boolean;
    page?: number;
    pageSize?: number;
}

export interface PaginatedBudgets {
    items: Budget[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/budgets';

export const budgetService = createCrudService<Budget, CreateBudgetRequest, UpdateBudgetRequest, BudgetQuery>(BASE);

export default budgetService;
