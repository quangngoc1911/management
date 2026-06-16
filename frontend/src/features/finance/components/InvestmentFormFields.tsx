'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { INVESTMENT_TYPE_OPTIONS } from '@/shared/lib/services/investmentService';
import type { InvestmentFormValues } from '../utils/investmentForm';

interface InvestmentFormFieldsProps {
    register: UseFormRegister<InvestmentFormValues>;
    errors: FieldErrors<InvestmentFormValues>;
}

export function InvestmentFormFields({ register, errors }: InvestmentFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Loại" required>
                <select {...register('type')} className="input">
                    {INVESTMENT_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Mã (symbol)">
                <input {...register('symbol')} className="input" />
            </FormField>

            <FormField label="Tên" required error={errors.name?.message} full>
                <input {...register('name')} className={`input ${errors.name ? 'input-error' : ''}`} />
            </FormField>

            <FormField label="Số lượng">
                <input {...register('quantity')} type="number" step="0.0001" className="input" />
            </FormField>

            <FormField label="Giá mua">
                <input {...register('purchasePrice')} type="number" step="0.01" className="input" />
            </FormField>

            <FormField label="Giá hiện tại">
                <input {...register('currentPrice')} type="number" step="0.01" className="input" />
            </FormField>

            <div className="flex items-center gap-2 md:mt-8">
                <input
                    id="isActive"
                    type="checkbox"
                    {...register('isActive')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="isActive" className="text-sm font-medium text-foreground">
                    Đang nắm giữ
                </label>
            </div>
        </form>
    );
}

export default InvestmentFormFields;
