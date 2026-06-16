'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { userService } from '@/shared/lib/services/userService';
import type { User } from '@/features/users/types/user';
import {
    buildUserSchema,
    emptyUserForm,
    toCreateUser,
    toUpdateUser,
    type UserFormValues,
} from '@/features/users/utils/userForm';
import { useUserColumns } from '@/features/users/utils/useUserColumns';
import { UserFormFields } from '@/features/users/components/UserFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';
import { useTranslation } from '@/shared/i18n/useTranslation';

export default function UsersPage() {
    const { t } = useTranslation('users');
    const { t: tc } = useTranslation('common');
    const toast = useToast();

    const list = usePaginatedList<User>({
        fetcher: userService.getAll,
        pageSize: 10,
        errorMessage: tc('error.load'),
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<User | null>(null);
    const [submitting, setSubmitting] = useState(false);
    const isEdit = !!selected;

    const schema = useMemo(() => buildUserSchema(t, isEdit), [t, isEdit]);
    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<UserFormValues>({ resolver: zodResolver(schema), defaultValues: emptyUserForm });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyUserForm);
        setModalOpen(true);
    };

    const openEditModal = (user: User) => {
        setSelected(user);
        reset({ fullName: user.fullName, email: user.email, role: user.role, password: '' });
        setModalOpen(true);
    };

    const openDeleteModal = (user: User) => {
        setSelected(user);
        setDeleteModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = useUserColumns({ onEdit: openEditModal, onDelete: openDeleteModal });

    const onSubmit = async (values: UserFormValues) => {
        setSubmitting(true);
        try {
            if (selected) {
                await userService.update(selected.id, toUpdateUser(values));
                toast.success(t('toast.updated'));
            } else {
                await userService.create(toCreateUser(values));
                toast.success(t('toast.created'));
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : tc('error.save'));
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await userService.remove(selected.id);
            toast.success(t('toast.deleted'));
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : tc('error.delete'));
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title={t('title')}
                subtitle={t('subtitle')}
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        {t('create')}
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder={t('searchPlaceholder')}
            />

            <ErrorBanner message={list.error} />

            {list.loading ? (
                <PageLoading />
            ) : (
                <PaginatedTable
                    data={list.items}
                    columns={columns}
                    page={list.page}
                    pageSize={list.pageSize}
                    total={list.total}
                    onPageChange={list.setPage}
                    emptyText={t('empty')}
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? t('form.editTitle') : t('form.createTitle')}
                footer={
                    <>
                        <button
                            onClick={closeModal}
                            className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
                        >
                            {tc('actions.cancel')}
                        </button>
                        <button
                            onClick={handleSubmit(onSubmit)}
                            disabled={submitting}
                            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
                        >
                            {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
                            {isEdit ? tc('actions.update') : tc('actions.create')}
                        </button>
                    </>
                }
            >
                <UserFormFields register={register} errors={errors} isEdit={isEdit} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title={t('delete.title')}
                message={`${t('delete.message', { name: selected?.fullName ?? '' })} ${tc('confirm.cannotUndo')}`}
                confirmText={tc('actions.delete')}
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
