// app/(dashboard)/layout.tsx
import { menuServerApi } from '@/features/menu/services/menu.server';
import Sidebar from '../../components/Sidebar';
import type { Menu } from '@/features/menu/types/menu';

export default async function DashboardLayout({ children }: { children: React.ReactNode }) {
    const menus: Menu[] = await menuServerApi.getTree().catch(() => []);

    return (
         <div className="flex h-screen overflow-hidden bg-background">
            <Sidebar menus={menus} />
            {/* overflow-auto ở main → chỉ vùng content cuộn, sidebar cố định */}
            <main className="flex-1 overflow-auto">
                <div className="page">{children}</div>
            </main>
        </div>
    );
}