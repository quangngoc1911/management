'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import type { DocumentFormValues } from '../utils/documentForm';
import type { Category } from '@/shared/lib/services/categoryService';

interface DocumentFormFieldsProps {
    register: UseFormRegister<DocumentFormValues>;
    errors: FieldErrors<DocumentFormValues>;
    /** Available categories for the dropdown. */
    categories: Category[];
    /** Whether the form is in edit mode (shows status selector). */
    isEdit: boolean;
}

export function DocumentFormFields({ register, errors, categories, isEdit }: DocumentFormFieldsProps) {
    return (
        <form className="space-y-4">
            <FormField label="Tiêu đề" required error={errors.title?.message}>
                <input
                    {...register('title')}
                    className={`input ${errors.title ? 'input-error' : ''}`}
                    placeholder="Nhập tiêu đề"
                />
            </FormField>

            <FormField label="Danh mục" required error={errors.categoryId?.message}>
                <select
                    {...register('categoryId')}
                    className={`input ${errors.categoryId ? 'input-error' : ''}`}
                >
                    <option value="">Chọn danh mục</option>
                    {categories.map((cat) => (
                        <option key={cat.id} value={cat.id}>
                            {cat.name}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Nội dung" required error={errors.content?.message}>
                <textarea
                    {...register('content')}
                    className={`input resize-none h-48 ${errors.content ? 'input-error' : ''}`}
                    placeholder="Nhập nội dung tài liệu"
                />
            </FormField>

            {isEdit && (
                <FormField label="Trạng thái">
                    <select {...register('status')} className="input">
                        <option value="draft">Bản nháp</option>
                        <option value="published">Đã xuất bản</option>
                        <option value="archived">Lưu trữ</option>
                    </select>
                </FormField>
            )}
        </form>
    );
}

export default DocumentFormFields;
