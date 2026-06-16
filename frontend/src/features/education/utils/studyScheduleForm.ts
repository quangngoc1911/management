import { z } from 'zod';
import type {
    CreateStudyScheduleRequest,
    StudySchedule,
} from '@/shared/lib/services/studyScheduleService';

const nowLocal = () => new Date().toISOString().slice(0, 16);

export interface StudyScheduleFormValues {
    memberId: string;
    title: string;
    subject?: string;
    startTime: string;
    endTime: string;
    location?: string;
    isOnline?: boolean;
    meetingUrl?: string;
    teacherName?: string;
    status?: string;
}

export const emptyStudyScheduleForm: StudyScheduleFormValues = {
    memberId: '',
    title: '',
    subject: '',
    startTime: nowLocal(),
    endTime: nowLocal(),
    location: '',
    isOnline: false,
    meetingUrl: '',
    teacherName: '',
    status: '1',
};

export const studyScheduleSchema = z
    .object({
        memberId: z.string().min(1, 'Chọn thành viên'),
        title: z.string().min(2, 'Tiêu đề phải có ít nhất 2 ký tự').max(300),
        subject: z.string().optional(),
        startTime: z.string().min(1, 'Chọn thời gian bắt đầu'),
        endTime: z.string().min(1, 'Chọn thời gian kết thúc'),
        location: z.string().optional(),
        isOnline: z.boolean().optional(),
        meetingUrl: z.string().optional(),
        teacherName: z.string().optional(),
        status: z.string().optional(),
    })
    .refine((d) => d.endTime >= d.startTime, {
        message: 'Thời gian kết thúc phải sau thời gian bắt đầu',
        path: ['endTime'],
    });

const s = (v?: string) => (v && v.trim() !== '' ? v.trim() : null);

export const toStudyScheduleRequest = (d: StudyScheduleFormValues): CreateStudyScheduleRequest => ({
    memberId: d.memberId,
    title: d.title.trim(),
    subject: s(d.subject),
    startTime: d.startTime,
    endTime: d.endTime,
    location: s(d.location),
    isOnline: d.isOnline ?? false,
    meetingUrl: s(d.meetingUrl),
    teacherName: s(d.teacherName),
    status: d.status ? Number(d.status) : 1,
});

export const toStudyScheduleFormValues = (sch: StudySchedule): StudyScheduleFormValues => ({
    memberId: sch.memberId,
    title: sch.title,
    subject: sch.subject ?? '',
    startTime: sch.startTime ? sch.startTime.slice(0, 16) : nowLocal(),
    endTime: sch.endTime ? sch.endTime.slice(0, 16) : nowLocal(),
    location: sch.location ?? '',
    isOnline: sch.isOnline ?? false,
    meetingUrl: sch.meetingUrl ?? '',
    teacherName: sch.teacherName ?? '',
    status: String(sch.status),
});
