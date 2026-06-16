'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { PERIOD_TYPE_OPTIONS } from '@/shared/lib/services/budgetService';
import type { BudgetFormValues } from '../utils/budgetForm';

interface BudgetFormFieldsProps {
    register: UseFormRegister<BudgetFormValues>;
    errors: FieldErrors<BudgetFormValues>;
}

export function BudgetFormFields({ register, errors }: BudgetFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Tên" required error={errors.name?.message} full>
                <input {...register('name')} className={`input ${errors.name ? 'input-error' : ''}`} />
            </FormField>

            <FormField label="Hạn mức" required error={errors.amount?.message}>
                <input
                    {...register('amount')}
                    type="number"
                    step="0.01"
                    className={`input ${errors.amount ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Tiền tệ">
                <input {...register('currency')} className="input" />
            </FormField>

            <FormField label="Kỳ" required>
                <select {...register('periodType')} className="input">
                    {PERIOD_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Ngưỡng cảnh báo (%)">
                <input {...register('alertThreshold')} type="number" step="0.1" className="input" />
            </FormField>

            <FormField label="Bắt đầu" required>
                <input {...register('startDate')} type="date" className="input" />
            </FormField>

            <FormField label="Kết thúc" required error={errors.endDate?.message}>
                <input
                    {...register('endDate')}
                    type="date"
                    className={`input ${errors.endDate ? 'input-error' : ''}`}
                />
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

export default BudgetFormFields;
