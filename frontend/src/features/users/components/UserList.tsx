'use client';

import { useUsers, useCreateUser } from '../hooks/useUsers';
import { Input } from '@/components/Input';
import { Button } from '@/components/Button';
import { Table } from '@/components/Table';
import { useForm, Errors } from '@/shared/hooks/useForm';
import { CreateUserDto, User } from '../types';

export function UserList() {
    const { data: users = [], isLoading, isError } = useUsers();
    const { mutate: createUser, isPending } = useCreateUser();

    const form = useForm<CreateUserDto>({ name: '', email: '' }, (values) => {
        const errors: Errors<CreateUserDto> = {};

        if (!values.name) {
            errors.name = 'Tên không được bỏ trống';
        }

        if (!values.email) {
            errors.email = 'Email không được bỏ trống';
        } else if (!values.email.includes('@')) {
            errors.email = 'Email không hợp lệ';
        }

        return errors;
    });

    const onSubmit = (values: CreateUserDto) => {
        createUser(values, {
            onSuccess: () => {
                form.setValues({ name: '', email: '' });
            },
        });
    };

    if (isLoading) return <p>Đang tải...</p>;
    if (isError) return <p className="text-red-500">Lỗi API</p>;

    return (
        <div className="space-y-6">
            <form onSubmit={form.handleSubmit(onSubmit)} className="flex gap-3">
                <Input
                    name="name"
                    value={form.values.name}
                    onChange={form.handleChange}
                    placeholder="Tên"
                    error={form.errors.name}
                />

                <Input
                    name="email"
                    value={form.values.email}
                    onChange={form.handleChange}
                    placeholder="Email"
                    error={form.errors.email}
                />

                <Button type="submit" loading={isPending}>
                    Thêm users
                </Button>
            </form>

            <Table<User>
                data={users}
                columns={[
                    { header: 'Tên', accessor: 'name' },
                    { header: 'Email', accessor: 'email' },
                    { header: 'Phone', accessor: 'phone' },
                ]}
            />
        </div>
    );
}
