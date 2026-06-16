'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { familyEventService } from '@/shared/lib/services/familyEventService';
import type { FamilyEvent } from '@/shared/lib/services/familyEventService';
import {
    eventSchema,
    emptyEventForm,
    toEventRequest,
    type EventFormValues,
} from '@/features/events/utils/eventForm';
import { getEventColumns } from '@/features/events/utils/eventColumns';
import { EventFormFields } from '@/features/events/components/EventFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function FamilyEventsPage() {
    const toast = useToast();

    const list = usePaginatedList<FamilyEvent>({
        fetcher: familyEventService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách sự kiện',
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<FamilyEvent | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<EventFormValues>({
        resolver: zodResolver(eventSchema),
        defaultValues: emptyEventForm,
    });

    const nowLocal = () => new Date().toISOString().slice(0, 16);

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyEventForm);
        setModalOpen(true);
    };

    const openEditModal = (event: FamilyEvent) => {
        setSelected(event);
        reset({
            title: event.title,
            eventType: event.eventType ?? '',
            startAt: event.startAt ? event.startAt.slice(0, 16) : nowLocal(),
            endAt: event.endAt ? event.endAt.slice(0, 16) : '',
            allDay: event.allDay,
            location: event.location ?? '',
            status: String(event.status),
            description: event.description ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getEventColumns({
        onEdit: openEditModal,
        onDelete: (event) => {
            setSelected(event);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: EventFormValues) => {
        setSubmitting(true);
        try {
            const payload = toEventRequest(values);
            if (selected) {
                await familyEventService.update(selected.id, payload);
                toast.success('Cập nhật sự kiện thành công');
            } else {
                await familyEventService.create(payload);
                toast.success('Thêm sự kiện thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu sự kiện thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await familyEventService.remove(selected.id);
            toast.success('Xóa sự kiện thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa sự kiện thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Sự kiện gia đình"
                subtitle="Sinh nhật, kỷ niệm, họp mặt, du lịch..."
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm sự kiện
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo tiêu đề, mô tả..."
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
                    emptyText="Chưa có sự kiện nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa sự kiện' : 'Thêm sự kiện'}
                size="lg"
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
                <EventFormFields register={register} errors={errors} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa sự kiện"
                message={`Bạn có chắc chắn muốn xóa "${selected?.title}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
