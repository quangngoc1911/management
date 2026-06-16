'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Plus, Loader2 } from 'lucide-react';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { FamilyMember } from '@/features/family/types/family';
import {
    buildMemberSchema,
    emptyMemberForm,
    toMemberRequest,
    type MemberFormValues,
} from '@/features/family/utils/familyForm';
import { useFamilyColumns } from '@/features/family/utils/useFamilyColumns';
import { FamilyMemberFormFields } from '@/features/family/components/FamilyMemberFormFields';
import { MemberProfileModal } from '@/features/family/components/MemberProfileModal';
import { MemberRelationshipsModal } from '@/features/family/components/MemberRelationshipsModal';
import { PaginatedTable } from '@/components/DataTable';
import { Modal, ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { SearchInput } from '@/components/SearchInput';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';
import { useTranslation } from '@/shared/i18n/useTranslation';

export default function FamilyMembersPage() {
    const { t } = useTranslation('family');
    const { t: tc } = useTranslation('common');
    const toast = useToast();

    const list = usePaginatedList<FamilyMember>({
        fetcher: familyMemberService.getAll,
        pageSize: 10,
        errorMessage: tc('error.load'),
    });

    const [modalOpen, setModalOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<FamilyMember | null>(null);
    const [submitting, setSubmitting] = useState(false);

    const [profileMember, setProfileMember] = useState<FamilyMember | null>(null);
    const [profileOpen, setProfileOpen] = useState(false);
    const [relMember, setRelMember] = useState<FamilyMember | null>(null);
    const [relOpen, setRelOpen] = useState(false);

    const isEdit = !!selected;

    const schema = useMemo(() => buildMemberSchema(t), [t]);
    const {
        register,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<MemberFormValues>({ resolver: zodResolver(schema), defaultValues: emptyMemberForm });

    const openCreateModal = () => {
        setSelected(null);
        reset(emptyMemberForm);
        setModalOpen(true);
    };

    const openEditModal = (member: FamilyMember) => {
        setSelected(member);
        reset({
            fullName: member.fullName,
            nickname: member.nickname ?? '',
            gender: member.gender != null ? String(member.gender) : '',
            relationToHead: member.relationToHead != null ? String(member.relationToHead) : '',
            dateOfBirth: member.dateOfBirth ?? '',
            phone: member.phone ?? '',
            email: member.email ?? '',
            isHouseholdHead: member.isHouseholdHead,
            notes: member.notes ?? '',
        });
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setSelected(null);
    };

    const columns = useFamilyColumns({
        onProfile: (member) => {
            setProfileMember(member);
            setProfileOpen(true);
        },
        onRelationships: (member) => {
            setRelMember(member);
            setRelOpen(true);
        },
        onEdit: openEditModal,
        onDelete: (member) => {
            setSelected(member);
            setDeleteModalOpen(true);
        },
    });

    const onSubmit = async (values: MemberFormValues) => {
        setSubmitting(true);
        try {
            const payload = toMemberRequest(values);
            if (selected) {
                await familyMemberService.update(selected.id, payload);
                toast.success(t('toast.updated'));
            } else {
                await familyMemberService.create(payload);
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
            await familyMemberService.remove(selected.id);
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
                size="lg"
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
                <FamilyMemberFormFields register={register} errors={errors} />
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

            <MemberProfileModal
                open={profileOpen}
                onClose={() => {
                    setProfileOpen(false);
                    setProfileMember(null);
                }}
                memberId={profileMember?.id ?? null}
                memberName={profileMember?.fullName}
            />

            <MemberRelationshipsModal
                open={relOpen}
                onClose={() => {
                    setRelOpen(false);
                    setRelMember(null);
                }}
                memberId={relMember?.id ?? null}
                memberName={relMember?.fullName}
            />
        </div>
    );
}
