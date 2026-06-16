'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { FREQUENCY_OPTIONS, RECURRING_TYPE_OPTIONS } from '@/shared/lib/services/recurringTransactionService';
import type { Account } from '@/shared/lib/services/accountService';
import type { RecurringTransactionFormValues } from '../utils/recurringTransactionForm';

interface RecurringTransactionFormFieldsProps {
    register: UseFormRegister<RecurringTransactionFormValues>;
    errors: FieldErrors<RecurringTransactionFormValues>;
    accounts: Account[];
}

export function RecurringTransactionFormFields({
    register,
    errors,
    accounts,
}: RecurringTransactionFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Tài khoản" required error={errors.accountId?.message}>
                <select {...register('accountId')} className={`input ${errors.accountId ? 'input-error' : ''}`}>
                    <option value="">-- Chọn --</option>
                    {accounts.map((a) => (
                        <option key={a.id} value={a.id}>
                            {a.name}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Loại" required>
                <select {...register('type')} className="input">
                    {RECURRING_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Tên" required error={errors.name?.message} full>
                <input {...register('name')} className={`input ${errors.name ? 'input-error' : ''}`} />
            </FormField>

            <FormField label="Số tiền" required error={errors.amount?.message}>
                <input
                    {...register('amount')}
                    type="number"
                    step="0.01"
                    className={`input ${errors.amount ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Tần suất" required>
                <select {...register('frequency')} className="input">
                    {FREQUENCY_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Bắt đầu" required>
                <input {...register('startDate')} type="date" className="input" />
            </FormField>

            <FormField label="Kỳ kế tiếp">
                <input {...register('nextDueDate')} type="date" className="input" />
            </FormField>

            <FormField label="Kết thúc">
                <input {...register('endDate')} type="date" className="input" />
            </FormField>

            <div className="flex items-center gap-2 md:mt-8">
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

export default RecurringTransactionFormFields;
