'use client';

import type { Column } from '@/components/DataTable';
import { auditActionLabel } from '@/shared/lib/services/auditLogService';
import type { AuditLog } from '@/shared/lib/services/auditLogService';

export function getAuditLogColumns(): Column<AuditLog>[] {
    return [
        {
            key: 'action',
            header: 'Hành động',
            accessor: 'action',
            render: (v) => auditActionLabel(v as number),
        },
        { key: 'entityType', header: 'Đối tượng', accessor: 'entityType' },
        { key: 'userName', header: 'Người dùng', accessor: 'userName' },
        { key: 'ipAddress', header: 'IP', accessor: 'ipAddress' },
        {
            key: 'createdAt',
            header: 'Thời gian',
            accessor: 'createdAt',
            render: (v) => (v ? new Date(v as string).toLocaleString('vi-VN') : '—'),
        },
    ];
}
