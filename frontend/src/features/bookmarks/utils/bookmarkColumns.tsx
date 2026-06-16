'use client';

import { Edit, Trash2, ExternalLink } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Bookmark } from '@/shared/lib/services/bookmarkService';

interface GetBookmarkColumnsArgs {
    onEdit: (bookmark: Bookmark) => void;
    onDelete: (bookmark: Bookmark) => void;
    onOpenLink: (bookmark: Bookmark) => void;
}

const formatDate = (v: unknown): string =>
    v ? new Date(v as string).toLocaleDateString('vi-VN') : '—';

export function getBookmarkColumns({
    onEdit,
    onDelete,
    onOpenLink,
}: GetBookmarkColumnsArgs): Column<Bookmark>[] {
    return [
        { key: 'entityType', header: 'Loại', accessor: 'entityType' },
        { key: 'entityId', header: 'ID đối tượng', accessor: 'entityId' },
        { key: 'note', header: 'Ghi chú', accessor: 'note' },
        {
            key: 'createdAt',
            header: 'Ngày đánh dấu',
            accessor: 'createdAt',
            render: (v) => formatDate(v),
        },
        {
            key: 'actions',
            header: 'Thao tác',
            accessor: 'id',
            render: (_v, row) => (
                <RowActions
                    actions={[
                        { icon: ExternalLink, title: 'Mở liên kết', onClick: () => onOpenLink(row) },
                        { icon: Edit, title: 'Sửa', onClick: () => onEdit(row) },
                        { icon: Trash2, title: 'Xóa', variant: 'danger', onClick: () => onDelete(row) },
                    ]}
                />
            ),
        },
    ];
}
