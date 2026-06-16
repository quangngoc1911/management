'use client';

import type { LucideIcon } from 'lucide-react';

export interface RowAction {
    icon: LucideIcon;
    title: string;
    onClick: () => void;
    variant?: 'default' | 'danger';
}

/** Renders a row of compact icon buttons for table row actions (view/edit/delete...). */
export function RowActions({ actions }: { actions: RowAction[] }) {
    return (
        <div className="flex items-center gap-2">
            {actions.map((action, i) => {
                const Icon = action.icon;
                const hover =
                    action.variant === 'danger' ? 'hover:text-danger' : 'hover:text-primary';
                return (
                    <button
                        key={i}
                        onClick={action.onClick}
                        title={action.title}
                        className={`p-1.5 text-muted ${hover} hover:bg-surface-alt rounded transition`}
                    >
                        <Icon className="w-4 h-4" />
                    </button>
                );
            })}
        </div>
    );
}

export default RowActions;
