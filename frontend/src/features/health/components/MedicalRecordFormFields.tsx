'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import { RECORD_TYPE_OPTIONS } from '@/shared/lib/services/medicalRecordService';
import type { MedicalRecordFormValues } from '../utils/medicalRecordForm';

interface MedicalRecordFormFieldsProps {
    register: UseFormRegister<MedicalRecordFormValues>;
    errors: FieldErrors<MedicalRecordFormValues>;
    members?: FamilyMember[];
}

export function MedicalRecordFormFields({
    register,
    errors,
    members = [],
}: MedicalRecordFormFieldsProps) {
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

            <FormField label="Loại hồ sơ" required error={errors.recordType?.message}>
                <select {...register('recordType')} className="input">
                    {RECORD_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Tiêu đề" required error={errors.title?.message} full>
                <input
                    {...register('title')}
                    className={`input ${errors.title ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Ngày khám" required error={errors.recordDate?.message}>
                <input {...register('recordDate')} type="date" className="input" />
            </FormField>

            <FormField label="Ngày tái khám">
                <input {...register('followUpDate')} type="date" className="input" />
            </FormField>

            <FormField label="Bác sĩ">
                <input {...register('doctorName')} className="input" />
            </FormField>

            <FormField label="Bệnh viện">
                <input {...register('hospitalName')} className="input" />
            </FormField>

            <FormField label="Chẩn đoán" full>
                <textarea {...register('diagnosis')} rows={2} className="input" />
            </FormField>

            <FormField label="Điều trị" full>
                <textarea {...register('treatment')} rows={2} className="input" />
            </FormField>

            <div className="md:col-span-2 flex items-center gap-2">
                <input
                    id="isPrivate"
                    type="checkbox"
                    {...register('isPrivate')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="isPrivate" className="text-sm font-medium text-foreground">
                    Hồ sơ riêng tư
                </label>
            </div>
        </form>
    );
}

export default MedicalRecordFormFields;
