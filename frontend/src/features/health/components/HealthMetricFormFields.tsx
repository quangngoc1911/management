'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import { METRIC_TYPE_OPTIONS } from '@/shared/lib/services/healthMetricService';
import type { HealthMetricFormValues } from '../utils/healthMetricForm';

interface HealthMetricFormFieldsProps {
    register: UseFormRegister<HealthMetricFormValues>;
    errors: FieldErrors<HealthMetricFormValues>;
    members?: FamilyMember[];
}

export function HealthMetricFormFields({
    register,
    errors,
    members = [],
}: HealthMetricFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Thành viên" required error={errors.memberId?.message}>
                <select
                    {...register('memberId')}
                    className={`input ${errors.memberId ? 'input-error' : ''}`}
                >
                    <option value="">-- Chọn --</option>
                    {members.map((m) => (
                        <option key={m.id} value={m.id}>
                            {m.fullName}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Loại chỉ số" required error={errors.metricType?.message}>
                <select {...register('metricType')} className="input">
                    {METRIC_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Giá trị" required error={errors.value?.message}>
                <input
                    {...register('value')}
                    type="number"
                    step="0.01"
                    className={`input ${errors.value ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Đơn vị" required error={errors.unit?.message}>
                <input
                    {...register('unit')}
                    className={`input ${errors.unit ? 'input-error' : ''}`}
                    placeholder="kg, mmHg, bpm..."
                />
            </FormField>

            <FormField label="Thời điểm đo" required error={errors.measuredAt?.message}>
                <input {...register('measuredAt')} type="datetime-local" className="input" />
            </FormField>

            <FormField label="Ghi chú" full>
                <textarea {...register('notes')} rows={2} className="input" />
            </FormField>
        </form>
    );
}

export default HealthMetricFormFields;
