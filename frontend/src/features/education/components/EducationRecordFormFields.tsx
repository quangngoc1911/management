'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import {
    EDUCATION_LEVEL_OPTIONS,
    EDUCATION_STATUS_OPTIONS,
} from '@/shared/lib/services/educationRecordService';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import type { EducationRecordFormValues } from '../utils/educationRecordForm';

interface EducationRecordFormFieldsProps {
    register: UseFormRegister<EducationRecordFormValues>;
    errors: FieldErrors<EducationRecordFormValues>;
    members?: FamilyMember[];
}

export function EducationRecordFormFields({
    register,
    errors,
    members = [],
}: EducationRecordFormFieldsProps) {
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

            <FormField label="Cấp học" required>
                <select {...register('level')} className="input">
                    {EDUCATION_LEVEL_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField
                label="Cơ sở đào tạo"
                required
                error={errors.institutionName?.message}
                full
            >
                <input
                    {...register('institutionName')}
                    className={`input ${errors.institutionName ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Chuyên ngành">
                <input {...register('major')} className="input" />
            </FormField>

            <FormField label="Bằng cấp">
                <input {...register('degree')} className="input" />
            </FormField>

            <FormField label="Mã sinh viên">
                <input {...register('studentId')} className="input" />
            </FormField>

            <FormField label="GPA">
                <input {...register('gpa')} type="number" step="0.01" className="input" />
            </FormField>

            <FormField label="Bắt đầu">
                <input {...register('startDate')} type="date" className="input" />
            </FormField>

            <FormField label="Kết thúc">
                <input {...register('endDate')} type="date" className="input" />
            </FormField>

            <FormField label="Trạng thái">
                <select {...register('status')} className="input">
                    {EDUCATION_STATUS_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Thành tích" full>
                <textarea {...register('achievements')} rows={2} className="input" />
            </FormField>

            <FormField label="Ghi chú" full>
                <textarea {...register('notes')} rows={2} className="input" />
            </FormField>
        </form>
    );
}

export default EducationRecordFormFields;
