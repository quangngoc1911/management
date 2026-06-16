import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Investment } from '@/shared/lib/services/investmentService';
import { investmentTypeLabel } from '@/shared/lib/services/investmentService';

interface GetInvestmentColumnsArgs {
    onEdit: (row: Investment) => void;
    onDelete: (row: Investment) => void;
}

export function getInvestmentColumns({ onEdit, onDelete }: GetInvestmentColumnsArgs): Column<Investment>[] {
    return [
        { key: 'name', header: 'Tên', accessor: 'name' },
        { key: 'symbol', header: 'Mã', accessor: 'symbol' },
        {
            key: 'type',
            header: 'Loại',
            accessor: 'type',
            render: (v) => investmentTypeLabel(v as string),
        },
        { key: 'quantity', header: 'Số lượng', accessor: 'quantity' },
        { key: 'currentPrice', header: 'Giá hiện tại', accessor: 'currentPrice' },
        {
            key: 'isActive',
            header: 'Trạng thái',
            accessor: 'isActive',
            render: (v) =>
                v ? (
                    <span className="badge badge-success">Hoạt động</span>
                ) : (
                    <span className="badge badge-neutral">Ngừng</span>
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
