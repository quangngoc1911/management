'use client';

import type { ReactNode } from 'react';
import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Tag } from '@/shared/lib/services/tagService';

// Hex colour options matching the backend validator (Tag.Color stores #RRGGBB).
export const TAG_COLOR_OPTIONS = [
    { value: '', label: 'Mặc định' },
    { value: '#2563eb', label: 'Xanh dương' },
    { value: '#16a34a', label: 'Xanh lá' },
    { value: '#dc2626', label: 'Đỏ' },
    { value: '#d97706', label: 'Vàng' },
    { value: '#7c3aed', label: 'Tím' },
    { value: '#db2777', label: 'Hồng' },
    { value: '#6b7280', label: 'Xám' },
];

export function formatTagColor(color?: string): ReactNode {
    return (
        <span className="inline-flex items-center gap-2">
            <span
                className="w-3 h-3 rounded-full border border-border"
                style={{ backgroundColor: color || 'transparent' }}
            />
            <span className="text-foreground">
                {TAG_COLOR_OPTIONS.find((c) => c.value === color)?.label || color || 'Mặc định'}
            </span>
        </span>
    );
}

interface GetTagColumnsArgs {
    onEdit: (tag: Tag) => void;
    onDelete: (tag: Tag) => void;
}

export function getTagColumns({ onEdit, onDelete }: GetTagColumnsArgs): Column<Tag>[] {
    return [
        { key: 'name', header: 'Tên', accessor: 'name' },
        { key: 'slug', header: 'Slug', accessor: 'slug' },
        {
            key: 'color',
            header: 'Màu',
            accessor: 'color',
            render: (_v, row) => formatTagColor(row.color),
        },
        { key: 'documentCount', header: 'Số tài liệu', accessor: 'documentCount' },
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
