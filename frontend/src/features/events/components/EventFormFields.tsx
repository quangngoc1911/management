'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { EVENT_TYPE_OPTIONS, EVENT_STATUS_OPTIONS } from '@/shared/lib/services/familyEventService';
import type { EventFormValues } from '../utils/eventForm';

interface EventFormFieldsProps {
    register: UseFormRegister<EventFormValues>;
    errors: FieldErrors<EventFormValues>;
}

export function EventFormFields({ register, errors }: EventFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Tiêu đề" required error={errors.title?.message} full>
                <input
                    {...register('title')}
                    className={`input ${errors.title ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Loại sự kiện">
                <select {...register('eventType')} className="input">
                    {EVENT_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Trạng thái">
                <select {...register('status')} className="input">
                    {EVENT_STATUS_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Bắt đầu" required error={errors.startAt?.message}>
                <input {...register('startAt')} type="datetime-local" className="input" />
            </FormField>

            <FormField label="Kết thúc">
                <input {...register('endAt')} type="datetime-local" className="input" />
            </FormField>

            <FormField label="Địa điểm" full>
                <input {...register('location')} className="input" />
            </FormField>

            <FormField label="Mô tả" full>
                <textarea {...register('description')} rows={2} className="input" />
            </FormField>

            <div className="md:col-span-2 flex items-center gap-2">
                <input
                    id="allDay"
                    type="checkbox"
                    {...register('allDay')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="allDay" className="text-sm font-medium text-foreground">
                    Cả ngày
                </label>
            </div>
        </form>
    );
}

export default EventFormFields;
