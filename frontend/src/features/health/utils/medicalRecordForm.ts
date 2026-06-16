import { z } from 'zod';
import type {
    CreateMedicalRecordRequest,
} from '@/shared/lib/services/medicalRecordService';

export interface MedicalRecordFormValues {
    memberId: string;
    recordType: string;
    title: string;
    recordDate: string;
    followUpDate?: string;
    diagnosis?: string;
    treatment?: string;
    doctorName?: string;
    hospitalName?: string;
    isPrivate?: boolean;
    notes?: string;
}

const today = () => new Date().toISOString().slice(0, 10);

export const emptyMedicalRecordForm: MedicalRecordFormValues = {
    memberId: '',
    recordType: 'checkup',
    title: '',
    recordDate: today(),
    followUpDate: '',
    diagnosis: '',
    treatment: '',
    doctorName: '',
    hospitalName: '',
    isPrivate: false,
    notes: '',
};

export const medicalRecordSchema = z.object({
    memberId: z.string().min(1, 'Chọn thành viên'),
    recordType: z.string().min(1, 'Chọn loại hồ sơ'),
    title: z.string().min(2, 'Tiêu đề phải có ít nhất 2 ký tự').max(300),
    recordDate: z.string().min(1, 'Chọn ngày khám'),
    followUpDate: z.string().optional(),
    diagnosis: z.string().optional(),
    treatment: z.string().optional(),
    doctorName: z.string().optional(),
    hospitalName: z.string().optional(),
    isPrivate: z.boolean().optional(),
    notes: z.string().optional(),
});

const s = (v?: string) => (v && v.trim() !== '' ? v.trim() : null);

export const toMedicalRecordRequest = (d: MedicalRecordFormValues): CreateMedicalRecordRequest => ({
    memberId: d.memberId,
    recordType: d.recordType,
    title: d.title.trim(),
    recordDate: d.recordDate,
    followUpDate: d.followUpDate && d.followUpDate.trim() !== '' ? d.followUpDate : null,
    diagnosis: s(d.diagnosis),
    treatment: s(d.treatment),
    doctorName: s(d.doctorName),
    hospitalName: s(d.hospitalName),
    isPrivate: d.isPrivate ?? false,
    notes: s(d.notes),
});
