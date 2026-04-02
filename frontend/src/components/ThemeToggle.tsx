'use client';

import { useTheme } from 'next-themes';
import { useEffect, useState } from 'react';

export function ThemeToggle() {
    const { resolvedTheme, setTheme } = useTheme();
    const [mounted, setMounted] = useState(false);

    useEffect(() => {
        const id = requestAnimationFrame(() => setMounted(true));
        return () => cancelAnimationFrame(id);
    }, []);

    // Trả về placeholder cùng kích thước để tránh layout shift
    if (!mounted) {
        return <span className="inline-block w-20 h-7" />;
    }

    const isDark = resolvedTheme === 'dark';

    return (
        <button
            onClick={() => setTheme(isDark ? 'light' : 'dark')}
            className="inline-flex items-center gap-1.5 border border-sidebar-border
                       text-sidebar-muted hover:text-white hover:border-white/30
                       px-3 py-1 rounded-md text-xs font-medium transition"
        >
            {isDark ? '☀️ Light' : '🌙 Dark'}
        </button>
    );
}
