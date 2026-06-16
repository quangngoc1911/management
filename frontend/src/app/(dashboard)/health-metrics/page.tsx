'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { healthMetricService } from '@/shared/lib/services/healthMetricService';
import type { HealthMetric } from '@/shared/lib/services/healthMetricService';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { FamilyMember } from '@/shared/lib/services/familyMemberService';
import {
    healthMetricSchema,
    emptyHealthMetricForm,
    toHealthMetricRequest,
    type HealthMetricFormValues,
} from '@/features/health/utils/healthMetricForm';
import { getHealthMetricColumns } from '@/features/health/utils/healthMetricColumns';
import { HealthMetricFormFields } from '@/features/health/components/HealthMetricFormFields';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function HealthMetricsPage() {
    const toast = useToast();

    const list = usePaginatedList<HealthMetric>({
        fetcher: healthMetricService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải danh sách chỉ số sức khỏe',
    });

    const [members, setMembers] = useState<FamilyMember[]>([]);
    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<HealthMetric | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const isEdit = !!selected;

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<HealthMetricFormValues>({
        resolver: zodResolver(healthMetricSchema),
        defaultValues: emptyHealthMetricForm,
    });

    useEffect(() => {
        familyMemberService
            .getAll({ pageSize: 100, sortBy: 'fullName' })
            .then((d) => setMembers(d.items))
            .catch(() => setMembers([]));
    }, []);

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyHealthMetricForm);
        setModalOpen(true);
    };

    const openEditModal = (h: HealthMetric) => {
        setSelected(h);
        reset({
            memberId: h.memberId,
            metricType: h.metricType,
            value: String(h.value),
            unit: h.unit,
            measuredAt: h.measuredAt ? h.measuredAt.slice(0, 16) : new Date().toISOString().slice(0, 16),
            notes: h.notes ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = getHealthMetricColumns({
        onEdit: openEditModal,
        onDelete: (h) => {
            setSelected(h);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: HealthMetricFormValues) => {
        setSubmitting(true);
        try {
            const payload = toHealthMetricRequest(values);
            if (selected) {
                await healthMetricService.update(selected.id, payload);
                toast.success('Cập nhật chỉ số thành công');
            } else {
                await healthMetricService.create(payload);
                toast.success('Thêm chỉ số thành công');
            }
            closeModal();
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Lưu chỉ số thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        setSubmitting(true);
        try {
            await healthMetricService.remove(selected.id);
            toast.success('Xóa chỉ số thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa chỉ số thất bại');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader
                title="Chỉ số sức khỏe"
                subtitle="Theo dõi cân nặng, huyết áp, đường huyết..."
                action={
                    <button
                        onClick={openCreateModal}
                        className="flex items-center gap-2 bg-primary hover:bg-primary-hover text-white px-4 py-2 rounded-md font-medium transition"
                    >
                        <Plus className="w-4 h-4" />
                        Thêm chỉ số
                    </button>
                }
            />

            <SearchInput
                value={list.search}
                onChange={list.setSearch}
                placeholder="Tìm kiếm chỉ số..."
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
                    emptyText="Chưa có chỉ số nào"
                />
            )}

            <Modal
                open={modalOpen}
                onClose={closeModal}
                title={isEdit ? 'Sửa chỉ số' : 'Thêm chỉ số'}
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
                <HealthMetricFormFields register={register} errors={errors} members={members} />
            </Modal>

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa chỉ số"
                message="Bạn có chắc chắn muốn xóa chỉ số này không?"
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
