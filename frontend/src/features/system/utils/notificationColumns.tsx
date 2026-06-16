'use client';

import { Mail, MailOpen, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Notification } from '@/shared/lib/services/notificationService';

interface NotificationColumnsArgs {
    onToggleRead: (notification: Notification) => void;
    onDelete: (notification: Notification) => void;
}

export function getNotificationColumns({ onToggleRead, onDelete }: NotificationColumnsArgs): Column<Notification>[] {
    return [
        {
            key: 'title',
            header: 'Tiêu đề',
            accessor: 'title',
            render: (_v, row) => (
                <span className={row.isRead ? 'text-muted' : 'font-semibold text-foreground'}>{row.title}</span>
            ),
        },
        { key: 'type', header: 'Loại', accessor: 'type' },
        {
            key: 'isRead',
            header: 'Trạng thái',
            accessor: 'isRead',
            render: (v) =>
                v ? (
                    <span className="badge badge-neutral">Đã đọc</span>
                ) : (
                    <span className="badge badge-info">Chưa đọc</span>
                ),
        },
        {
            key: 'createdAt',
            header: 'Thời gian',
            accessor: 'createdAt',
            render: (v) => (v ? new Date(v as string).toLocaleString('vi-VN') : '—'),
        },
        {
            key: 'actions',
            header: 'Thao tác',
            accessor: 'id',
            render: (_v, row) => (
                <RowActions
                    actions={[
                        {
                            icon: row.isRead ? Mail : MailOpen,
                            title: row.isRead ? 'Đánh dấu chưa đọc' : 'Đánh dấu đã đọc',
                            onClick: () => onToggleRead(row),
                        },
                        { icon: Trash2, title: 'Xóa', variant: 'danger', onClick: () => onDelete(row) },
                    ]}
                />
            ),
        },
    ];
}
