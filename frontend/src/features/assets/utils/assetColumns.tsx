'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Asset } from '@/shared/lib/services/assetService';
import { assetTypeLabel, assetStatusLabel } from '@/shared/lib/services/assetService';

interface GetAssetColumnsArgs {
    onEdit: (asset: Asset) => void;
    onDelete: (asset: Asset) => void;
}

const formatMoney = (v: unknown, row: Asset) =>
    v != null ? `${new Intl.NumberFormat('vi-VN').format(v as number)} ${row.currency ?? ''}`.trim() : '—';

export function getAssetColumns({ onEdit, onDelete }: GetAssetColumnsArgs): Column<Asset>[] {
    return [
        { key: 'name', header: 'Tên tài sản', accessor: 'name' },
        {
            key: 'assetType',
            header: 'Loại',
            accessor: 'assetType',
            render: (_v, row) => assetTypeLabel(row.assetType),
        },
        {
            key: 'purchasePrice',
            header: 'Giá mua',
            accessor: 'purchasePrice',
            render: (v, row) => formatMoney(v, row),
        },
        { key: 'location', header: 'Vị trí', accessor: 'location' },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (_v, row) => assetStatusLabel(row.status),
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
