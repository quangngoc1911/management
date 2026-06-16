import { createCrudService } from './createCrudService';

export const REMINDER_STATUS_OPTIONS = [
    { value: 1, label: 'Chờ nhắc' },
    { value: 2, label: 'Đã gửi' },
    { value: 3, label: 'Tạm hoãn' },
    { value: 4, label: 'Đã bỏ qua' },
];

export const reminderStatusLabel = (value?: number | null): string =>
    REMINDER_STATUS_OPTIONS.find((o) => o.value === value)?.label ?? '—';

export interface Reminder {
    id: string;
    userId: string;
    memberId?: string | null;
    memberName?: string | null;
    title: string;
    description?: string | null;
    remindAt: string;
    recurrenceRule?: string | null;
    entityType?: string | null;
    entityId?: string | null;
    status: number;
    snoozedUntil?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateReminderRequest {
    memberId?: string | null;
    title: string;
    description?: string | null;
    remindAt: string;
    recurrenceRule?: string | null;
    entityType?: string | null;
    entityId?: string | null;
    status: number;
    snoozedUntil?: string | null;
}

export type UpdateReminderRequest = CreateReminderRequest;

export interface ReminderQuery {
    search?: string;
    status?: number;
    page?: number;
    pageSize?: number;
}

const BASE = '/reminders';

export const reminderService = {
    ...createCrudService<Reminder, CreateReminderRequest, UpdateReminderRequest, ReminderQuery>(BASE),
};

export default reminderService;
