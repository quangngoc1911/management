'use client';

import { createContext, useCallback, useContext, useMemo, useState } from 'react';
import { CheckCircle2, AlertCircle, Info, X } from 'lucide-react';

type ToastType = 'success' | 'error' | 'info';

interface Toast {
    id: number;
    type: ToastType;
    message: string;
}

interface ToastContextValue {
    success: (message: string) => void;
    error: (message: string) => void;
    info: (message: string) => void;
}

const ToastContext = createContext<ToastContextValue | undefined>(undefined);

const TOAST_DURATION = 3500;

const toastStyles: Record<ToastType, { icon: typeof Info; className: string }> = {
    success: { icon: CheckCircle2, className: 'border-success/40 text-success' },
    error: { icon: AlertCircle, className: 'border-danger/40 text-danger' },
    info: { icon: Info, className: 'border-primary/40 text-primary' },
};

export function ToastProvider({ children }: { children: React.ReactNode }) {
    const [toasts, setToasts] = useState<Toast[]>([]);

    const dismiss = useCallback((id: number) => {
        setToasts((prev) => prev.filter((t) => t.id !== id));
    }, []);

    const push = useCallback(
        (type: ToastType, message: string) => {
            const id = Date.now() + Math.random();
            setToasts((prev) => [...prev, { id, type, message }]);
            setTimeout(() => dismiss(id), TOAST_DURATION);
        },
        [dismiss],
    );

    const value = useMemo<ToastContextValue>(
        () => ({
            success: (m) => push('success', m),
            error: (m) => push('error', m),
            info: (m) => push('info', m),
        }),
        [push],
    );

    return (
        <ToastContext.Provider value={value}>
            {children}
            <div className="fixed bottom-4 right-4 z-modal flex flex-col gap-2 w-80 max-w-[calc(100vw-2rem)]">
                {toasts.map((toast) => {
                    const { icon: Icon, className } = toastStyles[toast.type];
                    return (
                        <div
                            key={toast.id}
                            role="status"
                            className={`flex items-start gap-3 p-3 rounded-md bg-surface shadow-modal border ${className} animate-scale-in`}
                        >
                            <Icon className="w-5 h-5 shrink-0 mt-0.5" />
                            <span className="flex-1 text-sm text-foreground">{toast.message}</span>
                            <button
                                onClick={() => dismiss(toast.id)}
                                className="text-muted hover:text-foreground transition"
                                aria-label="close"
                            >
                                <X className="w-4 h-4" />
                            </button>
                        </div>
                    );
                })}
            </div>
        </ToastContext.Provider>
    );
}

export function useToast(): ToastContextValue {
    const ctx = useContext(ToastContext);
    if (!ctx) {
        throw new Error('useToast must be used within a ToastProvider');
    }
    return ctx;
}
