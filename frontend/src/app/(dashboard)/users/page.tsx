'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Plus, Edit, Trash2, Search, Loader2 } from 'lucide-react';
import {
    userService,
    User,
    CreateUserRequest,
    UpdateUserRequest,
} from '@/shared/lib/services/userService';
import { DataTable, PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';

const userSchema = z.object({
    email: z.string().email('Email không hợp lệ'),
    fullName: z.string().min(2, 'Họ tên phải có ít nhất 2 ký tự'),
    role: z.string().min(1, 'Vui lòng chọn vai trò'),
});

const createSchema = userSchema.extend({
    password: z.string().min(6, 'Mật khẩu phải có ít nhất 6 ký tự'),
});

type UserFormData = z.infer<typeof userSchema>;
type CreateFormData = z.infer<typeof createSchema>;

const columns = [
    { key: 'fullName', header: 'Họ tên', accessor: 'fullName' as keyof User },
    { key: 'email', header: 'Email', accessor: 'email' as keyof User },
    { key: 'role', header: 'Vai trò', accessor: 'role' as keyof User },
    { key: 'isActive', header: 'Trạng thái', accessor: 'isActive' as keyof User },
    { key: 'actions', header: 'Thao tác', accessor: 'actions' as keyof User },
];

const roleOptions = [
    { value: 'admin', label: 'Quản trị viên' },
    { value: 'editor', label: 'Biên tập viên' },
    { value: 'user', label: 'Người dùng' },
];

export default function UsersPage() {
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [total, setTotal] = useState(0);

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selectedUser, setSelectedUser] = useState<User | null>(null);
    const [submitting, setSubmitting] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<UserFormData>();
    const {
        register: registerCreate,
        handleSubmit: handleCreateSubmit,
        reset: resetCreate,
        formState: { errors: errorsCreate },
    } = useForm<CreateFormData>();

    const fetchUsers = async () => {
        setLoading(true);
        try {
            const data = await userService.getUsers(page, pageSize);
            setUsers(data.items);
            setTotal(data.total);
        } catch (error) {
            // Mock data
            setUsers([
                {
                    id: '1',
                    email: 'admin@example.com',
                    fullName: 'Admin User',
                    role: 'admin',
                    isActive: true,
                    createdAt: '2024-01-01',
                    updatedAt: '2024-01-01',
                },
                {
                    id: '2',
                    email: 'editor@example.com',
                    fullName: 'Editor User',
                    role: 'editor',
                    isActive: true,
                    createdAt: '2024-01-02',
                    updatedAt: '2024-01-02',
                },
                {
                    id: '3',
                    email: 'user@example.com',
                    fullName: 'Regular User',
                    role: 'user',
                    isActive: false,
                    createdAt: '2024-01-03',
                    updatedAt: '2024-01-03',
                },
            ]);
            setTotal(3);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, [page]);

    const handleCreate = async (data: CreateFormData) => {
        setSubmitting(true);
        try {
            await userService.createUser(data);
            setModalOpen(false);
            resetCreate();
            fetchUsers();
        } catch (error) {
            console.error('Create user error:', error);
        } finally {
            setSubmitting(false);
        }
    };

    const handleUpdate = async (data: UserFormData) => {
        if (!selectedUser) return;
        setSubmitting(true);
        try {
            await userService.updateUser(selectedUser.id, data);
            setModalOpen(false);
            setSelectedUser(null);
            reset();
            fetchUsers();
        } catch (error) {
            console.error('Update user error:', error);
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selectedUser) return;
        setSubmitting(true);
        try {
            await userService.deleteUser(selectedUser.id);
            setDeleteModalOpen(false);
            setSelectedUser(null);
            fetchUsers();
        } catch (error) {
            console.error('Delete user error:', error);
        } finally {
            setSubmitting(false);
        }
    };

    const openCreateModal = () => {
        setSelectedUser(null);
        reset();
        resetCreate();
        setModalOpen(true);
    };

    const openEditModal = (user: User) => {
        setSelectedUser(user);
        reset({ fullName: user.fullName, email: user.email, role: user.role });
        setModalOpen(true);
    };

    const openDeleteModal = (user: User) => {
        setSelectedUser(user);
        setDeleteModalOpen(true);
    };

    const tableData = users.map((user) => ({
        ...user,
        actions: (
            <div className="flex items-center gap-2">
                <button
                    onClick={() => openEditModal(user)}
                    className="p-1.5 text-muted hover:text-primary hover:bg-surface-alt rounded transition"
                    title="Sửa"
                >
                    <Edit className="w-4 h-4" />
                </button>
                <button
                    onClick={() => openDeleteModal(user)}
                    className="p-1.5 text-muted hover:text-danger hover:bg-surface-alt rounded transition"
                    title="Xóa"
                >
                    <Trash2 className="w-4 h-4" />
                </button>
            </div>
        ),
    }));

    return (
        <div className="space-y-6">
            {/* Page Header */}
            <div className="page-header">
                <div>
                    <h1 className="text-2xl font-bold text-foreground">Quản lý người dùng</h1>
                    <p className="text-muted mt-1">Quản lý tài khoản người dùng trong hệ thống</p>
                </div>
                <button
                    onClick={openCreateModal}
                    className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                >
                    <Plus className="w-4 h-4" />
                    Thêm người dùng
                </button>
            </div>

            {/* Search */}
            <div className="card p-4">
                <div className="relative max-w-md">
                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted" />
                    <input
                        type="text"
                        placeholder="Tìm kiếm người dùng..."
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        className="input pl-10"
                    />
                </div>
            </div>

            {/* Users Table */}
            {loading ? (
                <PageLoading />
            ) : (
                <PaginatedTable
                    data={tableData}
                    columns={columns}
                    page={page}
                    pageSize={pageSize}
                    total={total}
                    onPageChange={setPage}
                />
            )}

            {/* Create/Edit Modal */}
            <Modal
                open={modalOpen}
                onClose={() => {
                    setModalOpen(false);
                    setSelectedUser(null);
                }}
                title={selectedUser ? 'Sửa người dùng' : 'Thêm người dùng'}
                footer={
                    <>
                        <button
                            onClick={() => {
                                setModalOpen(false);
                                setSelectedUser(null);
                            }}
                            className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
                        >
                            Hủy
                        </button>
                        <button
                            onClick={() => {
                                if (selectedUser) {
                                    handleSubmit(handleUpdate)();
                                } else {
                                    handleCreateSubmit(handleCreate)();
                                }
                            }}
                            disabled={submitting}
                            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
                        >
                            {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
                            {selectedUser ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </>
                }
            >
                <form className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-foreground mb-2">
                            Họ tên
                        </label>
                        <input
                            {...register('fullName')}
                            className={`input ${errors.fullName ? 'input-error' : ''}`}
                            placeholder="Nhập họ tên"
                        />
                        {errors.fullName && (
                            <p className="text-danger text-xs mt-1">{errors.fullName.message}</p>
                        )}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-foreground mb-2">
                            Email
                        </label>
                        <input
                            {...register('email')}
                            type="email"
                            className={`input ${errors.email ? 'input-error' : ''}`}
                            placeholder="Nhập email"
                            disabled={!!selectedUser}
                        />
                        {errors.email && (
                            <p className="text-danger text-xs mt-1">{errors.email.message}</p>
                        )}
                    </div>

                    {!selectedUser && (
                        <div>
                            <label className="block text-sm font-medium text-foreground mb-2">
                                Mật khẩu
                            </label>
                            <input
                                {...registerCreate('password')}
                                type="password"
                                className={`input ${errorsCreate.password ? 'input-error' : ''}`}
                                placeholder="Nhập mật khẩu"
                            />
                            {errorsCreate.password && (
                                <p className="text-danger text-xs mt-1">
                                    {errorsCreate.password.message}
                                </p>
                            )}
                        </div>
                    )}

                    <div>
                        <label className="block text-sm font-medium text-foreground mb-2">
                            Vai trò
                        </label>
                        <select
                            {...(selectedUser ? register('role') : registerCreate('role'))}
                            className={`input ${(selectedUser ? errors.role : errorsCreate.role) ? 'input-error' : ''}`}
                        >
                            <option value="">Chọn vai trò</option>
                            {roleOptions.map((opt) => (
                                <option key={opt.value} value={opt.value}>
                                    {opt.label}
                                </option>
                            ))}
                        </select>
                        {(selectedUser ? errors.role : errorsCreate.role) && (
                            <p className="text-danger text-xs mt-1">
                                {selectedUser ? errors.role?.message : errorsCreate.role?.message}
                            </p>
                        )}
                    </div>
                </form>
            </Modal>

            {/* Delete Confirmation Modal */}
            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa người dùng"
                message={`Bạn có chắc chắn muốn xóa người dùng "${selectedUser?.fullName}" không? Hành động này không thể hoàn tác.`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
