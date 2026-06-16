'use client';

import { Search } from 'lucide-react';

interface SearchInputProps {
    value: string;
    onChange: (value: string) => void;
    placeholder?: string;
}

/** Card-wrapped search box used above list tables. */
export function SearchInput({ value, onChange, placeholder }: SearchInputProps) {
    return (
        <div className="card p-4">
            <div className="relative max-w-md">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted" />
                <input
                    type="text"
                    value={value}
                    onChange={(e) => onChange(e.target.value)}
                    placeholder={placeholder}
                    className="input pl-10"
                />
            </div>
        </div>
    );
}

export default SearchInput;
