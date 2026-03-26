'use client';
import { useMenu } from '@/shared/hooks/useMenu';
import MenuForm from './components/MenuForm';

export default function MenuPage() {
    const { menus, loading } = useMenu();

    return (
        <div className="space-y-6">
            <h1 className="text-2xl font-bold text-foreground">Quản lý Menu</h1>

            <MenuForm />

            {loading ? (
                <p className="text-sm text-muted animate-pulse">Đang tải...</p>
            ) : (
                <div className="bg-surface border border-border rounded-lg overflow-hidden">
                    <table className="w-full text-sm">
                        <thead className="bg-surface-alt border-b border-border">
                            <tr>
                                {['Tên', 'Đường dẫn', 'Thứ tự', 'Hiển thị'].map((h) => (
                                    <th
                                        key={h}
                                        className="px-4 py-3 text-left text-xs font-semibold
                                                   text-muted uppercase tracking-wide"
                                    >
                                        {h}
                                    </th>
                                ))}
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-border">
                            {menus.map((m) => (
                                <tr key={m.id} className="hover:bg-surface-alt transition-colors">
                                    <td className="px-4 py-3 text-foreground">{m.name}</td>
                                    <td className="px-4 py-3 text-muted">{m.path}</td>
                                    <td className="px-4 py-3 text-foreground">{m.order}</td>
                                    <td className="px-4 py-3">
                                        <span
                                            className={[
                                                'px-2 py-0.5 rounded-full text-xs font-medium',
                                                m.isVisible
                                                    ? 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400'
                                                    : 'bg-surface-alt text-muted',
                                            ].join(' ')}
                                        >
                                            {m.isVisible ? 'Hiện' : 'Ẩn'}
                                        </span>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}
