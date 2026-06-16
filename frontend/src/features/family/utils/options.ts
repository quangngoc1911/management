// Enum values mirror backend ManagementSystem.Domain.Enums (serialized as integers).
// Labels are localized via the `family` namespace: e.g. t(`gender.${value}`).

export const GENDER_OPTIONS = [1, 2, 3] as const;

export const RELATION_TO_HEAD_OPTIONS = [0, 1, 2, 3, 4, 5, 6, 7, 99] as const;

export const MARITAL_STATUS_OPTIONS = [1, 2, 3, 4, 5] as const;

export const EDUCATION_LEVEL_OPTIONS = [0, 1, 2, 3, 4, 5, 6, 7, 8] as const;

export const RELATIONSHIP_TYPE_OPTIONS = [
    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 99,
] as const;
