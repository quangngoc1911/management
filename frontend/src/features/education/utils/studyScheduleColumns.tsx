'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import { studyScheduleStatusLabel } from '@/shared/lib/services/studyScheduleService';
import type { StudySchedule } from '@/shared/lib/services/studyScheduleService';

interface GetStudyScheduleColumnsArgs {
    onEdit: (schedule: StudySchedule) => void;
    onDelete: (schedule: StudySchedule) => void;
}

export function getStudyScheduleColumns({
    onEdit,
    onDelete,
}: GetStudyScheduleColumnsArgs): Column<StudySchedule>[] {
    return [
        { key: 'title', header: 'Tiêu đề', accessor: 'title' },
        { key: 'memberName', header: 'Thành viên', accessor: 'memberName' },
        { key: 'subject', header: 'Môn học', accessor: 'subject' },
        {
            key: 'startTime',
            header: 'Bắt đầu',
            accessor: 'startTime',
            render: (_v, row) =>
                row.startTime ? new Date(row.startTime).toLocaleString('vi-VN') : '—',
        },
        {
            key: 'status',
            header: 'Trạng thái',
            accessor: 'status',
            render: (_v, row) => studyScheduleStatusLabel(row.status),
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
