import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import type { HealthMetric } from '@/shared/lib/services/healthMetricService';
import { metricTypeLabel } from '@/shared/lib/services/healthMetricService';

interface GetHealthMetricColumnsArgs {
    onEdit: (row: HealthMetric) => void;
    onDelete: (row: HealthMetric) => void;
}

export function getHealthMetricColumns({
    onEdit,
    onDelete,
}: GetHealthMetricColumnsArgs): Column<HealthMetric>[] {
    return [
        {
            key: 'metricType',
            header: 'Chỉ số',
            accessor: 'metricType',
            render: (v) => metricTypeLabel(v as string),
        },
        { key: 'memberName', header: 'Thành viên', accessor: 'memberName' },
        {
            key: 'value',
            header: 'Giá trị',
            accessor: 'value',
            render: (v, row) => `${v} ${row.unit}`,
        },
        {
            key: 'measuredAt',
            header: 'Thời điểm',
            accessor: 'measuredAt',
            render: (v) => (v ? new Date(v as string).toLocaleString('vi-VN') : '—'),
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
