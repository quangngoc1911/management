'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { ASSET_TYPE_OPTIONS, ASSET_STATUS_OPTIONS } from '@/shared/lib/services/assetService';
import type { AssetFormValues } from '../utils/assetForm';

interface AssetFormFieldsProps {
    register: UseFormRegister<AssetFormValues>;
    errors: FieldErrors<AssetFormValues>;
}

export function AssetFormFields({ register, errors }: AssetFormFieldsProps) {
    return (
        <form className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField label="Tên tài sản" required error={errors.name?.message} full>
                <input
                    {...register('name')}
                    className={`input ${errors.name ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Loại" required error={errors.assetType?.message}>
                <select {...register('assetType')} className="input">
                    {ASSET_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Trạng thái">
                <select {...register('status')} className="input">
                    {ASSET_STATUS_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Giá mua">
                <input {...register('purchasePrice')} type="number" step="0.01" className="input" />
            </FormField>

            <FormField label="Tiền tệ">
                <input {...register('currency')} className="input" />
            </FormField>

            <FormField label="Ngày mua">
                <input {...register('purchaseDate')} type="date" className="input" />
            </FormField>

            <FormField label="Số serial">
                <input {...register('serialNumber')} className="input" />
            </FormField>

            <FormField label="Vị trí" full>
                <input {...register('location')} className="input" />
            </FormField>

            <FormField label="Mô tả" full>
                <textarea {...register('description')} rows={2} className="input" />
            </FormField>

            <div className="flex items-center gap-2">
                <input
                    id="isInsured"
                    type="checkbox"
                    {...register('isInsured')}
                    className="w-4 h-4 rounded border-border"
                />
                <label htmlFor="isInsured" className="text-sm font-medium text-foreground">
                    Có bảo hiểm
                </label>
            </div>

            <FormField label="Thông tin bảo hiểm">
                <input {...register('insuranceInfo')} className="input" />
            </FormField>
        </form>
    );
}

export default AssetFormFields;
