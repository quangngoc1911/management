'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { educationRecordService } from '@/shared/lib/services/educationRecordService';
import type { EducationRecord } from '@/shared/lib/services/educationRecordService';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import {
    educationRecordSchema,
    emptyEducationRecordForm,
    toEducationRecordRequest,
    toEducationRecordFormValues,
    type EducationRecordFormValues,
} from '@/features/education/utils/educationRecordForm';
import { getEducationRecordColumns } from '@/features/education/utils/educationRecordColumns';
import { EducationRecordFormFields } from '@/features/education/components/EducationRecordFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function EducationRecordsPage() {
    const toast = useToast();

    const list = usePaginatedList<EducationRecord>({
        fetcher: educationRecordService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách hồ sơ học tập',
    });

    const [members, setMembers] = useState<FamilyMember[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<EducationRecord | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<EducationRecordFormValues>({
        resolver: zodResolver(educationRecordSchema),
        defaultValues: emptyEducationRecordForm,
    });

    useEffect(() => {
        familyMemberService
            .getAll({ pageSize: 100, sortBy: 'fullName' })
            .then((d) => setMembers(d.items))
            .catch(() => setMembers([]));
    }, []);

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyEducationRecordForm);
        setModalOpen(true);
    };

    const openEditModal = (record: EducationRecord) => {
        setSelected(record);
        reset(toEducationRecordFormValues(record));
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getEducationRecordColumns({
        onEdit: openEditModal,
        onDelete: (record) => {
            setSelected(record);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: EducationRecordFormValues) => {
        setSubmitting(true);
        try {
            const payload = toEducationRecordRequest(values);
            if (selected) {
                await educationRecordService.update(selected.id, payload);
                toast.success('Cập nhật hồ sơ học tập thành công');
            } else {
                await educationRecordService.create(payload);
                toast.success('Thêm hồ sơ học tập thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu hồ sơ học tập thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await educationRecordService.remove(selected.id);
            toast.success('Xóa hồ sơ học tập thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa hồ sơ học tập thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Hồ sơ học tập"
                subtitle="Quá trình học tập của các thành viên"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm hồ sơ
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo cơ sở, chuyên ngành..."
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
                    emptyText="Chưa có hồ sơ học tập nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa hồ sơ học tập' : 'Thêm hồ sơ học tập'}
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
                <EducationRecordFormFields
                    register={register}
                    errors={errors}
                    members={members}
                />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa hồ sơ học tập"
                message={`Bạn có chắc chắn muốn xóa hồ sơ tại "${selected?.institutionName}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
