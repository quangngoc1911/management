import { createCrudService } from './createCrudService';

export interface Medication {
    id: string;
    memberId: string;
    memberName: string;
    medicalRecordId?: string | null;
    name: string;
    dosage?: string | null;
    frequency?: string | null;
    startDate: string;
    endDate?: string | null;
    reminderTimes?: string | null;
    isActive: boolean;
    notes?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateMedicationRequest {
    memberId: string;
    medicalRecordId?: string | null;
    name: string;
    dosage?: string | null;
    frequency?: string | null;
    startDate: string;
    endDate?: string | null;
    reminderTimes?: string | null;
    isActive: boolean;
    notes?: string | null;
}

export type UpdateMedicationRequest = CreateMedicationRequest;

export interface MedicationQuery {
    search?: string;
    memberId?: string;
    isActive?: boolean;
    page?: number;
    pageSize?: number;
}

export interface PaginatedMedications {
    items: Medication[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/medications';

export const medicationService = createCrudService<
    Medication,
    CreateMedicationRequest,
    UpdateMedicationRequest,
    MedicationQuery
>(BASE);

export default medicationService;
