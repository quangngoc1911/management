// app/(dashboard)/layout.tsx
import { menuServerApi } from '@/shared/api/menu.server';
import Sidebar from './components/Sidebar';

export default async function DashboardLayout({ children }: { children: React.ReactNode }) {
    // Fetch menu ở Server Component → nhanh, không flicker
    const menus = await menuServerApi.getTree();

    return (
        <div className="flex h-screen">
            <Sidebar menus={menus} />
            <main className="flex-1 overflow-auto p-6">{children}</main>
        </div>
    );
}
