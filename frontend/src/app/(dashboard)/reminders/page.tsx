'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { reminderService } from '@/shared/lib/services/reminderService';
import type { Reminder } from '@/shared/lib/services/reminderService';
import {
    reminderSchema,
    emptyReminderForm,
    toReminderRequest,
    type ReminderFormValues,
} from '@/features/reminders/utils/reminderForm';
import { getReminderColumns } from '@/features/reminders/utils/reminderColumns';
import { ReminderFormFields } from '@/features/reminders/components/ReminderFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function RemindersPage() {
    const toast = useToast();

    const list = usePaginatedList<Reminder>({
        fetcher: reminderService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách nhắc nhở',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Reminder | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<ReminderFormValues>({
        resolver: zodResolver(reminderSchema),
        defaultValues: emptyReminderForm,
    });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyReminderForm);
        setModalOpen(true);
    };

    const openEditModal = (reminder: Reminder) => {
        setSelected(reminder);
        reset({
            title: reminder.title,
            description: reminder.description ?? '',
            remindAt: reminder.remindAt ? reminder.remindAt.slice(0, 16) : emptyReminderForm.remindAt,
            status: String(reminder.status),
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getReminderColumns({
        onEdit: openEditModal,
        onDelete: (reminder) => {
            setSelected(reminder);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: ReminderFormValues) => {
        setSubmitting(true);
        try {
            const payload = toReminderRequest(values);
            if (selected) {
                await reminderService.update(selected.id, payload);
                toast.success('Cập nhật nhắc nhở thành công');
            } else {
                await reminderService.create(payload);
                toast.success('Tạo nhắc nhở thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu nhắc nhở thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await reminderService.remove(selected.id);
            toast.success('Xóa nhắc nhở thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa nhắc nhở thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Nhắc nhở"
                subtitle="Nhắc nhở cá nhân của bạn"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm nhắc nhở
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo tiêu đề..."
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
                    emptyText="Chưa có nhắc nhở nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa nhắc nhở' : 'Thêm nhắc nhở'}
                footer={
                    <>
                        <button
                            onClick={closeModal}
                            className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
                        >
                            Hủy
                        </button>
                        <button
                            onClick={handleSubmit(onSubmit)}
                            disabled={submitting}
                            className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
                        >
                            {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
                            {isEdit ? 'Cập nhật' : 'Tạo mới'}
                        </button>
                    </>
                }
            >
                <ReminderFormFields register={register} errors={errors} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa nhắc nhở"
                message={`Bạn có chắc chắn muốn xóa "${selected?.title}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
