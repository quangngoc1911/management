'use client';

import { useCallback, useEffect, useState } from 'react';
import { useDebouncedValue } from './useDebouncedValue';

export interface PaginatedFetchResult<T> {
    items: T[];
    totalCount: number;
}

export interface UsePaginatedListOptions<T> {
    /** Stable reference (e.g. a service method) that fetches one page. */
    fetcher: (params: { page: number; pageSize: number; search?: string }) => Promise<PaginatedFetchResult<T>>;
    pageSize?: number;
    debounceMs?: number;
    /** Localized fallback message used when the thrown error carries none. */
    errorMessage?: string;
}

export interface UsePaginatedListResult<T> {
    items: T[];
    total: number;
    loading: boolean;
    error: string | null;
    page: number;
    pageSize: number;
    search: string;
    setPage: (page: number) => void;
    setSearch: (search: string) => void;
    setError: (error: string | null) => void;
    /** Refetch the current page (call after create/update/delete). */
    reload: () => Promise<void>;
}

/**
 * Encapsulates the list concern duplicated across every CRUD page:
 * items, total, loading, error, pagination and debounced search.
 */
export function usePaginatedList<T>({
    fetcher,
    pageSize = 10,
    debounceMs = 300,
    errorMessage = 'Failed to load data',
}: UsePaginatedListOptions<T>): UsePaginatedListResult<T> {
    const [items, setItems] = useState<T[]>([]);
    const [total, setTotal] = useState(0);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [page, setPage] = useState(1);
    const [search, setSearchState] = useState('');

    const debouncedSearch = useDebouncedValue(search, debounceMs);

    const load = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await fetcher({
                page,
                pageSize,
                search: debouncedSearch.trim() || undefined,
            });
            setItems(data.items);
            setTotal(data.totalCount);
        } catch (err) {
            setItems([]);
            setTotal(0);
            setError(err instanceof Error ? err.message : errorMessage);
        } finally {
            setLoading(false);
        }
    }, [fetcher, page, pageSize, debouncedSearch, errorMessage]);

    useEffect(() => {
        load();
    }, [load]);

    // Reset to the first page whenever the search term changes.
    const setSearch = useCallback((value: string) => {
        setSearchState(value);
        setPage(1);
    }, []);

    return {
        items,
        total,
        loading,
        error,
        page,
        pageSize,
        search,
        setPage,
        setSearch,
        setError,
        reload: load,
    };
}
