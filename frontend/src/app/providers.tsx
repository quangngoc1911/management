'use client'
import { QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { queryClient } from '@/shared/lib/queryClient'
import { ThemeProvider } from 'next-themes';
import { AuthProvider } from '@/shared/lib/store/authStore';

export function Providers({ children }: { children: React.ReactNode }) {
    return (
        <ThemeProvider attribute="class" defaultTheme="light">
            <QueryClientProvider client={queryClient}>
                <AuthProvider>
                    {children}
                    <ReactQueryDevtools initialIsOpen={false} />
                </AuthProvider>
            </QueryClientProvider>
        </ThemeProvider>
    );
}