'use client';

import { InputHTMLAttributes } from 'react';
import clsx from 'clsx';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    error?: string;
}

export function Input({ label, error, className, ...props }: InputProps) {
    return (
        <div className="flex flex-col gap-1">
            {label && <label className="font-medium text-foreground">{label}</label>}

            <input
                {...props}
                className={clsx(
                    'border border-border px-3 py-2 rounded-md outline-none text-sm',
                    'bg-surface-alt text-foreground placeholder:text-muted',
                    'focus:ring-2 focus:ring-primary focus:border-primary transition',
                    'disabled:opacity-50 disabled:cursor-not-allowed',
                    error && 'border-danger focus:ring-danger',
                    className,
                )}
            />

            {error && <span className="text-xs text-danger">{error}</span>}
        </div>
    );
}
