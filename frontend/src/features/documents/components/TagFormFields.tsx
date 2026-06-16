'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { TAG_COLOR_OPTIONS } from '../utils/tagColumns';
import type { TagFormValues } from '../utils/tagForm';

interface TagFormFieldsProps {
    register: UseFormRegister<TagFormValues>;
    errors: FieldErrors<TagFormValues>;
}

export function TagFormFields({ register, errors }: TagFormFieldsProps) {
    return (
        <form className="space-y-4">
            <FormField label="Tên tag" required error={errors.name?.message}>
                <input
                    {...register('name')}
                    className={`input ${errors.name ? 'input-error' : ''}`}
                    placeholder="Nhập tên tag"
                />
            </FormField>

            <FormField label="Slug (URL)">
                <input {...register('slug')} className="input" placeholder="ten-tag-url (tùy chọn)" />
            </FormField>

            <FormField label="Màu sắc">
                <select {...register('color')} className="input">
                    {TAG_COLOR_OPTIONS.map((opt) => (
                        <option key={opt.value} value={opt.value}>
                            {opt.label}
                        </option>
                    ))}
                </select>
            </FormField>
        </form>
    );
}

export default TagFormFields;
