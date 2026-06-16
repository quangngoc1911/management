'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { STUDY_SCHEDULE_STATUS_OPTIONS } from '@/shared/lib/services/studyScheduleService';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import type { StudyScheduleFormValues } from '../utils/studyScheduleForm';

interface StudyScheduleFormFieldsProps {
    register: UseFormRegister<StudyScheduleFormValues>;
    errors: FieldErrors<StudyScheduleFormValues>;
    members?: FamilyMember[];
}

export function StudyScheduleFormFields({
    register,
    errors,
    members = [],
}: StudyScheduleFormFieldsProps) {
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

            <FormField label="Môn học">
                <input {...register('subject')} className="input" />
            </FormField>

            <FormField label="Tiêu đề" required error={errors.title?.message} full>
                <input
                    {...register('title')}
                    className={`input ${errors.title ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Bắt đầu" required>
                <input {...register('startTime')} type="datetime-local" className="input" />
            </FormField>

            <FormField label="Kết thúc" required error={errors.endTime?.message}>
                <input
                    {...register('endTime')}
                    type="datetime-local"
                    className={`input ${errors.endTime ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Giáo viên">
                <input {...register('teacherName')} className="input" />
            </FormField>

            <FormField label="Trạng thái">
                <select {...register('status')} className="input">
                    {STUDY_SCHEDULE_STATUS_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Địa điểm">
                <input {...register('location')} className="input" />
            </FormField>

            <FormField label="Link học online">
                <input {...register('meetingUrl')} className="input" />
            </FormField>

            <div className="md:col-span-2 flex items-center gap-2">
                <input
                    id="isOnline"
                    type="checkbox"
                    {...register('isOnline')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="isOnline" className="text-sm font-medium text-foreground">
                    Học trực tuyến
                </label>
            </div>
        </form>
    );
}

export default StudyScheduleFormFields;
