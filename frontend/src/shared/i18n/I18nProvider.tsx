'use client';

import { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { DEFAULT_LOCALE, isLocale, Locale, LOCALE_STORAGE_KEY } from './config';

interface I18nContextValue {
    locale: Locale;
    setLocale: (locale: Locale) => void;
}

const I18nContext = createContext<I18nContextValue | undefined>(undefined);

export function I18nProvider({ children }: { children: React.ReactNode }) {
    // Always start from DEFAULT_LOCALE so the server render and the first client render match,
    // then adopt the persisted locale after mount (same approach as ThemeToggle / authStore).
    const [locale, setLocaleState] = useState<Locale>(DEFAULT_LOCALE);

    useEffect(() => {
        const stored = localStorage.getItem(LOCALE_STORAGE_KEY);
        if (isLocale(stored) && stored !== locale) {
            setLocaleState(stored);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const setLocale = useCallback((next: Locale) => {
        setLocaleState(next);
        localStorage.setItem(LOCALE_STORAGE_KEY, next);
        document.documentElement.lang = next;
    }, []);

    const value = useMemo<I18nContextValue>(() => ({ locale, setLocale }), [locale, setLocale]);

    return <I18nContext.Provider value={value}>{children}</I18nContext.Provider>;
}

export function useI18n(): I18nContextValue {
    const ctx = useContext(I18nContext);
    if (!ctx) {
        throw new Error('useI18n must be used within an I18nProvider');
    }
    return ctx;
}
