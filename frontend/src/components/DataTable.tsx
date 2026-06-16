'use client';

import { ReactNode } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { useTranslation } from '@/shared/i18n/useTranslation';

export interface Column<T> {
    key: string;
    header: string;
    accessor: keyof T;
    render?: (value: T[keyof T], row: T) => ReactNode;
    className?: string;
}

interface DataTableProps<T> {
    data: T[];
    columns: Column<T>[];
    loading?: boolean;
    /** Optional override; defaults to the localized "no data" text. */
    emptyText?: string;
}

export function DataTable<T extends { id?: string | number }>({
    data,
    columns,
    loading = false,
    emptyText,
}: DataTableProps<T>) {
    const { t } = useTranslation('common');
    const empty = emptyText ?? t('table.empty');

    if (loading) {
        return (
            <div className="table-wrapper">
                <table className="w-full text-sm">
                    <thead>
                        <tr className="bg-surface-alt border-b border-border text-left">
                            {columns.map((col) => (
                                <th
                                    key={col.key}
                                    className="px-4 py-3 text-xs font-semibold text-muted uppercase tracking-wide"
                                >
                                    {col.header}
                                </th>
                            ))}
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colSpan={columns.length} className="px-4 py-10 text-center text-muted">
                                {t('table.loading')}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        );
    }

    return (
        <div className="table-wrapper">
            <table className="w-full text-sm">
                <thead>
                    <tr className="bg-surface-alt border-b border-border text-left">
                        {columns.map((col) => (
                            <th
                                key={col.key}
                                className={`px-4 py-3 text-xs font-semibold text-muted uppercase tracking-wide ${col.className || ''}`}
                            >
                                {col.header}
                            </th>
                        ))}
                    </tr>
                </thead>

                <tbody className="divide-y divide-border">
                    {data.length > 0 ? (
                        data.map((row, i) => (
                            <tr key={row.id ?? i} className="hover:bg-surface-alt transition-colors">
                                {columns.map((col) => (
                                    <td
                                        key={col.key}
                                        className={`px-4 py-3 text-foreground ${col.className || ''}`}
                                    >
                                        {col.render
                                            ? col.render(row[col.accessor], row)
                                            : String(row[col.accessor] ?? '')}
                                    </td>
                                ))}
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={columns.length} className="px-4 py-10 text-center text-muted">
                                {empty}
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}

// Pagination Component
interface PaginationProps {
    page: number;
    pageSize: number;
    total: number;
    onPageChange: (page: number) => void;
}

export function Pagination({ page, pageSize, total, onPageChange }: PaginationProps) {
    const { t } = useTranslation('common');
    const totalPages = Math.ceil(total / pageSize);

    if (totalPages <= 1) return null;

    const start = (page - 1) * pageSize + 1;
    const end = Math.min(page * pageSize, total);

    return (
        <div className="flex items-center justify-between px-4 py-3 border-t border-border">
            <div className="text-sm text-muted">{t('table.showing', { start, end, total })}</div>

            <div className="flex items-center gap-2">
                <button
                    onClick={() => onPageChange(page - 1)}
                    disabled={page <= 1}
                    className="p-2 rounded-md text-muted hover:bg-surface-alt hover:text-foreground disabled:opacity-50 disabled:cursor-not-allowed transition"
                >
                    <ChevronLeft className="w-4 h-4" />
                </button>

                <span className="text-sm text-foreground">{t('table.page', { page, totalPages })}</span>

                <button
                    onClick={() => onPageChange(page + 1)}
                    disabled={page >= totalPages}
                    className="p-2 rounded-md text-muted hover:bg-surface-alt hover:text-foreground disabled:opacity-50 disabled:cursor-not-allowed transition"
                >
                    <ChevronRight className="w-4 h-4" />
                </button>
            </div>
        </div>
    );
}

// DataTable with Pagination
interface PaginatedTableProps<T> extends DataTableProps<T> {
    page: number;
    pageSize: number;
    total: number;
    onPageChange: (page: number) => void;
}

export function PaginatedTable<T extends { id?: string | number }>({
    data,
    columns,
    loading = false,
    emptyText,
    page,
    pageSize,
    total,
    onPageChange,
}: PaginatedTableProps<T>) {
    return (
        <div className="card overflow-hidden">
            <DataTable data={data} columns={columns} loading={loading} emptyText={emptyText} />
            <Pagination page={page} pageSize={pageSize} total={total} onPageChange={onPageChange} />
        </div>
    );
}
