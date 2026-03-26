'use client';
import { Input } from './Input';
import { Button } from './Button';

type Field = { name: string; type: 'text' | 'textarea'; required?: boolean };

interface Props {
    fields: Field[];
    values: Record<string, string>;
    errors: Record<string, string>;
    onChange: (name: string, value: string) => void;
    onSubmit: () => void;
}

export function DynamicForm({ fields, values, errors, onChange, onSubmit }: Props) {
    return (
        <form
            onSubmit={(e) => {
                e.preventDefault();
                onSubmit();
            }}
            className="space-y-4"
        >
            {fields.map((field) => {
                if (field.type === 'text') {
                    return (
                        <Input
                            key={field.name}
                            name={field.name}
                            value={values[field.name] || ''}
                            onChange={(e) => onChange(field.name, e.target.value)}
                            placeholder={field.name}
                            error={errors[field.name]}
                        />
                    );
                }

                if (field.type === 'textarea') {
                    return (
                        <div key={field.name} className="flex flex-col gap-1">
                            <textarea
                                value={values[field.name] || ''}
                                onChange={(e) => onChange(field.name, e.target.value)}
                                placeholder={field.name}
                                className="w-full bg-surface-alt border border-border rounded-md
                                           px-3 py-2 text-sm text-foreground placeholder:text-muted
                                           focus:outline-none focus:ring-2 focus:ring-primary
                                           resize-none transition"
                            />
                            {errors[field.name] && (
                                <span className="text-xs text-danger">{errors[field.name]}</span>
                            )}
                        </div>
                    );
                }

                return null;
            })}
            <Button type="submit">Lưu</Button>
        </form>
    );
}
