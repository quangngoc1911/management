'use client'
import { QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { queryClient } from '@/shared/lib/queryClient'
import { ThemeProvider } from 'next-themes';
import { AuthProvider } from '@/shared/lib/store/authStore';
import { I18nProvider } from '@/shared/i18n/I18nProvider';
import { ToastProvider } from '@/shared/hooks/useToast';

export function Providers({ children }: { children: React.ReactNode }) {
    return (
        <ThemeProvider attribute="class" defaultTheme="light">
            <I18nProvider>
                <QueryClientProvider client={queryClient}>
                    <AuthProvider>
                        <ToastProvider>
                            {children}
                            <ReactQueryDevtools initialIsOpen={false} />
                        </ToastProvider>
                    </AuthProvider>
                </QueryClientProvider>
            </I18nProvider>
        </ThemeProvider>
    );
}