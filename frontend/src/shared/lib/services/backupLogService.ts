import { api } from '../axiosInstance';
import { unwrap } from './createCrudService';

export interface BackupLog {
    id: string;
    backupType: string;
    status: string;
    filePath?: string | null;
    fileSize?: number | null;
    checksum?: string | null;
    startedAt?: string | null;
    completedAt?: string | null;
    errorMessage?: string | null;
    createdAt?: string | null;
}

export interface BackupLogQuery {
    backupType?: string;
    status?: string;
    page?: number;
    pageSize?: number;
}

export interface PaginatedBackupLogs {
    items: BackupLog[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/backup-logs';

export const backupLogService = {
    getAll: async (query: BackupLogQuery = {}): Promise<PaginatedBackupLogs> =>
        unwrap(await api.get(BASE, { params: query })),
};

export default backupLogService;
