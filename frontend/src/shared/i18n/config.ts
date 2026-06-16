// i18n configuration. Custom lightweight i18n (no extra dependency) that mirrors
// the project's existing React-Context store pattern (see shared/lib/store/authStore).

export const LOCALES = ['vi', 'en'] as const;
export type Locale = (typeof LOCALES)[number];

/** Default locale rendered on the server and on the first client render (avoids hydration mismatch). */
export const DEFAULT_LOCALE: Locale = 'vi';

/** Locale used when a key is missing in the active locale. Content is authored in vi. */
export const FALLBACK_LOCALE: Locale = 'vi';

/** localStorage key holding the user's chosen locale. */
export const LOCALE_STORAGE_KEY = 'locale';

/** Translation namespaces. One JSON file per (locale, namespace). */
export const NAMESPACES = ['common', 'users', 'family'] as const;
export type Namespace = (typeof NAMESPACES)[number];

export const isLocale = (value: unknown): value is Locale =>
    typeof value === 'string' && (LOCALES as readonly string[]).includes(value);
