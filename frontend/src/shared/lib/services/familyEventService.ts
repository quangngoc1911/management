import { createCrudService } from './createCrudService';

export const EVENT_STATUS_OPTIONS = [
    { value: 1, label: 'Đã lên kế hoạch' },
    { value: 2, label: 'Đang diễn ra' },
    { value: 3, label: 'Đã hoàn tất' },
    { value: 4, label: 'Đã huỷ' },
];

export const EVENT_TYPE_OPTIONS = [
    { value: 'birthday', label: 'Sinh nhật' },
    { value: 'anniversary', label: 'Kỷ niệm' },
    { value: 'holiday', label: 'Ngày lễ' },
    { value: 'meeting', label: 'Họp mặt' },
    { value: 'trip', label: 'Du lịch' },
    { value: 'other', label: 'Khác' },
];

export const eventStatusLabel = (value?: number | null): string =>
    EVENT_STATUS_OPTIONS.find((o) => o.value === value)?.label ?? '—';

export const eventTypeLabel = (value?: string | null): string =>
    EVENT_TYPE_OPTIONS.find((o) => o.value === value)?.label ?? value ?? '—';

export interface FamilyEvent {
    id: string;
    title: string;
    description?: string | null;
    eventType?: string | null;
    startAt: string;
    endAt?: string | null;
    allDay: boolean;
    location?: string | null;
    recurrenceRule?: string | null;
    status: number;
    coverFileId?: string | null;
    createdByUserId: string;
    createdByUserName?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateFamilyEventRequest {
    title: string;
    description?: string | null;
    eventType?: string | null;
    startAt: string;
    endAt?: string | null;
    allDay: boolean;
    location?: string | null;
    recurrenceRule?: string | null;
    status: number;
    coverFileId?: string | null;
}

export type UpdateFamilyEventRequest = CreateFamilyEventRequest;

export interface FamilyEventQuery {
    search?: string;
    eventType?: string;
    status?: number;
    page?: number;
    pageSize?: number;
}

const BASE = '/family-events';

export const familyEventService = createCrudService<
    FamilyEvent,
    CreateFamilyEventRequest,
    UpdateFamilyEventRequest,
    FamilyEventQuery
>(BASE);

export default familyEventService;
