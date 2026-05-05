'use client';

import { ReactNode } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import {
    LayoutDashboard,
    Users,
    FolderTree,
    FileText,
    Search,
    Tags,
    User,
    Menu,
    Bell,
    LogOut,
    X,
} from 'lucide-react';
import { useAuth } from '@/features/auth/hooks/useAuth';
import { useState } from 'react';

const menuItems = [
    { href: '/dashboard', label: 'Tổng quan', icon: LayoutDashboard },
    { href: '/users', label: 'Người dùng', icon: Users },
    { href: '/categories', label: 'Danh mục', icon: FolderTree },
    { href: '/documents', label: 'Tài liệu', icon: FileText },
    { href: '/documents/search', label: 'Tìm kiếm', icon: Search },
    { href: '/tags', label: 'Tags', icon: Tags },
    { href: '/profile', label: 'Hồ sơ', icon: User },
];

export default function DashboardLayout({ children }: { children: ReactNode }) {
    const pathname = usePathname();
    const { user, logout } = useAuth();
    const [sidebarOpen, setSidebarOpen] = useState(false);

    return (
        <div className="min-h-screen bg-background flex">
            {/* Mobile Sidebar Overlay */}
            {sidebarOpen && (
                <div
                    className="fixed inset-0 bg-black/50 z-overlay lg:hidden"
                    onClick={() => setSidebarOpen(false)}
                />
            )}

            {/* Sidebar */}
            <aside
                className={`
        fixed lg:static inset-y-0 left-0 z-sticky w-64 bg-sidebar flex flex-col 
        transform transition-transform duration-200 lg:transform-none
        ${sidebarOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0'}
      `}
            >
                {/* Logo */}
                <div className="flex items-center justify-between px-4 py-5 border-b border-sidebar-border">
                    <Link href="/dashboard" className="text-white font-bold text-base">
                        DocSystem
                    </Link>
                    <button
                        onClick={() => setSidebarOpen(false)}
                        className="lg:hidden text-sidebar-muted hover:text-white"
                    >
                        <X className="w-5 h-5" />
                    </button>
                </div>

                {/* Navigation */}
                <nav className="flex-1 px-3 py-4 space-y-1 overflow-y-auto">
                    {menuItems.map((item) => {
                        const Icon = item.icon;
                        const isActive =
                            pathname === item.href ||
                            (item.href !== '/dashboard' && pathname.startsWith(item.href));

                        return (
                            <Link
                                key={item.href}
                                href={item.href}
                                onClick={() => setSidebarOpen(false)}
                                className={`
                  flex items-center gap-3 px-3 py-2.5 rounded-md text-sm transition
                  ${
                      isActive
                          ? 'bg-primary text-white'
                          : 'text-sidebar-muted hover:bg-sidebar-border hover:text-white'
                  }
                `}
                            >
                                <Icon className="w-5 h-5" />
                                <span>{item.label}</span>
                            </Link>
                        );
                    })}
                </nav>

                {/* User & Logout */}
                <div className="px-3 py-4 border-t border-sidebar-border">
                    {user && (
                        <div className="flex items-center gap-3 px-3 py-2 mb-2">
                            <div className="w-8 h-8 rounded-full bg-primary flex items-center justify-center text-white text-sm font-medium">
                                {user.fullName?.charAt(0).toUpperCase() || 'U'}
                            </div>
                            <div className="flex-1 min-w-0">
                                <p className="text-sm font-medium text-white truncate">
                                    {user.fullName}
                                </p>
                                <p className="text-xs text-sidebar-muted truncate">{user.email}</p>
                            </div>
                        </div>
                    )}
                    <button
                        onClick={logout}
                        className="w-full flex items-center gap-3 px-3 py-2.5 rounded-md text-sm
                       text-danger hover:bg-sidebar-border transition text-left"
                    >
                        <LogOut className="w-5 h-5" />
                        <span>Đăng xuất</span>
                    </button>
                </div>
            </aside>

            {/* Main Content */}
            <div className="flex-1 flex flex-col min-w-0">
                {/* Top Navbar */}
                <header className="sticky top-0 z-sticky flex items-center gap-4 px-4 lg:px-8 py-4 bg-background border-b border-border">
                    {/* Mobile Menu Button */}
                    <button
                        onClick={() => setSidebarOpen(true)}
                        className="lg:hidden p-2 -ml-2 text-muted hover:text-foreground"
                    >
                        <Menu className="w-6 h-6" />
                    </button>

                    {/* Spacer */}
                    <div className="flex-1" />

                    {/* Notifications */}
                    <button className="relative p-2 text-muted hover:text-foreground rounded-md hover:bg-surface-alt transition">
                        <Bell className="w-5 h-5" />
                        <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-danger rounded-full" />
                    </button>
                </header>

                {/* Page Content */}
                <main className="flex-1 p-4 lg:p-8">{children}</main>
            </div>
        </div>
    );
}
