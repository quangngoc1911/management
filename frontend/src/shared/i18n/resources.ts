import { Locale, Namespace } from './config';

import viCommon from './locales/vi/common.json';
import viUsers from './locales/vi/users.json';
import viFamily from './locales/vi/family.json';
import enCommon from './locales/en/common.json';
import enUsers from './locales/en/users.json';
import enFamily from './locales/en/family.json';

type Dictionary = Record<string, unknown>;

/** Static map of every (locale, namespace) dictionary. Add new namespaces here. */
export const resources: Record<Locale, Record<Namespace, Dictionary>> = {
    vi: { common: viCommon, users: viUsers, family: viFamily },
    en: { common: enCommon, users: enUsers, family: enFamily },
};
