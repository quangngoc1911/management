'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Reminder } from '@/shared/lib/services/reminderService';
import { reminderStatusLabel } from '@/shared/lib/services/reminderService';

interface GetReminderColumnsArgs {
    onEdit: (reminder: Reminder) => void;
    onDelete: (reminder: Reminder) => void;
}

const formatDateTime = (v: unknown): string =>
    v ? new Date(v as string).toLocaleString('vi-VN') : '—';

export function getReminderColumns({ onEdit, onDelete }: GetReminderColumnsArgs): Column<Reminder>[] {
    return [
        { key: 'title', header: 'Tiêu đề', accessor: 'title' },
        {
            key: 'remindAt',
            header: 'Nhắc lúc',
            accessor: 'remindAt',
            render: (v) => formatDateTime(v),
        },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (v) => reminderStatusLabel(v as number),
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
