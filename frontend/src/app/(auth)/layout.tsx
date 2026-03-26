// src/app/(dashboard)/layout.tsx
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { menuServerApi } from '@/shared/api/menu.server';
import Sidebar from '../(dashboard)/components/Sidebar';

export default async function DashboardLayout({ children }: { children: React.ReactNode }) {
    // Kiểm tra token — nếu chưa đăng nhập thì redirect
    // Dùng middleware thay thế nếu cần bảo vệ nhiều route
    const menus = await menuServerApi.getTree();

    return (
        <div className="flex h-screen">
            <Sidebar menus={menus} />
            <main className="flex-1 overflow-auto p-6">{children}</main>
        </div>
    );
}
