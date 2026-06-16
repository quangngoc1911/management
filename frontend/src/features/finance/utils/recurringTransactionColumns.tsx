import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { RecurringTransaction } from '@/shared/lib/services/recurringTransactionService';
import { recurringTypeLabel, frequencyLabel } from '@/shared/lib/services/recurringTransactionService';

const formatMoney = (v: number) => new Intl.NumberFormat('vi-VN').format(v);

interface GetRecurringTransactionColumnsArgs {
    onEdit: (row: RecurringTransaction) => void;
    onDelete: (row: RecurringTransaction) => void;
}

export function getRecurringTransactionColumns({
    onEdit,
    onDelete,
}: GetRecurringTransactionColumnsArgs): Column<RecurringTransaction>[] {
    return [
        { key: 'name', header: 'Tên', accessor: 'name' },
        {
            key: 'type',
            header: 'Loại',
            accessor: 'type',
            render: (v) => recurringTypeLabel(v as string),
        },
        {
            key: 'amount',
            header: 'Số tiền',
            accessor: 'amount',
            render: (v) => formatMoney(v as number),
        },
        {
            key: 'frequency',
            header: 'Tần suất',
            accessor: 'frequency',
            render: (v) => frequencyLabel(v as string),
        },
        { key: 'accountName', header: 'Tài khoản', accessor: 'accountName' },
        {
            key: 'nextDueDate',
            header: 'Kỳ kế tiếp',
            accessor: 'nextDueDate',
            render: (v) => (v ? new Date(v as string).toLocaleDateString('vi-VN') : '—'),
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
