import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Budget } from '@/shared/lib/services/budgetService';
import { periodTypeLabel } from '@/shared/lib/services/budgetService';

const formatMoney = (v: number, c: string) => `${new Intl.NumberFormat('vi-VN').format(v)} ${c}`;

interface GetBudgetColumnsArgs {
    onEdit: (row: Budget) => void;
    onDelete: (row: Budget) => void;
}

export function getBudgetColumns({ onEdit, onDelete }: GetBudgetColumnsArgs): Column<Budget>[] {
    return [
        { key: 'name', header: 'Tên', accessor: 'name' },
        {
            key: 'amount',
            header: 'Hạn mức',
            accessor: 'amount',
            render: (v, row) => formatMoney(v as number, row.currency),
        },
        {
            key: 'periodType',
            header: 'Kỳ',
            accessor: 'periodType',
            render: (v) => periodTypeLabel(v as string),
        },
        {
            key: 'startDate',
            header: 'Bắt đầu',
            accessor: 'startDate',
            render: (v) => (v ? new Date(v as string).toLocaleDateString('vi-VN') : '—'),
        },
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
