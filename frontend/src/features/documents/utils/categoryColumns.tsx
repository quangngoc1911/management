'use client';

import type { ReactNode } from 'react';
import { Edit, Trash2, Plus, Folder, FolderOpen, ChevronDown, ChevronRight } from 'lucide-react';
import type { Category } from '@/shared/lib/services/categoryService';

interface GetCategoryColumnsArgs {
    onEdit: (category: Category) => void;
    onDelete: (category: Category) => void;
    onAddChild: (category: Category) => void;
    expandedIds: Set<string>;
    onToggleExpand: (id: string) => void;
}

/** Renders a recursive category tree row (not a DataTable column set — categories use a custom tree view). */
export function renderCategoryTree(
    items: Category[],
    { onEdit, onDelete, onAddChild, expandedIds, onToggleExpand }: GetCategoryColumnsArgs,
    level = 0,
): ReactNode[] {
    return items.map((category) => {
        const hasChildren = !!category.children && category.children.length > 0;
        const isExpanded = expandedIds.has(category.id);
        const indent = level > 0 ? `ml-${level * 6}` : '';

        return (
            <div key={category.id}>
                <div className={`flex items-center gap-2 px-4 py-3 hover:bg-surface-alt transition ${indent}`}>
                    {hasChildren ? (
                        <button
                            onClick={() => onToggleExpand(category.id)}
                            className="p-1 text-muted hover:text-foreground"
                        >
                            {isExpanded ? (
                                <ChevronDown className="w-4 h-4" />
                            ) : (
                                <ChevronRight className="w-4 h-4" />
                            )}
                        </button>
                    ) : (
                        <span className="w-6" />
                    )}

                    {hasChildren || isExpanded ? (
                        <FolderOpen className="w-5 h-5 text-primary" />
                    ) : (
                        <Folder className="w-5 h-5 text-muted" />
                    )}

                    <div className="flex-1 min-w-0">
                        <p className="font-medium text-foreground truncate">{category.name}</p>
                        {category.description && (
                            <p className="text-xs text-muted truncate">{category.description}</p>
                        )}
                    </div>

                    <span className="badge badge-neutral">{category.documentCount || 0} tài liệu</span>

                    <div className="flex items-center gap-1">
                        <button
                            onClick={() => onAddChild(category)}
                            className="p-1.5 text-muted hover:text-primary hover:bg-surface-alt rounded transition"
                            title="Thêm danh mục con"
                        >
                            <Plus className="w-4 h-4" />
                        </button>
                        <button
                            onClick={() => onEdit(category)}
                            className="p-1.5 text-muted hover:text-primary hover:bg-surface-alt rounded transition"
                            title="Sửa"
                        >
                            <Edit className="w-4 h-4" />
                        </button>
                        <button
                            onClick={() => onDelete(category)}
                            className="p-1.5 text-muted hover:text-danger hover:bg-surface-alt rounded transition"
                            title="Xóa"
                        >
                            <Trash2 className="w-4 h-4" />
                        </button>
                    </div>
                </div>

                {hasChildren && isExpanded && (
                    <div className="border-l border-border ml-3">
                        {renderCategoryTree(
                            category.children!,
                            { onEdit, onDelete, onAddChild, expandedIds, onToggleExpand },
                            level + 1,
                        )}
                    </div>
                )}
            </div>
        );
    });
}
