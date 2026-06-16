'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { ReminderFormValues } from '../utils/reminderForm';
import { REMINDER_STATUS_OPTIONS } from '@/shared/lib/services/reminderService';

interface ReminderFormFieldsProps {
    register: UseFormRegister<ReminderFormValues>;
    errors: FieldErrors<ReminderFormValues>;
}

export function ReminderFormFields({ register, errors }: ReminderFormFieldsProps) {
    return (
        <form className="space-y-4">
            <FormField label="Tiêu đề" required error={errors.title?.message}>
                <input
                    {...register('title')}
                    className={`input ${errors.title ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Nhắc lúc" required error={errors.remindAt?.message}>
                <input {...register('remindAt')} type="datetime-local" className="input" />
            </FormField>

            <FormField label="Trạng thái">
                <select {...register('status')} className="input">
                    {REMINDER_STATUS_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Mô tả">
                <textarea {...register('description')} rows={3} className="input" />
            </FormField>
        </form>
    );
}

export default ReminderFormFields;
