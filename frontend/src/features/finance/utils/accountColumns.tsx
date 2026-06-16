import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Account } from '@/shared/lib/services/accountService';
import { accountTypeLabel } from '@/shared/lib/services/accountService';

interface GetAccountColumnsArgs {
    onEdit: (row: Account) => void;
    onDelete: (row: Account) => void;
}

export function getAccountColumns({ onEdit, onDelete }: GetAccountColumnsArgs): Column<Account>[] {
    return [
        { key: 'name', header: 'Tên tài khoản', accessor: 'name' },
        {
            key: 'accountType',
            header: 'Loại',
            accessor: 'accountType',
            render: (value) => accountTypeLabel(value as string | null),
        },
        { key: 'bankName', header: 'Ngân hàng', accessor: 'bankName' },
        { key: 'currency', header: 'Tiền tệ', accessor: 'currency' },
        {
            key: 'isActive',
            header: 'Trạng thái',
            accessor: 'isActive',
            render: (value) =>
                value ? (
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
