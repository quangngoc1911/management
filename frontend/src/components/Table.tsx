type Column<T> = {
    header: string;
    accessor: keyof T;
};

interface TableProps<T> {
    data: T[];
    columns: Column<T>[];
}

export function Table<T extends { id?: number | string }>({ data, columns }: TableProps<T>) {
    return (
        <div className="bg-surface border border-border rounded-lg overflow-hidden">
            <table className="w-full text-sm">
                <thead>
                    <tr className="bg-surface-alt border-b border-border text-left">
                        {columns.map((col) => (
                            <th
                                key={String(col.accessor)}
                                className="px-4 py-3 text-xs font-semibold text-muted uppercase tracking-wide"
                            >
                                {col.header}
                            </th>
                        ))}
                    </tr>
                </thead>

                <tbody className="divide-y divide-border">
                    {data.map((row, i) => (
                        <tr key={row.id ?? i} className="hover:bg-surface-alt transition-colors">
                            {columns.map((col) => (
                                <td
                                    key={String(col.accessor)}
                                    className="px-4 py-3 text-foreground"
                                >
                                    {String(row[col.accessor] ?? '')}
                                </td>
                            ))}
                        </tr>
                    ))}

                    {data.length === 0 && (
                        <tr>
                            <td
                                colSpan={columns.length}
                                className="px-4 py-10 text-center text-muted text-sm"
                            >
                                Không có dữ liệu
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}
