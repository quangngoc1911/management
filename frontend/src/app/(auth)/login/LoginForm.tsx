'use client';
import { useState } from 'react';
import { useRouter } from 'next/navigation';

export default function LoginForm() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const router = useRouter();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        try {
            const res = await fetch('/api/auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
            });
            if (!res.ok) {
                const data = await res.json();
                setError(data.message || 'Đăng nhập thất bại');
                setLoading(false);
                return;
            }
            const data = await res.json();
            localStorage.setItem('token', data.token || data.accessToken);
            router.push('/dashboard');
        } catch (err) {
            setError('Lỗi hệ thống. Vui lòng thử lại.');
            setLoading(false);
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            className="space-y-4 max-w-sm mx-auto mt-10 p-6 border rounded shadow"
        >
            <h2 className="text-xl font-bold mb-4">Đăng nhập</h2>
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                className="w-full p-2 border rounded"
            />
            <input
                type="password"
                placeholder="Mật khẩu"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                className="w-full p-2 border rounded"
            />
            {error && <div className="text-red-600 text-sm">{error}</div>}
            <button
                type="submit"
                disabled={loading}
                className="w-full bg-blue-600 text-white p-2 rounded hover:bg-blue-700 disabled:opacity-60"
            >
                {loading ? 'Đang đăng nhập...' : 'Đăng nhập'}
            </button>
        </form>
    );
}
