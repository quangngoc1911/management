'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { SystemConfigFormValues } from '../utils/systemConfigForm';

interface SystemConfigFormFieldsProps {
    register: UseFormRegister<SystemConfigFormValues>;
    errors: FieldErrors<SystemConfigFormValues>;
    /** When true the `key` field is read-only (update mode). */
    isEdit?: boolean;
}

export function SystemConfigFormFields({ register, errors, isEdit = false }: SystemConfigFormFieldsProps) {
    return (
        <form className="space-y-4">
            <FormField label="Khoá" required error={errors.key?.message}>
                <input
                    {...register('key')}
                    disabled={isEdit}
                    className={`input ${errors.key ? 'input-error' : ''} ${isEdit ? 'opacity-60 cursor-not-allowed' : ''}`}
                    placeholder="vd: app.theme"
                />
            </FormField>

            <FormField label="Giá trị (JSON)" required error={errors.value?.message}>
                <textarea
                    {...register('value')}
                    rows={4}
                    className={`input font-mono text-sm ${errors.value ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Mô tả" error={errors.description?.message}>
                <input {...register('description')} className="input" />
            </FormField>

            <div className="flex items-center gap-6">
                <label className="flex items-center gap-2 text-sm font-medium text-foreground">
                    <input type="checkbox" {...register('isPublic')} className="w-4 h-4 rounded border-border" />
                    Công khai
                </label>
                <label className="flex items-center gap-2 text-sm font-medium text-foreground">
                    <input type="checkbox" {...register('isEncrypted')} className="w-4 h-4 rounded border-border" />
                    Mã hoá
                </label>
            </div>
        </form>
    );
}

export default SystemConfigFormFields;
