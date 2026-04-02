'use client';
import { useMenu } from '@/features/menu/hooks/useMenu';
import MenuForm from '../../../features/menu/components/MenuForm';


export default function MenuPage() {
    const { menus, loading } = useMenu();

    return (
        <>
            <div className="page-header">
                <h1 className="text-2xl font-bold text-foreground">Quản lý Menu</h1>
            </div>

            <MenuForm />

            {loading ? (
                <p className="text-sm text-muted animate-pulse">Đang tải...</p>
            ) : (
                <div className="table-wrapper">
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
                                                'badge',
                                                m.isVisible ? 'badge-success' : 'badge-neutral',
                                            ].join(' ')}
                                        >
                                            {m.isVisible ? 'Hiện' : 'Ẩn'}
                                        </span>
                                    </td>
                                </tr>
                            ))}
                            {menus.length === 0 && (
                                <tr>
                                    <td
                                        colSpan={4}
                                        className="px-4 py-10 text-center text-muted text-sm"
                                    >
                                        Không có dữ liệu
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            )}
        </>
    );
}
