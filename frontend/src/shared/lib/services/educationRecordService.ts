import { createCrudService } from './createCrudService';

export const EDUCATION_STATUS_OPTIONS = [
    { value: 1, label: 'Đang học' },
    { value: 2, label: 'Đã tốt nghiệp' },
    { value: 3, label: 'Đã nghỉ' },
    { value: 4, label: 'Bảo lưu' },
];

export const EDUCATION_LEVEL_OPTIONS = [
    { value: 'primary', label: 'Tiểu học' },
    { value: 'secondary', label: 'THCS' },
    { value: 'highschool', label: 'THPT' },
    { value: 'vocational', label: 'Trung cấp/Nghề' },
    { value: 'college', label: 'Cao đẳng' },
    { value: 'university', label: 'Đại học' },
    { value: 'postgraduate', label: 'Sau đại học' },
];

export const educationStatusLabel = (value?: number | null): string =>
    EDUCATION_STATUS_OPTIONS.find((o) => o.value === value)?.label ?? '—';

export const educationLevelLabel = (value?: string | null): string =>
    EDUCATION_LEVEL_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface EducationRecord {
    id: string;
    memberId: string;
    memberName: string;
    institutionName: string;
    level: string;
    major?: string | null;
    degree?: string | null;
    studentId?: string | null;
    startDate?: string | null;
    endDate?: string | null;
    gpa?: number | null;
    status: number;
    achievements?: string | null;
    notes?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateEducationRecordRequest {
    memberId: string;
    institutionName: string;
    level: string;
    major?: string | null;
    degree?: string | null;
    studentId?: string | null;
    startDate?: string | null;
    endDate?: string | null;
    gpa?: number | null;
    status: number;
    achievements?: string | null;
    notes?: string | null;
}

export type UpdateEducationRecordRequest = CreateEducationRecordRequest;

export interface EducationRecordQuery {
    search?: string;
    memberId?: string;
    level?: string;
    status?: number;
    page?: number;
    pageSize?: number;
}

export interface PaginatedEducationRecords {
    items: EducationRecord[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/education-records';

export const educationRecordService = createCrudService<
    EducationRecord,
    CreateEducationRecordRequest,
    UpdateEducationRecordRequest,
    EducationRecordQuery
>(BASE);

export default educationRecordService;
