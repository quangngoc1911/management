'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { Asset } from '@/shared/lib/services/assetService';
import type { AssetValuationFormValues } from '../utils/assetValuationForm';

interface AssetValuationFormFieldsProps {
    register: UseFormRegister<AssetValuationFormValues>;
    errors: FieldErrors<AssetValuationFormValues>;
    assets: Asset[];
    isEdit: boolean;
}

export function AssetValuationFormFields({
    register,
    errors,
    assets,
    isEdit,
}: AssetValuationFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Tài sản" required error={errors.assetId?.message} full>
                <select
                    {...register('assetId')}
                    disabled={isEdit}
                    className={`input ${errors.assetId ? 'input-error' : ''}`}
                >
                    <option value="">-- Chọn --</option>
                    {assets.map((a) => (
                        <option key={a.id} value={a.id}>
                            {a.name}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Giá trị" required error={errors.value?.message}>
                <input
                    {...register('value')}
                    type="number"
                    step="0.01"
                    className={`input ${errors.value ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Tiền tệ" error={errors.currency?.message}>
                <input {...register('currency')} className="input" />
            </FormField>

            <FormField label="Ngày định giá" required error={errors.valuationDate?.message}>
                <input {...register('valuationDate')} type="date" className="input" />
            </FormField>

            <FormField label="Phương pháp">
                <input
                    {...register('valuationMethod')}
                    className="input"
                    placeholder="Thị trường, thẩm định..."
                />
            </FormField>

            <FormField label="Ghi chú" full>
                <textarea {...register('notes')} rows={2} className="input" />
            </FormField>
        </form>
    );
}

export default AssetValuationFormFields;
