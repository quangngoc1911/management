'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { CategoryFormValues } from '../utils/categoryForm';
import type { Category } from '@/shared/lib/services/categoryService';

interface CategoryFormFieldsProps {
    register: UseFormRegister<CategoryFormValues>;
    errors: FieldErrors<CategoryFormValues>;
    /** All categories — passed in for the parent selector shown in edit mode. */
    categories: Category[];
    /** Whether the form is in edit mode (shows parent-category selector). */
    isEdit: boolean;
}

export function CategoryFormFields({ register, errors, categories, isEdit }: CategoryFormFieldsProps) {
    return (
        <form className="space-y-4">
            <FormField label="Tên danh mục" required error={errors.name?.message}>
                <input
                    {...register('name')}
                    className={`input ${errors.name ? 'input-error' : ''}`}
                    placeholder="Nhập tên danh mục"
                />
            </FormField>

            <FormField label="Slug (URL)">
                <input {...register('slug')} className="input" placeholder="duong-dan-url (tùy chọn)" />
            </FormField>

            <FormField label="Mô tả">
                <textarea
                    {...register('description')}
                    className="input resize-none h-24"
                    placeholder="Nhập mô tả (tùy chọn)"
                />
            </FormField>

            {isEdit && (
                <FormField label="Danh mục cha">
                    <select {...register('parentId')} className="input">
                        <option value="">-- Không có --</option>
                        {categories.map((cat) => (
                            <option key={cat.id} value={cat.id}>
                                {cat.name}
                            </option>
                        ))}
                    </select>
                </FormField>
            )}
        </form>
    );
}

export default CategoryFormFields;
