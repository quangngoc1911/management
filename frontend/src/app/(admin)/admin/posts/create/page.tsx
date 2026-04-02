'use client';
import { useForm } from '@/shared/hooks/useForm';
import { Input } from '@/components/Input';
import { Button } from '@/components/Button';

interface CreatePostDto {
    title: string;
    description: string;
}

export default function CreatePostPage() {
    const form = useForm<CreatePostDto>({ title: '', description: '' }, (values) => {
        const errors: any = {};
        if (!values.title) errors.title = 'Nhập tiêu đề';
        if (!values.description) errors.description = 'Nhập nội dung';
        return errors;
    });

    const onSubmit = (values: CreatePostDto) => {
        console.log('Submit:', values);
    };

    return (
        <div className="max-w-2xl space-y-6">
            <h1 className="text-2xl font-bold text-foreground">Tạo bài viết</h1>

            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
                <Input
                    name="title"
                    placeholder="Tiêu đề"
                    value={form.values.title}
                    onChange={form.handleChange}
                    error={form.errors.title}
                />

                <div className="flex flex-col gap-1">
                    <textarea
                        name="description"
                        value={form.values.description}
                        onChange={form.handleChange as any}
                        placeholder="Nội dung"
                        className="w-full bg-surface-alt border border-border rounded-md
                                   px-3 py-2 text-sm text-foreground placeholder:text-muted
                                   focus:outline-none focus:ring-2 focus:ring-primary
                                   resize-none h-40 transition"
                    />
                    {form.errors.description && (
                        <span className="text-xs text-danger">{form.errors.description}</span>
                    )}
                </div>

                <Button type="submit">Đăng bài</Button>
            </form>
        </div>
    );
}
