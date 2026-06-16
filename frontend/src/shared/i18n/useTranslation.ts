'use client';

import { useCallback } from 'react';
import { LOCALES, Namespace } from './config';
import { useI18n } from './I18nProvider';
import { translate, TranslationVars } from './translate';

/** Signature of the `t` function. Use this when passing `t` into utils/builders. */
export type Translate = (key: string, vars?: TranslationVars) => string;

export interface UseTranslationResult {
    /** Translate a key within `namespace`. Use `ns:key` to reach another namespace. */
    t: Translate;
    locale: ReturnType<typeof useI18n>['locale'];
    setLocale: ReturnType<typeof useI18n>['setLocale'];
    locales: typeof LOCALES;
}

/**
 * Localization hook. Pages/components call `const { t } = useTranslation('users')`.
 * Keeps text out of components and provides interpolation + namespace fallback.
 */
export function useTranslation(namespace: Namespace = 'common'): UseTranslationResult {
    const { locale, setLocale } = useI18n();

    const t = useCallback(
        (key: string, vars?: TranslationVars) => translate(locale, namespace, key, vars),
        [locale, namespace],
    );

    return { t, locale, setLocale, locales: LOCALES };
}
