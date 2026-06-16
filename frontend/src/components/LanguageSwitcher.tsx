'use client';

import { Globe } from 'lucide-react';
import { useTranslation } from '@/shared/i18n/useTranslation';

/**
 * Compact locale toggle for the topbar. Mirrors ThemeToggle's placement/style.
 * State is consistent between server and first client render (both DEFAULT_LOCALE),
 * so no mounted guard is required.
 */
export function LanguageSwitcher() {
    const { t, locale, setLocale, locales } = useTranslation('common');

    const next = locales[(locales.indexOf(locale) + 1) % locales.length];

    return (
        <button
            onClick={() => setLocale(next)}
            title={t('language.label')}
            aria-label={t('language.label')}
            className="flex items-center gap-2 p-2 text-muted hover:text-foreground rounded-md hover:bg-surface-alt transition"
        >
            <Globe className="w-5 h-5" />
            <span className="text-sm font-medium uppercase">{locale}</span>
        </button>
    );
}

export default LanguageSwitcher;
