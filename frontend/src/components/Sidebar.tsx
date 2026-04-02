'use client';

import { ThemeToggle } from '@/components/ThemeToggle';
import { useAuth } from '@/features/auth/hooks/useAuth';
import { Menu } from '@/features/menu/types/menu';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

export default function Sidebar({ menus }: { menus: Menu[] }) {
    const pathname = usePathname();
    const { logout } = useAuth();

    return (
        <aside className="w-64 bg-sidebar flex flex-col h-screen shrink-0">
            {/* Header */}
            <div
                className="flex items-center justify-between px-4 py-5
                            border-b border-sidebar-border"
            >
                <span className="text-white font-bold text-base">My System</span>
                <ThemeToggle />
            </div>

            {/* Nav */}
            <nav className="flex-1 px-3 py-4 space-y-1 overflow-y-auto">
                {menus
                    .filter((m) => m.isVisible)
                    .sort((a, b) => a.order - b.order)
                    .map((menu) => (
                        <MenuItem key={menu.id} menu={menu} pathname={pathname} />
                    ))}
            </nav>

            {/* Footer — đăng xuất */}
            <div className="px-3 py-4 border-t border-sidebar-border">
                <button
                    onClick={logout}
                    className="w-full flex items-center gap-2 px-3 py-2 rounded-md text-sm
                               text-danger hover:bg-sidebar-border transition text-left"
                >
                    Đăng xuất
                </button>
            </div>
        </aside>
    );
}

// Menu item — hỗ trợ cả menu cha và menu con
function MenuItem({ menu, pathname }: { menu: Menu; pathname: string }) {
    const isActive = pathname === menu.path;
    const hasChildren = menu.children?.length > 0;

    return (
        <div>
            <Link
                href={menu.path || '#'}
                className={[
                    'flex items-center gap-3 px-3 py-2 rounded-md text-sm transition',
                    isActive
                        ? 'bg-primary text-white'
                        : 'text-sidebar-muted hover:bg-sidebar-border hover:text-white',
                ].join(' ')}
            >
                {menu.icon && <span className="text-base">{menu.icon}</span>}
                <span>{menu.name}</span>
            </Link>

            {/* Menu con */}
            {hasChildren && (
                <div className="ml-3 mt-1 pl-3 border-l border-sidebar-border space-y-1">
                    {menu.children
                        .filter((c) => c.isVisible)
                        .sort((a, b) => a.order - b.order)
                        .map((child) => (
                            <Link
                                key={child.id}
                                href={child.path || '#'}
                                className={[
                                    'block px-3 py-2 rounded-md text-xs transition',
                                    pathname === child.path
                                        ? 'bg-primary text-white'
                                        : 'text-sidebar-muted hover:bg-sidebar-border hover:text-white',
                                ].join(' ')}
                            >
                                {child.name}
                            </Link>
                        ))}
                </div>
            )}
        </div>
    );
}
