'use client';

import { Edit, Trash2, IdCard, Network } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import { useTranslation } from '@/shared/i18n/useTranslation';
import type { FamilyMember } from '../types/family';

interface UseFamilyColumnsArgs {
    onProfile: (member: FamilyMember) => void;
    onRelationships: (member: FamilyMember) => void;
    onEdit: (member: FamilyMember) => void;
    onDelete: (member: FamilyMember) => void;
}

/** Localized table columns for the Family Members list, including the row-action column. */
export function useFamilyColumns({
    onProfile,
    onRelationships,
    onEdit,
    onDelete,
}: UseFamilyColumnsArgs): Column<FamilyMember>[] {
    const { t } = useTranslation('family');
    const { t: tc } = useTranslation('common');

    return [
        { key: 'fullName', header: t('columns.fullName'), accessor: 'fullName' },
        {
            key: 'relationToHead',
            header: t('columns.relationToHead'),
            accessor: 'relationToHead',
            render: (_v, row) => (row.relationToHead != null ? t(`relationToHead.${row.relationToHead}`) : '—'),
        },
        {
            key: 'gender',
            header: t('columns.gender'),
            accessor: 'gender',
            render: (_v, row) => (row.gender != null ? t(`gender.${row.gender}`) : '—'),
        },
        { key: 'phone', header: t('columns.phone'), accessor: 'phone' },
        {
            key: 'isHouseholdHead',
            header: t('columns.isHouseholdHead'),
            accessor: 'isHouseholdHead',
            render: (_v, row) =>
                row.isHouseholdHead ? (
                    <span className="badge badge-success">{t('relationToHead.0')}</span>
                ) : (
                    <span className="text-muted">—</span>
                ),
        },
        {
            key: 'actions',
            header: tc('actions.actions'),
            accessor: 'id',
            render: (_v, row) => (
                <RowActions
                    actions={[
                        { icon: IdCard, title: t('rowActions.profile'), onClick: () => onProfile(row) },
                        {
                            icon: Network,
                            title: t('rowActions.relationships'),
                            onClick: () => onRelationships(row),
                        },
                        { icon: Edit, title: tc('actions.edit'), onClick: () => onEdit(row) },
                        {
                            icon: Trash2,
                            title: tc('actions.delete'),
                            variant: 'danger',
                            onClick: () => onDelete(row),
                        },
                    ]}
                />
            ),
        },
    ];
}
