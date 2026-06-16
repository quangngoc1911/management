import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { MedicalRecord } from '@/shared/lib/services/medicalRecordService';
import { recordTypeLabel } from '@/shared/lib/services/medicalRecordService';

interface GetMedicalRecordColumnsArgs {
    onEdit: (row: MedicalRecord) => void;
    onDelete: (row: MedicalRecord) => void;
}

export function getMedicalRecordColumns({
    onEdit,
    onDelete,
}: GetMedicalRecordColumnsArgs): Column<MedicalRecord>[] {
    return [
        { key: 'title', header: 'Tiêu đề', accessor: 'title' },
        { key: 'memberName', header: 'Thành viên', accessor: 'memberName' },
        {
            key: 'recordType',
            header: 'Loại',
            accessor: 'recordType',
            render: (v) => recordTypeLabel(v as string),
        },
        {
            key: 'recordDate',
            header: 'Ngày khám',
            accessor: 'recordDate',
            render: (v) => (v ? new Date(v as string).toLocaleDateString('vi-VN') : '—'),
        },
        { key: 'doctorName', header: 'Bác sĩ', accessor: 'doctorName' },
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
