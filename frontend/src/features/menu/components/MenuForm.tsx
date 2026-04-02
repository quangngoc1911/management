'use client';
import { useMenu } from '@/features/menu/hooks/useMenu';
import { CreateMenuRequest } from '@/features/menu/types/menu';
import { useState } from 'react';

// Class dùng lại cho mọi input/select trong form — định nghĩa 1 lần
const inputClass = `
    w-full bg-surface-alt border border-border rounded-md
    px-3 py-2 text-sm text-foreground placeholder:text-muted
    focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary
    transition
`.trim();

export default function MenuForm({ onSuccess }: { onSuccess?: () => void }) {
    const { createMenu, loading, error, menus } = useMenu();

    const [form, setForm] = useState<CreateMenuRequest>({
        name: '',
        path: '',
        icon: '',
        order: 0,
        isVisible: true,
        parentId: null,
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const ok = await createMenu({ ...form, parentId: form.parentId || null });
        if (ok) {
            setForm({ name: '', path: '', icon: '', order: 0, isVisible: true, parentId: null });
            onSuccess?.();
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            className="space-y-4 p-6 bg-surface border border-border rounded-lg"
        >
            <h2 className="text-base font-semibold text-foreground">Tạo menu mới</h2>

            {error && (
                <div
                    className="p-3 rounded-lg text-sm
                                bg-red-50 text-red-700
                                dark:bg-red-900/20 dark:text-red-400"
                >
                    {error}
                </div>
            )}

            <div className="grid grid-cols-2 gap-4">
                <div className="flex flex-col gap-1">
                    <label className="text-sm font-medium text-foreground">Tên menu *</label>
                    <input
                        type="text"
                        required
                        value={form.name}
                        onChange={(e) => setForm((p) => ({ ...p, name: e.target.value }))}
                        className={inputClass}
                        placeholder="Ví dụ: Tài liệu"
                    />
                </div>

                <div className="flex flex-col gap-1">
                    <label className="text-sm font-medium text-foreground">Đường dẫn *</label>
                    <input
                        type="text"
                        required
                        value={form.path}
                        onChange={(e) => setForm((p) => ({ ...p, path: e.target.value }))}
                        className={inputClass}
                        placeholder="Ví dụ: /documents"
                    />
                </div>

                <div className="flex flex-col gap-1">
                    <label className="text-sm font-medium text-foreground">Icon</label>
                    <input
                        type="text"
                        value={form.icon}
                        onChange={(e) => setForm((p) => ({ ...p, icon: e.target.value }))}
                        className={inputClass}
                        placeholder="Ví dụ: 📁"
                    />
                </div>

                <div className="flex flex-col gap-1">
                    <label className="text-sm font-medium text-foreground">Thứ tự</label>
                    <input
                        type="number"
                        value={form.order}
                        onChange={(e) => setForm((p) => ({ ...p, order: +e.target.value }))}
                        className={inputClass}
                    />
                </div>

                <div className="flex flex-col gap-1">
                    <label className="text-sm font-medium text-foreground">Menu cha</label>
                    <select
                        value={form.parentId ?? ''}
                        onChange={(e) =>
                            setForm((p) => ({
                                ...p,
                                parentId: e.target.value ? +e.target.value : null,
                            }))
                        }
                        className={inputClass}
                    >
                        <option value="">-- Không có --</option>
                        {menus.map((m) => (
                            <option key={m.id} value={m.id}>
                                {m.name}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="flex items-center gap-2 mt-6">
                    <input
                        type="checkbox"
                        id="isVisible"
                        checked={form.isVisible}
                        onChange={(e) => setForm((p) => ({ ...p, isVisible: e.target.checked }))}
                        className="w-4 h-4 rounded-sm accent-primary"
                    />
                    <label htmlFor="isVisible" className="text-sm text-foreground">
                        Hiển thị
                    </label>
                </div>
            </div>

            <button
                type="submit"
                disabled={loading}
                className="w-full bg-primary hover:bg-primary-hover text-white
                           py-2 rounded-md text-sm font-medium
                           disabled:opacity-50 disabled:cursor-not-allowed transition"
            >
                {loading ? 'Đang tạo...' : 'Tạo menu'}
            </button>
        </form>
    );
}
