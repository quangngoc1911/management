import { createCrudService } from './createCrudService';

export const RECORD_TYPE_OPTIONS = [
    { value: 'checkup', label: 'Khám tổng quát' },
    { value: 'diagnosis', label: 'Chẩn đoán' },
    { value: 'surgery', label: 'Phẫu thuật' },
    { value: 'vaccination', label: 'Tiêm chủng' },
    { value: 'test', label: 'Xét nghiệm' },
    { value: 'other', label: 'Khác' },
];

export const recordTypeLabel = (value?: string | null): string =>
    RECORD_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface MedicalRecord {
    id: string;
    memberId: string;
    memberName: string;
    recordType: string;
    title: string;
    diagnosis?: string | null;
    treatment?: string | null;
    doctorName?: string | null;
    hospitalName?: string | null;
    recordDate: string;
    followUpDate?: string | null;
    isPrivate: boolean;
    notes?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateMedicalRecordRequest {
    memberId: string;
    recordType: string;
    title: string;
    diagnosis?: string | null;
    treatment?: string | null;
    doctorName?: string | null;
    hospitalName?: string | null;
    recordDate: string;
    followUpDate?: string | null;
    isPrivate: boolean;
    notes?: string | null;
}

export type UpdateMedicalRecordRequest = CreateMedicalRecordRequest;

export interface MedicalRecordQuery {
    search?: string;
    memberId?: string;
    recordType?: string;
    page?: number;
    pageSize?: number;
}

export interface PaginatedMedicalRecords {
    items: MedicalRecord[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/medical-records';

export const medicalRecordService = createCrudService<
    MedicalRecord,
    CreateMedicalRecordRequest,
    UpdateMedicalRecordRequest,
    MedicalRecordQuery
>(BASE);

export default medicalRecordService;
