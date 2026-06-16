'use client';

import type { ReactNode } from 'react';
import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Document, DocumentStatus } from '@/shared/lib/services/documentService';

export const STATUS_LABELS: Record<DocumentStatus, string> = {
    draft: 'Bản nháp',
    published: 'Đã xuất bản',
    archived: 'Lưu trữ',
};

export function formatDocumentStatus(status: DocumentStatus): ReactNode {
    const badgeClass =
        status === 'published' ? 'badge-success' : status === 'draft' ? 'badge-warning' : 'badge-neutral';
    return <span className={`badge ${badgeClass}`}>{STATUS_LABELS[status]}</span>;
}

interface GetDocumentColumnsArgs {
    onEdit: (doc: Document) => void;
    onDelete: (doc: Document) => void;
}

export function getDocumentColumns({ onEdit, onDelete }: GetDocumentColumnsArgs): Column<Document>[] {
    return [
        { key: 'title', header: 'Tiêu đề', accessor: 'title' },
        { key: 'categoryName', header: 'Danh mục', accessor: 'categoryName' },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (_v, row) => formatDocumentStatus(row.status),
        },
        { key: 'viewCount', header: 'Lượt xem', accessor: 'viewCount' },
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
