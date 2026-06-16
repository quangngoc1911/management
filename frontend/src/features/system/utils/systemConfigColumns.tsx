'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { SystemConfig } from '@/shared/lib/services/systemConfigService';

interface SystemConfigColumnsArgs {
    onEdit: (config: SystemConfig) => void;
    onDelete: (config: SystemConfig) => void;
}

export function getSystemConfigColumns({ onEdit, onDelete }: SystemConfigColumnsArgs): Column<SystemConfig>[] {
    return [
        { key: 'key', header: 'Khoá', accessor: 'key' },
        { key: 'description', header: 'Mô tả', accessor: 'description' },
        {
            key: 'isPublic',
            header: 'Công khai',
            accessor: 'isPublic',
            render: (v) =>
                v ? (
                    <span className="badge badge-success">Có</span>
                ) : (
                    <span className="badge badge-neutral">Không</span>
                ),
        },
        {
            key: 'isEncrypted',
            header: 'Mã hoá',
            accessor: 'isEncrypted',
            render: (v) =>
                v ? (
                    <span className="badge badge-warning">Có</span>
                ) : (
                    <span className="text-muted">—</span>
                ),
        },
        {
            key: 'actions',
            header: 'Thao tác',
            accessor: 'id',
            render: (_v, row) => (
                <RowActions
                    actions={[
                        { icon: Edit, title: 'Sửa', onClick: () => onEdit(row) },
                        { icon: Trash2, title: 'Xóa', variant: 'danger', onClick: () => onDelete(row) },
                    ]}
                />
            ),
        },
    ];
}
