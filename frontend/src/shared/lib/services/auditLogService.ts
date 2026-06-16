import { api } from '../axiosInstance';
import { unwrap } from './createCrudService';

export const AUDIT_ACTION_OPTIONS = [
    { value: 1, label: 'Tạo' },
    { value: 2, label: 'Cập nhật' },
    { value: 3, label: 'Xóa' },
    { value: 4, label: 'Đăng nhập' },
    { value: 5, label: 'Đăng xuất' },
    { value: 6, label: 'Xuất dữ liệu' },
    { value: 7, label: 'Nhập dữ liệu' },
    { value: 99, label: 'Khác' },
];

export const auditActionLabel = (value?: number | null): string =>
    AUDIT_ACTION_OPTIONS.find((o) => o.value === value)?.label ?? '—';

export interface AuditLog {
    id: string;
    userId?: string | null;
    userName?: string | null;
    action: number;
    entityType: string;
    entityId?: string | null;
    oldValues?: string | null;
    newValues?: string | null;
    ipAddress?: string | null;
    createdAt?: string | null;
}

export interface AuditLogQuery {
    action?: number;
    entityType?: string;
    page?: number;
    pageSize?: number;
}

export interface PaginatedAuditLogs {
    items: AuditLog[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/audit-logs';

export const auditLogService = {
    getAll: async (query: AuditLogQuery = {}): Promise<PaginatedAuditLogs> =>
        unwrap(await api.get(BASE, { params: query })),
};

export default auditLogService;
