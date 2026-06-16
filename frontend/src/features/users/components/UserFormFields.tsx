'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { useTranslation } from '@/shared/i18n/useTranslation';
import { ROLE_VALUES } from '../types/user';
import type { UserFormValues } from '../utils/userForm';

interface UserFormFieldsProps {
    register: UseFormRegister<UserFormValues>;
    errors: FieldErrors<UserFormValues>;
    /** Hides the password field and disables email in edit mode. */
    isEdit: boolean;
}

export function UserFormFields({ register, errors, isEdit }: UserFormFieldsProps) {
    const { t } = useTranslation('users');

    return (
        <form className="space-y-4">
            <FormField label={t('form.fullName')} error={errors.fullName?.message}>
                <input
                    {...register('fullName')}
                    className={`input ${errors.fullName ? 'input-error' : ''}`}
                    placeholder={t('form.fullNamePlaceholder')}
                />
            </FormField>

            <FormField label={t('form.email')} error={errors.email?.message}>
                <input
                    {...register('email')}
                    type="email"
                    className={`input ${errors.email ? 'input-error' : ''}`}
                    placeholder={t('form.emailPlaceholder')}
                    disabled={isEdit}
                />
            </FormField>

            {!isEdit && (
                <FormField label={t('form.password')} error={errors.password?.message}>
                    <input
                        {...register('password')}
                        type="password"
                        className={`input ${errors.password ? 'input-error' : ''}`}
                        placeholder={t('form.passwordPlaceholder')}
                    />
                </FormField>
            )}

            <FormField label={t('form.role')} error={errors.role?.message}>
                <select {...register('role')} className={`input ${errors.role ? 'input-error' : ''}`}>
                    <option value="">{t('form.rolePlaceholder')}</option>
                    {ROLE_VALUES.map((value) => (
                        <option key={value} value={value}>
                            {t(`roles.${value}`)}
                        </option>
                    ))}
                </select>
            </FormField>
        </form>
    );
}

export default UserFormFields;
