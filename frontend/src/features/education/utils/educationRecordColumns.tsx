'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import {
    educationLevelLabel,
    educationStatusLabel,
} from '@/shared/lib/services/educationRecordService';
import type { EducationRecord } from '@/shared/lib/services/educationRecordService';

interface GetEducationRecordColumnsArgs {
    onEdit: (record: EducationRecord) => void;
    onDelete: (record: EducationRecord) => void;
}

export function getEducationRecordColumns({
    onEdit,
    onDelete,
}: GetEducationRecordColumnsArgs): Column<EducationRecord>[] {
    return [
        { key: 'institutionName', header: 'Cơ sở đào tạo', accessor: 'institutionName' },
        { key: 'memberName', header: 'Thành viên', accessor: 'memberName' },
        {
            key: 'level',
            header: 'Cấp học',
            accessor: 'level',
            render: (_v, row) => educationLevelLabel(row.level),
        },
        { key: 'major', header: 'Chuyên ngành', accessor: 'major' },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (_v, row) => educationStatusLabel(row.status),
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
