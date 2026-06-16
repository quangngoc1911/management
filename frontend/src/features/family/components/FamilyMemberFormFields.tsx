'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { useTranslation } from '@/shared/i18n/useTranslation';
import { GENDER_OPTIONS, RELATION_TO_HEAD_OPTIONS } from '../utils/options';
import type { MemberFormValues } from '../utils/familyForm';

interface FamilyMemberFormFieldsProps {
    register: UseFormRegister<MemberFormValues>;
    errors: FieldErrors<MemberFormValues>;
}

export function FamilyMemberFormFields({ register, errors }: FamilyMemberFormFieldsProps) {
    const { t } = useTranslation('family');

    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label={t('form.fullName')} required error={errors.fullName?.message} full>
                <input
                    {...register('fullName')}
                    className={`input ${errors.fullName ? 'input-error' : ''}`}
                    placeholder={t('form.fullNamePlaceholder')}
                />
            </FormField>

            <FormField label={t('form.nickname')} error={errors.nickname?.message}>
                <input
                    {...register('nickname')}
                    className={`input ${errors.nickname ? 'input-error' : ''}`}
                    placeholder={t('form.nicknamePlaceholder')}
                />
            </FormField>

            <FormField label={t('form.dateOfBirth')}>
                <input type="date" {...register('dateOfBirth')} className="input" />
            </FormField>

            <FormField label={t('form.gender')}>
                <select {...register('gender')} className="input">
                    <option value="">{t('form.select')}</option>
                    {GENDER_OPTIONS.map((value) => (
                        <option key={value} value={value}>
                            {t(`gender.${value}`)}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label={t('form.relationToHead')}>
                <select {...register('relationToHead')} className="input">
                    <option value="">{t('form.select')}</option>
                    {RELATION_TO_HEAD_OPTIONS.map((value) => (
                        <option key={value} value={value}>
                            {t(`relationToHead.${value}`)}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label={t('form.phone')} error={errors.phone?.message}>
                <input
                    {...register('phone')}
                    className={`input ${errors.phone ? 'input-error' : ''}`}
                    placeholder={t('form.phonePlaceholder')}
                />
            </FormField>

            <FormField label={t('form.email')} error={errors.email?.message}>
                <input
                    {...register('email')}
                    type="email"
                    className={`input ${errors.email ? 'input-error' : ''}`}
                    placeholder={t('form.emailPlaceholder')}
                />
            </FormField>

            <div className="md:col-span-2 flex items-center gap-2">
                <input
                    id="isHouseholdHead"
                    type="checkbox"
                    {...register('isHouseholdHead')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="isHouseholdHead" className="text-sm font-medium text-foreground">
                    {t('form.isHouseholdHead')}
                </label>
            </div>

            <FormField label={t('form.notes')} full>
                <textarea
                    {...register('notes')}
                    rows={3}
                    className="input"
                    placeholder={t('form.notesPlaceholder')}
                />
            </FormField>
        </form>
    );
}

export default FamilyMemberFormFields;
