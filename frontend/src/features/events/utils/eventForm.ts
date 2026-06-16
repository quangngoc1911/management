import { z } from 'zod';
import type {
    CreateFamilyEventRequest,
    UpdateFamilyEventRequest,
} from '@/shared/lib/services/familyEventService';

export interface EventFormValues {
    title: string;
    eventType?: string;
    startAt: string;
    endAt?: string;
    allDay?: boolean;
    location?: string;
    status?: string;
    description?: string;
}

const nowLocal = () => new Date().toISOString().slice(0, 16);

export const emptyEventForm: EventFormValues = {
    title: '',
    eventType: 'meeting',
    startAt: nowLocal(),
    endAt: '',
    allDay: false,
    location: '',
    status: '1',
    description: '',
};

export const eventSchema = z.object({
    title: z.string().min(2, 'Tiêu đề phải có ít nhất 2 ký tự').max(300),
    eventType: z.string().optional(),
    startAt: z.string().min(1, 'Chọn thời gian bắt đầu'),
    endAt: z.string().optional(),
    allDay: z.boolean().optional(),
    location: z.string().optional(),
    status: z.string().optional(),
    description: z.string().optional(),
});

const s = (v?: string) => (v && v.trim() !== '' ? v.trim() : null);

export const toEventRequest = (d: EventFormValues): CreateFamilyEventRequest & UpdateFamilyEventRequest => ({
    title: d.title.trim(),
    eventType: d.eventType || null,
    startAt: d.startAt,
    endAt: d.endAt && d.endAt.trim() !== '' ? d.endAt : null,
    allDay: d.allDay ?? false,
    location: s(d.location),
    status: d.status ? Number(d.status) : 1,
    description: s(d.description),
});
