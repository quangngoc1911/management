'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';

const TOKEN_KEY = 'access_token';

export default function HomePage() {
    const router = useRouter();

    useEffect(() => {
        const token = localStorage.getItem(TOKEN_KEY);

        if (token) {
            router.replace('/dashboard');
        } else {
            router.replace('/login');
        }
    }, [router]);

    return null;
}
