import { z } from 'zod';
import type { CreateReminderRequest } from '@/shared/lib/services/reminderService';

export interface ReminderFormValues {
    title: string;
    description?: string;
    remindAt: string;
    status?: string;
}

const nowLocal = () => new Date().toISOString().slice(0, 16);

export const emptyReminderForm: ReminderFormValues = {
    title: '',
    description: '',
    remindAt: nowLocal(),
    status: '1',
};

export const reminderSchema = z.object({
    title: z.string().min(2, 'Tiêu đề phải có ít nhất 2 ký tự').max(300),
    description: z.string().optional(),
    remindAt: z.string().min(1, 'Chọn thời điểm nhắc'),
    status: z.string().optional(),
});

export const toReminderRequest = (d: ReminderFormValues): CreateReminderRequest => ({
    title: d.title.trim(),
    description: d.description?.trim() || null,
    remindAt: d.remindAt,
    status: d.status ? Number(d.status) : 1,
});
