'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { BookmarkFormValues } from '../utils/bookmarkForm';
import { ENTITY_TYPE_OPTIONS } from '../utils/bookmarkForm';

interface BookmarkFormFieldsProps {
    register: UseFormRegister<BookmarkFormValues>;
    errors: FieldErrors<BookmarkFormValues>;
    isEdit: boolean;
}

export function BookmarkFormFields({ register, errors, isEdit }: BookmarkFormFieldsProps) {
    return (
        <form className="space-y-4">
            <FormField label="Loại đối tượng" required>
                <select {...register('entityType')} disabled={isEdit} className="input">
                    {ENTITY_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="ID đối tượng" required error={errors.entityId?.message}>
                <input
                    {...register('entityId')}
                    disabled={isEdit}
                    className={`input ${errors.entityId ? 'input-error' : ''}`}
                    placeholder="GUID của đối tượng"
                />
            </FormField>

            <FormField label="Ghi chú">
                <textarea {...register('note')} rows={3} className="input" />
            </FormField>
        </form>
    );
}

export default BookmarkFormFields;
