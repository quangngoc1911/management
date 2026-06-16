import { FALLBACK_LOCALE, Locale, Namespace } from './config';
import { resources } from './resources';

export type TranslationVars = Record<string, string | number>;

/** Walks a dot-separated path (e.g. "form.fullName") into a nested dictionary. */
function getPath(dict: unknown, path: string): string | undefined {
    const value = path.split('.').reduce<unknown>((acc, key) => {
        if (acc && typeof acc === 'object') {
            return (acc as Record<string, unknown>)[key];
        }
        return undefined;
    }, dict);
    return typeof value === 'string' ? value : undefined;
}

/** Replaces {placeholders} in a template with provided values. Unknown vars are left intact. */
function interpolate(template: string, vars?: TranslationVars): string {
    if (!vars) return template;
    return template.replace(/\{(\w+)\}/g, (_, key: string) =>
        key in vars ? String(vars[key]) : `{${key}}`,
    );
}

/**
 * Resolves a translation key.
 * - `key` may be scoped to another namespace with the `ns:key` form (e.g. "common:actions.save").
 * - Falls back to FALLBACK_LOCALE, then returns the raw key if still missing.
 */
export function translate(
    locale: Locale,
    namespace: Namespace,
    key: string,
    vars?: TranslationVars,
): string {
    let targetNs: Namespace = namespace;
    let realKey = key;

    const sep = key.indexOf(':');
    if (sep > -1) {
        targetNs = key.slice(0, sep) as Namespace;
        realKey = key.slice(sep + 1);
    }

    const fromLocale = getPath(resources[locale]?.[targetNs], realKey);
    if (fromLocale !== undefined) return interpolate(fromLocale, vars);

    const fromFallback = getPath(resources[FALLBACK_LOCALE]?.[targetNs], realKey);
    if (fromFallback !== undefined) return interpolate(fromFallback, vars);

    return key;
}
