'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { Column } from '@/components/DataTable';
import { RowActions } from '@/components/RowActions';
import { useTranslation } from '@/shared/i18n/useTranslation';
import type { User } from '../types/user';

interface UseUserColumnsArgs {
    onEdit: (user: User) => void;
    onDelete: (user: User) => void;
}

/** Localized table columns for the Users list, including the row-action column. */
export function useUserColumns({ onEdit, onDelete }: UseUserColumnsArgs): Column<User>[] {
    const { t } = useTranslation('users');
    const { t: tc } = useTranslation('common');

    return [
        { key: 'fullName', header: t('columns.fullName'), accessor: 'fullName' },
        { key: 'email', header: t('columns.email'), accessor: 'email' },
        {
            key: 'role',
            header: t('columns.role'),
            accessor: 'role',
            render: (_v, row) => t(`roles.${row.role}`),
        },
        {
            key: 'isActive',
            header: t('columns.status'),
            accessor: 'isActive',
            render: (_v, row) =>
                row.isActive ? (
                    <span className="badge badge-success">{tc('status.active')}</span>
                ) : (
                    <span className="text-muted">{tc('status.inactive')}</span>
                ),
        },
        {
            key: 'actions',
            header: tc('actions.actions'),
            accessor: 'id',
            render: (_v, row) => (
                <RowActions
                    actions={[
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
