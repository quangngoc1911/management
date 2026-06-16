'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { ACCOUNT_TYPE_OPTIONS } from '@/shared/lib/services/accountService';
import type { AccountFormValues } from '../utils/accountForm';

interface AccountFormFieldsProps {
    register: UseFormRegister<AccountFormValues>;
    errors: FieldErrors<AccountFormValues>;
}

export function AccountFormFields({ register, errors }: AccountFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Tên tài khoản" required error={errors.name?.message} full>
                <input {...register('name')} className={`input ${errors.name ? 'input-error' : ''}`} />
            </FormField>

            <FormField label="Loại tài khoản" required error={errors.accountType?.message}>
                <select {...register('accountType')} className={`input ${errors.accountType ? 'input-error' : ''}`}>
                    <option value="">-- Chọn --</option>
                    {ACCOUNT_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Đơn vị tiền tệ" error={errors.currency?.message}>
                <input {...register('currency')} className={`input ${errors.currency ? 'input-error' : ''}`} />
            </FormField>

            <FormField label="Ngân hàng">
                <input {...register('bankName')} className="input" />
            </FormField>

            <FormField label="Số tài khoản">
                <input {...register('accountNumber')} className="input" placeholder="Được mã hoá khi lưu" />
            </FormField>

            <div className="md:col-span-2 flex items-center gap-2">
                <input
                    id="isActive"
                    type="checkbox"
                    {...register('isActive')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="isActive" className="text-sm font-medium text-foreground">
                    Đang hoạt động
                </label>
            </div>
        </form>
    );
}

export default AccountFormFields;
