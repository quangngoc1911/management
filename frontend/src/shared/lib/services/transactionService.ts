import { createCrudService } from './createCrudService';

export const TRANSACTION_TYPE_OPTIONS = [
    { value: 'income', label: 'Thu' },
    { value: 'expense', label: 'Chi' },
    { value: 'transfer', label: 'Chuyển khoản' },
];

export const TRANSACTION_STATUS_OPTIONS = [
    { value: 1, label: 'Chờ xử lý' },
    { value: 2, label: 'Hoàn tất' },
    { value: 3, label: 'Thất bại' },
    { value: 4, label: 'Đã huỷ' },
];

export const transactionTypeLabel = (value?: string | null): string =>
    TRANSACTION_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export const transactionStatusLabel = (value?: number | null): string =>
    TRANSACTION_STATUS_OPTIONS.find((o) => o.value === value)?.label ?? '—';

export interface Transaction {
    id: string;
    accountId: string;
    accountName: string;
    categoryId?: string | null;
    categoryName?: string | null;
    memberId?: string | null;
    memberName?: string | null;
    type: string;
    amount: number;
    currency: string;
    exchangeRate?: number | null;
    description?: string | null;
    note?: string | null;
    transactionDate: string;
    status: number;
    transferToAccountId?: string | null;
    transferToAccountName?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateTransactionRequest {
    accountId: string;
    categoryId?: string | null;
    memberId?: string | null;
    type: string;
    amount: number;
    currency: string;
    exchangeRate?: number | null;
    description?: string | null;
    note?: string | null;
    transactionDate: string;
    status: number;
    transferToAccountId?: string | null;
}

export type UpdateTransactionRequest = CreateTransactionRequest;

export interface TransactionQuery {
    search?: string;
    accountId?: string;
    categoryId?: string;
    type?: string;
    status?: number;
    fromDate?: string;
    toDate?: string;
    sortBy?: string;
    isDescending?: boolean;
    page?: number;
    pageSize?: number;
}

export interface PaginatedTransactions {
    items: Transaction[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/transactions';

export const transactionService = createCrudService<
    Transaction,
    CreateTransactionRequest,
    UpdateTransactionRequest,
    TransactionQuery
>(BASE);

export default transactionService;
