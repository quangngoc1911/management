import { ReactNode } from 'react';

interface FormFieldProps {
    label: string;
    required?: boolean;
    error?: string;
    /** Spans both columns inside a 2-column grid form. */
    full?: boolean;
    children: ReactNode;
}

/** Label + (optional) required marker + control + validation error message. */
export function FormField({ label, required, error, full, children }: FormFieldProps) {
    return (
        <div className={full ? 'md:col-span-2' : ''}>
            <label className="block text-sm font-medium text-foreground mb-2">
                {label}
                {required && <span className="text-danger"> *</span>}
            </label>
            {children}
            {error && <p className="text-danger text-xs mt-1">{error}</p>}
        </div>
    );
}

export default FormField;
