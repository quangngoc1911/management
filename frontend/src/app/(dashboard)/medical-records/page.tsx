'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { medicalRecordService } from '@/shared/lib/services/medicalRecordService';
import type { MedicalRecord } from '@/shared/lib/services/medicalRecordService';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import {
    medicalRecordSchema,
    emptyMedicalRecordForm,
    toMedicalRecordRequest,
    type MedicalRecordFormValues,
} from '@/features/health/utils/medicalRecordForm';
import { getMedicalRecordColumns } from '@/features/health/utils/medicalRecordColumns';
import { MedicalRecordFormFields } from '@/features/health/components/MedicalRecordFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function MedicalRecordsPage() {
    const toast = useToast();

    const list = usePaginatedList<MedicalRecord>({
        fetcher: medicalRecordService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách hồ sơ y tế',
    });

    const [members, setMembers] = useState<FamilyMember[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<MedicalRecord | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<MedicalRecordFormValues>({
        resolver: zodResolver(medicalRecordSchema),
        defaultValues: emptyMedicalRecordForm,
    });

    useEffect(() => {
        familyMemberService
            .getAll({ pageSize: 100, sortBy: 'fullName' })
            .then((d) => setMembers(d.items))
            .catch(() => setMembers([]));
    }, []);

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyMedicalRecordForm);
        setModalOpen(true);
    };

    const openEditModal = (r: MedicalRecord) => {
        setSelected(r);
        reset({
            memberId: r.memberId,
            recordType: r.recordType,
            title: r.title,
            recordDate: r.recordDate ? r.recordDate.slice(0, 10) : new Date().toISOString().slice(0, 10),
            followUpDate: r.followUpDate ? r.followUpDate.slice(0, 10) : '',
            diagnosis: r.diagnosis ?? '',
            treatment: r.treatment ?? '',
            doctorName: r.doctorName ?? '',
            hospitalName: r.hospitalName ?? '',
            isPrivate: r.isPrivate,
            notes: r.notes ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getMedicalRecordColumns({
        onEdit: openEditModal,
        onDelete: (r) => {
            setSelected(r);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: MedicalRecordFormValues) => {
        setSubmitting(true);
        try {
            const payload = toMedicalRecordRequest(values);
            if (selected) {
                await medicalRecordService.update(selected.id, payload);
                toast.success('Cập nhật hồ sơ y tế thành công');
            } else {
                await medicalRecordService.create(payload);
                toast.success('Thêm hồ sơ y tế thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu hồ sơ y tế thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await medicalRecordService.remove(selected.id);
            toast.success('Xóa hồ sơ y tế thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa hồ sơ y tế thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Hồ sơ y tế"
                subtitle="Lịch sử khám chữa bệnh của các thành viên"
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
                placeholder="Tìm theo tiêu đề, chẩn đoán..."
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
                    emptyText="Chưa có hồ sơ y tế nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa hồ sơ y tế' : 'Thêm hồ sơ y tế'}
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
                <MedicalRecordFormFields register={register} errors={errors} members={members} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa hồ sơ y tế"
                message={`Bạn có chắc chắn muốn xóa "${selected?.title}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
