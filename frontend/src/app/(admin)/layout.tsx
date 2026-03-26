export default function AdminLayout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex h-screen">
            {/* Sidebar */}
            <aside className="w-64 border-r p-4 bg-gray-100 dark:bg-gray-800">
                <h2 className="font-bold mb-4">Menu</h2>

                <ul className="space-y-2">
                    <li>
                        <a href="/admin/posts" className="block hover:underline">
                            Bài viết
                        </a>
                    </li>
                    <li>
                        <a href="/admin/posts/create" className="block hover:underline">
                            Tạo bài viết
                        </a>
                    </li>
                </ul>
            </aside>

            {/* Content */}
            <main className="flex-1 p-6 overflow-auto">{children}</main>
        </div>
    );
}
