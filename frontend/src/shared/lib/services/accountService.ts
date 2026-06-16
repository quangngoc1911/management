import { createCrudService } from './createCrudService';

export const ACCOUNT_TYPE_OPTIONS = [
    { value: 'cash', label: 'Tiền mặt' },
    { value: 'bank', label: 'Tài khoản ngân hàng' },
    { value: 'ewallet', label: 'Ví điện tử' },
    { value: 'credit', label: 'Thẻ tín dụng' },
    { value: 'savings', label: 'Tiết kiệm' },
    { value: 'investment', label: 'Đầu tư' },
];

export const accountTypeLabel = (value?: string | null): string =>
    ACCOUNT_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface Account {
    id: string;
    memberId?: string | null;
    memberName?: string | null;
    name: string;
    accountType: string;
    accountNumber?: string | null;
    bankName?: string | null;
    currency: string;
    isActive: boolean;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateAccountRequest {
    memberId?: string | null;
    name: string;
    accountType: string;
    accountNumber?: string | null;
    bankName?: string | null;
    currency: string;
    isActive: boolean;
}

export type UpdateAccountRequest = CreateAccountRequest;

export interface AccountQuery {
    search?: string;
    memberId?: string;
    accountType?: string;
    isActive?: boolean;
    sortBy?: string;
    isDescending?: boolean;
    page?: number;
    pageSize?: number;
}

export interface PaginatedAccounts {
    items: Account[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/accounts';

export const accountService = createCrudService<Account, CreateAccountRequest, UpdateAccountRequest, AccountQuery>(BASE);

export default accountService;
