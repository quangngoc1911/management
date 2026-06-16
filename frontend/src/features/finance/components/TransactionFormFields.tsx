'use client';

import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { FormField } from '@/components/FormField';
import { TRANSACTION_TYPE_OPTIONS, TRANSACTION_STATUS_OPTIONS } from '@/shared/lib/services/transactionService';
import type { Account } from '@/shared/lib/services/accountService';
import type { Category } from '@/shared/lib/services/categoryService';
import type { TransactionFormValues } from '../utils/transactionForm';

interface TransactionFormFieldsProps {
    register: UseFormRegister<TransactionFormValues>;
    errors: FieldErrors<TransactionFormValues>;
    accounts: Account[];
    categories: Category[];
}

export function TransactionFormFields({ register, errors, accounts, categories }: TransactionFormFieldsProps) {
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

            <FormField label="Loại" required error={errors.type?.message}>
                <select {...register('type')} className={`input ${errors.type ? 'input-error' : ''}`}>
                    {TRANSACTION_TYPE_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Số tiền" required error={errors.amount?.message}>
                <input
                    {...register('amount')}
                    type="number"
                    step="0.01"
                    className={`input ${errors.amount ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Đơn vị tiền tệ" error={errors.currency?.message}>
                <input {...register('currency')} className={`input ${errors.currency ? 'input-error' : ''}`} />
            </FormField>

            <FormField label="Ngày giao dịch" required error={errors.transactionDate?.message}>
                <input
                    {...register('transactionDate')}
                    type="date"
                    className={`input ${errors.transactionDate ? 'input-error' : ''}`}
                />
            </FormField>

            <FormField label="Trạng thái">
                <select {...register('status')} className="input">
                    {TRANSACTION_STATUS_OPTIONS.map((o) => (
                        <option key={o.value} value={o.value}>
                            {o.label}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Danh mục" full>
                <select {...register('categoryId')} className="input">
                    <option value="">-- Không --</option>
                    {categories.map((c) => (
                        <option key={c.id} value={c.id}>
                            {c.name}
                        </option>
                    ))}
                </select>
            </FormField>

            <FormField label="Mô tả" full>
                <input {...register('description')} className="input" />
            </FormField>

            <FormField label="Ghi chú" full>
                <textarea {...register('note')} rows={2} className="input" />
            </FormField>
        </form>
    );
}

export default TransactionFormFields;
