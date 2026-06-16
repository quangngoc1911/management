import { z } from 'zod';
import type {
    CreateEducationRecordRequest,
    EducationRecord,
} from '@/shared/lib/services/educationRecordService';

export interface EducationRecordFormValues {
    memberId: string;
    institutionName: string;
    level: string;
    major?: string;
    degree?: string;
    studentId?: string;
    startDate?: string;
    endDate?: string;
    gpa?: string;
    status?: string;
    achievements?: string;
    notes?: string;
}

export const emptyEducationRecordForm: EducationRecordFormValues = {
    memberId: '',
    institutionName: '',
    level: 'university',
    major: '',
    degree: '',
    studentId: '',
    startDate: '',
    endDate: '',
    gpa: '',
    status: '1',
    achievements: '',
    notes: '',
};

export const educationRecordSchema = z.object({
    memberId: z.string().min(1, 'Chọn thành viên'),
    institutionName: z.string().min(2, 'Tên cơ sở phải có ít nhất 2 ký tự').max(300),
    level: z.string().min(1, 'Chọn cấp học'),
    major: z.string().optional(),
    degree: z.string().optional(),
    studentId: z.string().optional(),
    startDate: z.string().optional(),
    endDate: z.string().optional(),
    gpa: z.string().optional(),
    status: z.string().optional(),
    achievements: z.string().optional(),
    notes: z.string().optional(),
});

const s = (v?: string) => (v && v.trim() !== '' ? v.trim() : null);

export const toEducationRecordRequest = (
    d: EducationRecordFormValues,
): CreateEducationRecordRequest => ({
    memberId: d.memberId,
    institutionName: d.institutionName.trim(),
    level: d.level,
    major: s(d.major),
    degree: s(d.degree),
    studentId: s(d.studentId),
    startDate: d.startDate && d.startDate.trim() !== '' ? d.startDate : null,
    endDate: d.endDate && d.endDate.trim() !== '' ? d.endDate : null,
    gpa: d.gpa && d.gpa.trim() !== '' ? Number(d.gpa) : null,
    status: d.status ? Number(d.status) : 1,
    achievements: s(d.achievements),
    notes: s(d.notes),
});

export const toEducationRecordFormValues = (r: EducationRecord): EducationRecordFormValues => ({
    memberId: r.memberId,
    institutionName: r.institutionName,
    level: r.level,
    major: r.major ?? '',
    degree: r.degree ?? '',
    studentId: r.studentId ?? '',
    startDate: r.startDate ? r.startDate.slice(0, 10) : '',
    endDate: r.endDate ? r.endDate.slice(0, 10) : '',
    gpa: r.gpa != null ? String(r.gpa) : '',
    status: String(r.status),
    achievements: r.achievements ?? '',
    notes: r.notes ?? '',
});
