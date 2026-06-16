import { z } from 'zod';
import type { CreateHealthMetricRequest } from '@/shared/lib/services/healthMetricService';

export interface HealthMetricFormValues {
    memberId: string;
    metricType: string;
    value: string;
    unit: string;
    measuredAt: string;
    notes?: string;
}

const nowLocal = () => new Date().toISOString().slice(0, 16);

export const emptyHealthMetricForm: HealthMetricFormValues = {
    memberId: '',
    metricType: 'weight',
    value: '',
    unit: 'kg',
    measuredAt: nowLocal(),
    notes: '',
};

export const healthMetricSchema = z.object({
    memberId: z.string().min(1, 'Chọn thành viên'),
    metricType: z.string().min(1, 'Chọn loại chỉ số'),
    value: z.string().min(1, 'Nhập giá trị'),
    unit: z.string().min(1, 'Nhập đơn vị'),
    measuredAt: z.string().min(1, 'Chọn thời điểm đo'),
    notes: z.string().optional(),
});

export const toHealthMetricRequest = (d: HealthMetricFormValues): CreateHealthMetricRequest => ({
    memberId: d.memberId,
    metricType: d.metricType,
    value: Number(d.value),
    unit: d.unit.trim(),
    measuredAt: d.measuredAt,
    notes: d.notes?.trim() || null,
});
