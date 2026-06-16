import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { Medication } from '@/shared/lib/services/medicationService';

interface GetMedicationColumnsArgs {
    onEdit: (row: Medication) => void;
    onDelete: (row: Medication) => void;
}

export function getMedicationColumns({
    onEdit,
    onDelete,
}: GetMedicationColumnsArgs): Column<Medication>[] {
    return [
        { key: 'name', header: 'Tên thuốc', accessor: 'name' },
        { key: 'memberName', header: 'Thành viên', accessor: 'memberName' },
        { key: 'dosage', header: 'Liều', accessor: 'dosage' },
        { key: 'frequency', header: 'Tần suất', accessor: 'frequency' },
        {
            key: 'isActive',
            header: 'Trạng thái',
            accessor: 'isActive',
            render: (v) =>
                v ? (
                    <span className="badge badge-success">Đang dùng</span>
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
