'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { Loader2 } from 'lucide-react';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { UpsertMemberProfileRequest } from '@/features/family/types/family';
import { MARITAL_STATUS_OPTIONS, EDUCATION_LEVEL_OPTIONS } from '@/features/family/utils/options';
import { Modal } from '@/components/Modal';
import { FormField } from '@/components/FormField';
import { ErrorBanner } from '@/components/ErrorBanner';
import { useToast } from '@/shared/hooks/useToast';
import { useTranslation } from '@/shared/i18n/useTranslation';

type ProfileFormData = {
    nationalId?: string;
    passportNo?: string;
    nationality?: string;
    ethnicity?: string;
    religion?: string;
    bloodType?: string;
    maritalStatus?: string;
    educationLevel?: string;
    occupation?: string;
    birthPlace?: string;
    currentAddress?: string;
    permanentAddress?: string;
    heightCm?: string;
    weightKg?: string;
    emergencyContactName?: string;
    emergencyContactPhone?: string;
    bio?: string;
};

const EMPTY: ProfileFormData = {
    nationalId: '', passportNo: '', nationality: '', ethnicity: '', religion: '', bloodType: '',
    maritalStatus: '', educationLevel: '', occupation: '', birthPlace: '', currentAddress: '',
    permanentAddress: '', heightCm: '', weightKg: '', emergencyContactName: '', emergencyContactPhone: '', bio: '',
};

const str = (v?: string) => (v && v.trim() !== '' ? v.trim() : null);
const num = (v?: string) => (v && v.trim() !== '' ? Number(v) : null);

interface MemberProfileModalProps {
    open: boolean;
    onClose: () => void;
    memberId: string | null;
    memberName?: string;
}

export function MemberProfileModal({ open, onClose, memberId, memberName }: MemberProfileModalProps) {
    const { t } = useTranslation('family');
    const toast = useToast();
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const { register, handleSubmit, reset } = useForm<ProfileFormData>({ defaultValues: EMPTY });

    useEffect(() => {
        if (!open || !memberId) return;
        let active = true;
        setError(null);
        setLoading(true);
        familyMemberService
            .getProfile(memberId)
            .then((p) => {
                if (!active) return;
                reset(
                    p
                        ? {
                              nationalId: p.nationalId ?? '',
                              passportNo: p.passportNo ?? '',
                              nationality: p.nationality ?? '',
                              ethnicity: p.ethnicity ?? '',
                              religion: p.religion ?? '',
                              bloodType: p.bloodType ?? '',
                              maritalStatus: p.maritalStatus != null ? String(p.maritalStatus) : '',
                              educationLevel: p.educationLevel != null ? String(p.educationLevel) : '',
                              occupation: p.occupation ?? '',
                              birthPlace: p.birthPlace ?? '',
                              currentAddress: p.currentAddress ?? '',
                              permanentAddress: p.permanentAddress ?? '',
                              heightCm: p.heightCm != null ? String(p.heightCm) : '',
                              weightKg: p.weightKg != null ? String(p.weightKg) : '',
                              emergencyContactName: p.emergencyContactName ?? '',
                              emergencyContactPhone: p.emergencyContactPhone ?? '',
                              bio: p.bio ?? '',
                          }
                        : EMPTY,
                );
            })
            .catch((e) => {
                if (active) setError(e instanceof Error ? e.message : t('profile.loadError'));
            })
            .finally(() => {
                if (active) setLoading(false);
            });
        return () => {
            active = false;
        };
    }, [open, memberId, reset, t]);

    const onSubmit = async (data: ProfileFormData) => {
        if (!memberId) return;
        setSubmitting(true);
        setError(null);
        const payload: UpsertMemberProfileRequest = {
            nationalId: str(data.nationalId),
            passportNo: str(data.passportNo),
            nationality: str(data.nationality),
            ethnicity: str(data.ethnicity),
            religion: str(data.religion),
            bloodType: str(data.bloodType),
            maritalStatus: data.maritalStatus ? Number(data.maritalStatus) : null,
            educationLevel: data.educationLevel ? Number(data.educationLevel) : null,
            occupation: str(data.occupation),
            birthPlace: str(data.birthPlace),
            currentAddress: str(data.currentAddress),
            permanentAddress: str(data.permanentAddress),
            heightCm: num(data.heightCm),
            weightKg: num(data.weightKg),
            emergencyContactName: str(data.emergencyContactName),
            emergencyContactPhone: str(data.emergencyContactPhone),
            bio: str(data.bio),
        };
        try {
            await familyMemberService.upsertProfile(memberId, payload);
            toast.success(t('profile.saved'));
            onClose();
        } catch (e) {
            setError(e instanceof Error ? e.message : t('profile.saveError'));
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <Modal
            open={open}
            onClose={onClose}
            title={memberName ? t('profile.titleNamed', { name: memberName }) : t('profile.title')}
            size="xl"
            footer={
                <>
                    <button
                        onClick={onClose}
                        className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
                    >
                        {t('common:actions.cancel')}
                    </button>
                    <button
                        onClick={handleSubmit(onSubmit)}
                        disabled={submitting || loading}
                        className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
                    >
                        {submitting && <Loader2 className="w-4 h-4 animate-spin" />}
                        {t('profile.save')}
                    </button>
                </>
            }
        >
            <ErrorBanner message={error} />

            {loading ? (
                <div className="flex items-center justify-center py-10 text-muted">
                    <Loader2 className="w-5 h-5 animate-spin mr-2" /> {t('profile.loading')}
                </div>
            ) : (
                <form className="grid grid-cols-1 md:grid-cols-2 gap-4" onSubmit={handleSubmit(onSubmit)}>
                    <FormField label={t('profile.fields.nationalId')}>
                        <input {...register('nationalId')} className="input" placeholder={t('profile.encryptedHint')} />
                    </FormField>
                    <FormField label={t('profile.fields.passportNo')}>
                        <input {...register('passportNo')} className="input" placeholder={t('profile.encryptedHint')} />
                    </FormField>

                    <FormField label={t('profile.fields.nationality')}>
                        <input {...register('nationality')} className="input" />
                    </FormField>
                    <FormField label={t('profile.fields.ethnicity')}>
                        <input {...register('ethnicity')} className="input" />
                    </FormField>

                    <FormField label={t('profile.fields.religion')}>
                        <input {...register('religion')} className="input" />
                    </FormField>
                    <FormField label={t('profile.fields.bloodType')}>
                        <input {...register('bloodType')} className="input" placeholder="A, B, O, AB..." />
                    </FormField>

                    <FormField label={t('profile.fields.maritalStatus')}>
                        <select {...register('maritalStatus')} className="input">
                            <option value="">{t('form.select')}</option>
                            {MARITAL_STATUS_OPTIONS.map((value) => (
                                <option key={value} value={value}>
                                    {t(`maritalStatus.${value}`)}
                                </option>
                            ))}
                        </select>
                    </FormField>
                    <FormField label={t('profile.fields.educationLevel')}>
                        <select {...register('educationLevel')} className="input">
                            <option value="">{t('form.select')}</option>
                            {EDUCATION_LEVEL_OPTIONS.map((value) => (
                                <option key={value} value={value}>
                                    {t(`educationLevel.${value}`)}
                                </option>
                            ))}
                        </select>
                    </FormField>

                    <FormField label={t('profile.fields.occupation')}>
                        <input {...register('occupation')} className="input" />
                    </FormField>
                    <FormField label={t('profile.fields.birthPlace')}>
                        <input {...register('birthPlace')} className="input" />
                    </FormField>

                    <FormField label={t('profile.fields.heightCm')}>
                        <input {...register('heightCm')} type="number" step="0.1" className="input" />
                    </FormField>
                    <FormField label={t('profile.fields.weightKg')}>
                        <input {...register('weightKg')} type="number" step="0.1" className="input" />
                    </FormField>

                    <FormField label={t('profile.fields.emergencyContactName')}>
                        <input
                            {...register('emergencyContactName')}
                            className="input"
                            placeholder={t('profile.fields.emergencyContactNamePlaceholder')}
                        />
                    </FormField>
                    <FormField label={t('profile.fields.emergencyContactPhone')}>
                        <input {...register('emergencyContactPhone')} className="input" />
                    </FormField>

                    <FormField label={t('profile.fields.currentAddress')} full>
                        <input {...register('currentAddress')} className="input" />
                    </FormField>
                    <FormField label={t('profile.fields.permanentAddress')} full>
                        <input {...register('permanentAddress')} className="input" />
                    </FormField>

                    <FormField label={t('profile.fields.bio')} full>
                        <textarea {...register('bio')} rows={3} className="input" />
                    </FormField>
                </form>
            )}
        </Modal>
    );
}

export default MemberProfileModal;
