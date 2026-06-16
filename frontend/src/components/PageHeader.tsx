import { ReactNode } from 'react';

interface PageHeaderProps {
    title: string;
    subtitle?: string;
    /** Optional right-aligned slot, typically a primary action button. */
    action?: ReactNode;
}

/** Standard page heading: title + subtitle on the left, an action slot on the right. */
export function PageHeader({ title, subtitle, action }: PageHeaderProps) {
    return (
        <div className="page-header">
            <div>
                <h1 className="text-2xl font-bold text-foreground">{title}</h1>
                {subtitle && <p className="text-muted mt-1">{subtitle}</p>}
            </div>
            {action}
        </div>
    );
}

export default PageHeader;
