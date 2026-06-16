import { createCrudService } from './createCrudService';

export const METRIC_TYPE_OPTIONS = [
    { value: 'weight', label: 'Cân nặng' },
    { value: 'height', label: 'Chiều cao' },
    { value: 'bmi', label: 'BMI' },
    { value: 'blood_pressure', label: 'Huyết áp' },
    { value: 'heart_rate', label: 'Nhịp tim' },
    { value: 'blood_sugar', label: 'Đường huyết' },
    { value: 'temperature', label: 'Nhiệt độ' },
    { value: 'other', label: 'Khác' },
];

export const metricTypeLabel = (value?: string | null): string =>
    METRIC_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface HealthMetric {
    id: string;
    memberId: string;
    memberName: string;
    metricType: string;
    value: number;
    unit: string;
    measuredAt: string;
    notes?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateHealthMetricRequest {
    memberId: string;
    metricType: string;
    value: number;
    unit: string;
    measuredAt: string;
    notes?: string | null;
}

export type UpdateHealthMetricRequest = CreateHealthMetricRequest;

export interface HealthMetricQuery {
    search?: string;
    memberId?: string;
    metricType?: string;
    page?: number;
    pageSize?: number;
}

export interface PaginatedHealthMetrics {
    items: HealthMetric[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/health-metrics';

export const healthMetricService = createCrudService<
    HealthMetric,
    CreateHealthMetricRequest,
    UpdateHealthMetricRequest,
    HealthMetricQuery
>(BASE);

export default healthMetricService;
