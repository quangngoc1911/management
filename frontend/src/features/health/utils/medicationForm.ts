import { z } from 'zod';
import type { CreateMedicationRequest } from '@/shared/lib/services/medicationService';

export interface MedicationFormValues {
    memberId: string;
    name: string;
    dosage?: string;
    frequency?: string;
    startDate: string;
    endDate?: string;
    isActive?: boolean;
    notes?: string;
}

const today = () => new Date().toISOString().slice(0, 10);

export const emptyMedicationForm: MedicationFormValues = {
    memberId: '',
    name: '',
    dosage: '',
    frequency: '',
    startDate: today(),
    endDate: '',
    isActive: true,
    notes: '',
};

export const medicationSchema = z.object({
    memberId: z.string().min(1, 'Chọn thành viên'),
    name: z.string().min(2, 'Tên thuốc phải có ít nhất 2 ký tự').max(200),
    dosage: z.string().optional(),
    frequency: z.string().optional(),
    startDate: z.string().min(1, 'Chọn ngày bắt đầu'),
    endDate: z.string().optional(),
    isActive: z.boolean().optional(),
    notes: z.string().optional(),
});

const s = (v?: string) => (v && v.trim() !== '' ? v.trim() : null);

export const toMedicationRequest = (d: MedicationFormValues): CreateMedicationRequest => ({
    memberId: d.memberId,
    name: d.name.trim(),
    dosage: s(d.dosage),
    frequency: s(d.frequency),
    startDate: d.startDate,
    endDate: d.endDate && d.endDate.trim() !== '' ? d.endDate : null,
    isActive: d.isActive ?? true,
    notes: s(d.notes),
});
