'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { studyScheduleService } from '@/shared/lib/services/studyScheduleService';
import type { StudySchedule } from '@/shared/lib/services/studyScheduleService';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import {
    studyScheduleSchema,
    emptyStudyScheduleForm,
    toStudyScheduleRequest,
    toStudyScheduleFormValues,
    type StudyScheduleFormValues,
} from '@/features/education/utils/studyScheduleForm';
import { getStudyScheduleColumns } from '@/features/education/utils/studyScheduleColumns';
import { StudyScheduleFormFields } from '@/features/education/components/StudyScheduleFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function StudySchedulesPage() {
    const toast = useToast();

    const list = usePaginatedList<StudySchedule>({
        fetcher: studyScheduleService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách lịch học',
    });

    const [members, setMembers] = useState<FamilyMember[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<StudySchedule | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<StudyScheduleFormValues>({
        resolver: zodResolver(studyScheduleSchema),
        defaultValues: emptyStudyScheduleForm,
    });

    useEffect(() => {
        familyMemberService
            .getAll({ pageSize: 100, sortBy: 'fullName' })
            .then((d) => setMembers(d.items))
            .catch(() => setMembers([]));
    }, []);

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyStudyScheduleForm);
        setModalOpen(true);
    };

    const openEditModal = (schedule: StudySchedule) => {
        setSelected(schedule);
        reset(toStudyScheduleFormValues(schedule));
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getStudyScheduleColumns({
        onEdit: openEditModal,
        onDelete: (schedule) => {
            setSelected(schedule);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: StudyScheduleFormValues) => {
        setSubmitting(true);
        try {
            const payload = toStudyScheduleRequest(values);
            if (selected) {
                await studyScheduleService.update(selected.id, payload);
                toast.success('Cập nhật lịch học thành công');
            } else {
                await studyScheduleService.create(payload);
                toast.success('Thêm lịch học thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu lịch học thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await studyScheduleService.remove(selected.id);
            toast.success('Xóa lịch học thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa lịch học thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Lịch học"
                subtitle="Thời khoá biểu, lịch học của các thành viên"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm lịch học
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo tiêu đề, môn học..."
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
                    emptyText="Chưa có lịch học nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa lịch học' : 'Thêm lịch học'}
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
                <StudyScheduleFormFields register={register} errors={errors} members={members} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa lịch học"
                message={`Bạn có chắc chắn muốn xóa "${selected?.title}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
