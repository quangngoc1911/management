'use client';

import { ButtonHTMLAttributes } from 'react';
import clsx from 'clsx';

type Variant = 'primary' | 'secondary' | 'outline' | 'danger' | 'ghost';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
    variant?: Variant;
    loading?: boolean;
}

export function Button({
    children,
    variant = 'primary',
    loading = false,
    className,
    disabled,
    ...props
}: ButtonProps) {
    const base =
        'px-4 py-2 rounded-md text-sm font-medium transition disabled:opacity-50 disabled:cursor-not-allowed';

    const variants: Record<Variant, string> = {
        primary: 'bg-primary hover:bg-primary-hover text-white',

        secondary: 'bg-surface-alt hover:bg-border text-foreground border border-border',

        outline: 'border border-border text-foreground hover:bg-surface-alt',

        danger: 'bg-danger hover:bg-danger-hover text-white',

        ghost: 'text-muted hover:text-foreground hover:bg-surface-alt',
    };

    return (
        <button
            className={clsx(base, variants[variant], className)}
            disabled={disabled || loading}
            {...props}
        >
            {loading ? (
                <span className="flex items-center gap-2">
                    {/* Spinner nhỏ khi loading */}
                    <span className="w-3.5 h-3.5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                    Đang xử lý...
                </span>
            ) : (
                children
            )}
        </button>
    );
}
