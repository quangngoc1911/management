'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { medicationService } from '@/shared/lib/services/medicationService';
import type { Medication } from '@/shared/lib/services/medicationService';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import {
    medicationSchema,
    emptyMedicationForm,
    toMedicationRequest,
    type MedicationFormValues,
} from '@/features/health/utils/medicationForm';
import { getMedicationColumns } from '@/features/health/utils/medicationColumns';
import { MedicationFormFields } from '@/features/health/components/MedicationFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function MedicationsPage() {
    const toast = useToast();

    const list = usePaginatedList<Medication>({
        fetcher: medicationService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách thuốc',
    });

    const [members, setMembers] = useState<FamilyMember[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Medication | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<MedicationFormValues>({
        resolver: zodResolver(medicationSchema),
        defaultValues: emptyMedicationForm,
    });

    useEffect(() => {
        familyMemberService
            .getAll({ pageSize: 100, sortBy: 'fullName' })
            .then((d) => setMembers(d.items))
            .catch(() => setMembers([]));
    }, []);

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyMedicationForm);
        setModalOpen(true);
    };

    const openEditModal = (m: Medication) => {
        setSelected(m);
        reset({
            memberId: m.memberId,
            name: m.name,
            dosage: m.dosage ?? '',
            frequency: m.frequency ?? '',
            startDate: m.startDate ? m.startDate.slice(0, 10) : new Date().toISOString().slice(0, 10),
            endDate: m.endDate ? m.endDate.slice(0, 10) : '',
            isActive: m.isActive,
            notes: m.notes ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getMedicationColumns({
        onEdit: openEditModal,
        onDelete: (m) => {
            setSelected(m);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: MedicationFormValues) => {
        setSubmitting(true);
        try {
            const payload = toMedicationRequest(values);
            if (selected) {
                await medicationService.update(selected.id, payload);
                toast.success('Cập nhật thuốc thành công');
            } else {
                await medicationService.create(payload);
                toast.success('Thêm thuốc thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu thuốc thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await medicationService.remove(selected.id);
            toast.success('Xóa thuốc thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa thuốc thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Thuốc"
                subtitle="Danh sách thuốc đang/đã sử dụng"
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm thuốc
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm theo tên thuốc..."
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
                    emptyText="Chưa có thuốc nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa thuốc' : 'Thêm thuốc'}
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
                <MedicationFormFields register={register} errors={errors} members={members} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa thuốc"
                message={`Bạn có chắc chắn muốn xóa "${selected?.name}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
