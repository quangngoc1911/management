'use client';

import type { Column } from '@/components/DataTable';
import type { BackupLog } from '@/shared/lib/services/backupLogService';

export function formatFileSize(bytes?: number | null): string {
    if (bytes == null) return '—';
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
    if (bytes < 1024 * 1024 * 1024) return `${(bytes / 1024 / 1024).toFixed(1)} MB`;
    return `${(bytes / 1024 / 1024 / 1024).toFixed(2)} GB`;
}

export function statusBadgeClass(status: string): string {
    switch (status?.toLowerCase()) {
        case 'completed':
        case 'success':
            return 'badge-success';
        case 'failed':
        case 'error':
            return 'badge-danger';
        case 'pending':
        case 'running':
            return 'badge-warning';
        default:
            return 'badge-neutral';
    }
}

export function getBackupLogColumns(): Column<BackupLog>[] {
    return [
        { key: 'backupType', header: 'Loại', accessor: 'backupType' },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (v) => (
                <span className={`badge ${statusBadgeClass(v as string)}`}>{(v as string) || '—'}</span>
            ),
        },
        {
            key: 'fileSize',
            header: 'Kích thước',
            accessor: 'fileSize',
            render: (v) => formatFileSize(v as number | null),
        },
        {
            key: 'startedAt',
            header: 'Bắt đầu',
            accessor: 'startedAt',
            render: (v) => (v ? new Date(v as string).toLocaleString('vi-VN') : '—'),
        },
        {
            key: 'completedAt',
            header: 'Hoàn tất',
            accessor: 'completedAt',
            render: (v) => (v ? new Date(v as string).toLocaleString('vi-VN') : '—'),
        },
    ];
}
