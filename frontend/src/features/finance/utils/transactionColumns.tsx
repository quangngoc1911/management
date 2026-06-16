import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Transaction } from '@/shared/lib/services/transactionService';
import { transactionTypeLabel, transactionStatusLabel } from '@/shared/lib/services/transactionService';

const formatMoney = (v: number, currency: string) =>
    `${new Intl.NumberFormat('vi-VN').format(v)} ${currency}`;

const typeBadgeClass = (type: string) => {
    switch (type) {
        case 'income':
            return 'badge-success';
        case 'expense':
            return 'badge-danger';
        default:
            return 'badge-info';
    }
};

interface GetTransactionColumnsArgs {
    onEdit: (row: Transaction) => void;
    onDelete: (row: Transaction) => void;
}

export function getTransactionColumns({ onEdit, onDelete }: GetTransactionColumnsArgs): Column<Transaction>[] {
    return [
        {
            key: 'transactionDate',
            header: 'Ngày',
            accessor: 'transactionDate',
            render: (value) => (value ? new Date(value as string).toLocaleDateString('vi-VN') : '—'),
        },
        {
            key: 'type',
            header: 'Loại',
            accessor: 'type',
            render: (value) => (
                <span className={`badge ${typeBadgeClass(value as string)}`}>
                    {transactionTypeLabel(value as string)}
                </span>
            ),
        },
        {
            key: 'amount',
            header: 'Số tiền',
            accessor: 'amount',
            render: (value, row) => formatMoney(value as number, row.currency),
        },
        { key: 'accountName', header: 'Tài khoản', accessor: 'accountName' },
        { key: 'categoryName', header: 'Danh mục', accessor: 'categoryName' },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (value) => transactionStatusLabel(value as number),
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
