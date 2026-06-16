'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { FamilyEvent } from '@/shared/lib/services/familyEventService';
import { eventTypeLabel, eventStatusLabel } from '@/shared/lib/services/familyEventService';

interface GetEventColumnsArgs {
    onEdit: (event: FamilyEvent) => void;
    onDelete: (event: FamilyEvent) => void;
}

const formatDateTime = (v: unknown) =>
    v ? new Date(v as string).toLocaleString('vi-VN') : '—';

export function getEventColumns({ onEdit, onDelete }: GetEventColumnsArgs): Column<FamilyEvent>[] {
    return [
        { key: 'title', header: 'Tiêu đề', accessor: 'title' },
        {
            key: 'eventType',
            header: 'Loại',
            accessor: 'eventType',
            render: (_v, row) => eventTypeLabel(row.eventType),
        },
        {
            key: 'startAt',
            header: 'Bắt đầu',
            accessor: 'startAt',
            render: (_v, row) => formatDateTime(row.startAt),
        },
        { key: 'location', header: 'Địa điểm', accessor: 'location' },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (_v, row) => eventStatusLabel(row.status),
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
