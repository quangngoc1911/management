'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import type { MedicationFormValues } from '../utils/medicationForm';

interface MedicationFormFieldsProps {
    register: UseFormRegister<MedicationFormValues>;
    errors: FieldErrors<MedicationFormValues>;
    members?: FamilyMember[];
}

export function MedicationFormFields({
    register,
    errors,
    members = [],
}: MedicationFormFieldsProps) {
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

            <FormField label="Tên thuốc" required error={errors.name?.message}>
                <input
                    {...register('name')}
                    className={`input ${errors.name ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Liều lượng">
                <input {...register('dosage')} className="input" placeholder="VD: 500mg" />
            </FormField>

            <FormField label="Tần suất">
                <input {...register('frequency')} className="input" placeholder="VD: 2 lần/ngày" />
            </FormField>

            <FormField label="Bắt đầu" required error={errors.startDate?.message}>
                <input {...register('startDate')} type="date" className="input" />
            </FormField>

            <FormField label="Kết thúc">
                <input {...register('endDate')} type="date" className="input" />
            </FormField>

            <FormField label="Ghi chú" full>
                <textarea {...register('notes')} rows={2} className="input" />
            </FormField>

            <div className="md:col-span-2 flex items-center gap-2">
                <input
                    id="isActive"
                    type="checkbox"
                    {...register('isActive')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="isActive" className="text-sm font-medium text-foreground">
                    Đang sử dụng
                </label>
            </div>
        </form>
    );
}

export default MedicationFormFields;
