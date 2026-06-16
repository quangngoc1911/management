import { createCrudService } from './createCrudService';

export const FREQUENCY_OPTIONS = [
    { value: 'daily', label: 'Hàng ngày' },
    { value: 'weekly', label: 'Hàng tuần' },
    { value: 'monthly', label: 'Hàng tháng' },
    { value: 'quarterly', label: 'Hàng quý' },
    { value: 'yearly', label: 'Hàng năm' },
];

export const RECURRING_TYPE_OPTIONS = [
    { value: 'income', label: 'Thu' },
    { value: 'expense', label: 'Chi' },
];

export const frequencyLabel = (value?: string | null): string =>
    FREQUENCY_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export const recurringTypeLabel = (value?: string | null): string =>
    RECURRING_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface RecurringTransaction {
    id: string;
    accountId: string;
    accountName: string;
    categoryId?: string | null;
    categoryName?: string | null;
    name: string;
    type: string;
    amount: number;
    frequency: string;
    startDate: string;
    endDate?: string | null;
    nextDueDate?: string | null;
    isActive: boolean;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateRecurringTransactionRequest {
    accountId: string;
    categoryId?: string | null;
    name: string;
    type: string;
    amount: number;
    frequency: string;
    startDate: string;
    endDate?: string | null;
    nextDueDate?: string | null;
    isActive: boolean;
}

export type UpdateRecurringTransactionRequest = CreateRecurringTransactionRequest;

export interface RecurringTransactionQuery {
    search?: string;
    accountId?: string;
    type?: string;
    frequency?: string;
    isActive?: boolean;
    page?: number;
    pageSize?: number;
}

export interface PaginatedRecurringTransactions {
    items: RecurringTransaction[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/recurring-transactions';

export const recurringTransactionService = createCrudService<
    RecurringTransaction,
    CreateRecurringTransactionRequest,
    UpdateRecurringTransactionRequest,
    RecurringTransactionQuery
>(BASE);

export default recurringTransactionService;
