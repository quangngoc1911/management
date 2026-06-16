'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { AssetValuation } from '@/shared/lib/services/assetValuationService';

interface GetAssetValuationColumnsArgs {
    onEdit: (valuation: AssetValuation) => void;
    onDelete: (valuation: AssetValuation) => void;
}

const formatDate = (v: unknown) =>
    v ? new Date(v as string).toLocaleDateString('vi-VN') : '—';

const formatMoney = (v: unknown, row: AssetValuation) =>
    `${new Intl.NumberFormat('vi-VN').format(v as number)} ${row.currency}`;

export function getAssetValuationColumns({
    onEdit,
    onDelete,
}: GetAssetValuationColumnsArgs): Column<AssetValuation>[] {
    return [
        { key: 'assetName', header: 'Tài sản', accessor: 'assetName' },
        {
            key: 'valuationDate',
            header: 'Ngày định giá',
            accessor: 'valuationDate',
            render: (v) => formatDate(v),
        },
        {
            key: 'value',
            header: 'Giá trị',
            accessor: 'value',
            render: (v, row) => formatMoney(v, row),
        },
        { key: 'valuationMethod', header: 'Phương pháp', accessor: 'valuationMethod' },
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
