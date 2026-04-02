import type { Metadata } from 'next'
import '@/app/styles/globals.css';
import { Providers } from './providers'

export const metadata: Metadata = {
  title: 'My App',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
      <html lang="vi" suppressHydrationWarning>
          <body className="bg-background text-foreground min-h-screen">
              <Providers>
                  <div className="p-6">{children}</div>
              </Providers>
          </body>
      </html>
  );
}