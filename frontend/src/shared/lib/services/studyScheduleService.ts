import { createCrudService } from './createCrudService';

export const STUDY_SCHEDULE_STATUS_OPTIONS = [
    { value: 1, label: 'Đã lên lịch' },
    { value: 2, label: 'Hoàn tất' },
    { value: 3, label: 'Đã huỷ' },
];

export const studyScheduleStatusLabel = (value?: number | null): string =>
    STUDY_SCHEDULE_STATUS_OPTIONS.find((o) => o.value === value)?.label ?? '—';

export interface StudySchedule {
    id: string;
    memberId: string;
    memberName: string;
    educationRecordId?: string | null;
    title: string;
    subject?: string | null;
    startTime: string;
    endTime: string;
    recurrenceRule?: string | null;
    location?: string | null;
    isOnline?: boolean | null;
    meetingUrl?: string | null;
    teacherName?: string | null;
    status: number;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateStudyScheduleRequest {
    memberId: string;
    educationRecordId?: string | null;
    title: string;
    subject?: string | null;
    startTime: string;
    endTime: string;
    recurrenceRule?: string | null;
    location?: string | null;
    isOnline?: boolean | null;
    meetingUrl?: string | null;
    teacherName?: string | null;
    status: number;
}

export type UpdateStudyScheduleRequest = CreateStudyScheduleRequest;

export interface StudyScheduleQuery {
    search?: string;
    memberId?: string;
    status?: number;
    page?: number;
    pageSize?: number;
}

export interface PaginatedStudySchedules {
    items: StudySchedule[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/study-schedules';

export const studyScheduleService = createCrudService<
    StudySchedule,
    CreateStudyScheduleRequest,
    UpdateStudyScheduleRequest,
    StudyScheduleQuery
>(BASE);

export default studyScheduleService;
