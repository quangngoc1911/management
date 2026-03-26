'use client';
import { useState } from 'react';
import { useAuth } from '@/shared/hooks/useAuth';

export default function LoginPage() {
    const { login, loading, error } = useAuth();
    const [form, setForm] = useState({ email: '', password: '' });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await login(form);
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-background">
            <div
                className="bg-surface border border-border rounded-lg shadow-sm
                            w-full max-w-md mx-4 p-8"
            >
                <h1 className="text-2xl font-bold mb-6 text-center text-foreground">Đăng nhập</h1>

                {error && (
                    <div
                        className="mb-4 p-3 rounded-lg text-sm
                                    bg-red-50 text-red-700
                                    dark:bg-red-900/20 dark:text-red-400"
                    >
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div className="flex flex-col gap-1">
                        <label className="block text-sm font-medium text-foreground">Email</label>
                        <input
                            type="email"
                            required
                            value={form.email}
                            onChange={(e) => setForm((p) => ({ ...p, email: e.target.value }))}
                            className="w-full bg-surface-alt border border-border rounded-md
                                       px-3 py-2 text-sm text-foreground placeholder:text-muted
                                       focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary
                                       transition"
                            placeholder="user@example.com"
                        />
                    </div>

                    <div className="flex flex-col gap-1">
                        <label className="block text-sm font-medium text-foreground">
                            Mật khẩu
                        </label>
                        <input
                            type="password"
                            required
                            value={form.password}
                            onChange={(e) => setForm((p) => ({ ...p, password: e.target.value }))}
                            className="w-full bg-surface-alt border border-border rounded-md
                                       px-3 py-2 text-sm text-foreground placeholder:text-muted
                                       focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary
                                       transition"
                            placeholder="••••••••"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-primary hover:bg-primary-hover text-white
                                   py-2 rounded-md text-sm font-medium
                                   disabled:opacity-50 disabled:cursor-not-allowed transition"
                    >
                        {loading ? 'Đang đăng nhập...' : 'Đăng nhập'}
                    </button>
                </form>
            </div>
        </div>
    );
}
