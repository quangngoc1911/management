import type { Metadata, Viewport } from 'next';
import '@/app/styles/globals.css';
import { Providers } from './providers';

export const metadata: Metadata = {
    title: 'Hệ thống Quản lý Tài liệu',
    description: 'Document Retrieval System',
};

export const viewport: Viewport = {
    width: 'device-width',
    initialScale: 1,
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
    return (
        <html lang="vi" suppressHydrationWarning>
            <body className="bg-background text-foreground min-h-screen">
                <Providers>{children}</Providers>
            </body>
        </html>
    );
}
