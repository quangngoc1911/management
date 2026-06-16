'use client';

import { useEffect, useState } from 'react';
import { Loader2, Trash2, Plus, Users } from 'lucide-react';
import { familyMemberService } from '@/shared/lib/services/familyMemberService';
import type { FamilyMember, MemberRelationship } from '@/features/family/types/family';
import { RELATIONSHIP_TYPE_OPTIONS } from '@/features/family/utils/options';
import { Modal } from '@/components/Modal';
import { ErrorBanner } from '@/components/ErrorBanner';
import { useToast } from '@/shared/hooks/useToast';
import { useTranslation } from '@/shared/i18n/useTranslation';

interface MemberRelationshipsModalProps {
    open: boolean;
    onClose: () => void;
    memberId: string | null;
    memberName?: string;
}

export function MemberRelationshipsModal({ open, onClose, memberId, memberName }: MemberRelationshipsModalProps) {
    const { t } = useTranslation('family');
    const toast = useToast();
    const [relationships, setRelationships] = useState<MemberRelationship[]>([]);
    const [members, setMembers] = useState<FamilyMember[]>([]);
    const [loading, setLoading] = useState(false);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const [relatedMemberId, setRelatedMemberId] = useState('');
    const [relationshipType, setRelationshipType] = useState('');

    useEffect(() => {
        if (!open || !memberId) return;
        let active = true;
        setError(null);
        setLoading(true);
        setRelatedMemberId('');
        setRelationshipType('');
        Promise.all([
            familyMemberService.getRelationships(memberId),
            familyMemberService.getAll({ pageSize: 100, sortBy: 'fullName' }),
        ])
            .then(([rels, page]) => {
                if (!active) return;
                setRelationships(rels);
                setMembers(page.items.filter((m) => m.id !== memberId));
            })
            .catch((e) => {
                if (active) setError(e instanceof Error ? e.message : t('relationships.loadError'));
            })
            .finally(() => {
                if (active) setLoading(false);
            });
        return () => {
            active = false;
        };
    }, [open, memberId, t]);

    const refresh = async () => {
        if (!memberId) return;
        setRelationships(await familyMemberService.getRelationships(memberId));
    };

    const handleAdd = async () => {
        if (!memberId || !relatedMemberId || !relationshipType) {
            setError(t('relationships.selectRequired'));
            return;
        }
        setSubmitting(true);
        setError(null);
        try {
            await familyMemberService.createRelationship({
                memberId,
                relatedMemberId,
                relationshipType: Number(relationshipType),
            });
            setRelatedMemberId('');
            setRelationshipType('');
            await refresh();
            toast.success(t('relationships.added'));
        } catch (e) {
            setError(e instanceof Error ? e.message : t('relationships.addError'));
        } finally {
            setSubmitting(false);
        }
    };

    const handleDelete = async (id: string) => {
        setError(null);
        try {
            await familyMemberService.deleteRelationship(id);
            await refresh();
            toast.success(t('relationships.deleted'));
        } catch (e) {
            setError(e instanceof Error ? e.message : t('relationships.deleteError'));
        }
    };

    return (
        <Modal
            open={open}
            onClose={onClose}
            title={memberName ? t('relationships.titleNamed', { name: memberName }) : t('relationships.title')}
            size="lg"
        >
            <div className="mb-4">
                <ErrorBanner message={error} />
            </div>

            {/* Add form */}
            <div className="grid grid-cols-1 sm:grid-cols-[1fr_1fr_auto] gap-2 mb-4">
                <select
                    value={relatedMemberId}
                    onChange={(e) => setRelatedMemberId(e.target.value)}
                    className="input"
                >
                    <option value="">{t('relationships.relatedMember')}</option>
                    {members.map((m) => (
                        <option key={m.id} value={m.id}>
                            {m.fullName}
                        </option>
                    ))}
                </select>
                <select
                    value={relationshipType}
                    onChange={(e) => setRelationshipType(e.target.value)}
                    className="input"
                >
                    <option value="">{t('relationships.relationshipType')}</option>
                    {RELATIONSHIP_TYPE_OPTIONS.map((value) => (
                        <option key={value} value={value}>
                            {t(`relationshipType.${value}`)}
                        </option>
                    ))}
                </select>
                <button
                    onClick={handleAdd}
                    disabled={submitting}
                    className="flex items-center justify-center gap-2 px-4 py-2 text-sm font-medium text-white bg-primary hover:bg-primary-hover rounded-md transition disabled:opacity-50"
                >
                    {submitting ? <Loader2 className="w-4 h-4 animate-spin" /> : <Plus className="w-4 h-4" />}
                    {t('relationships.add')}
                </button>
            </div>

            {/* List */}
            {loading ? (
                <div className="flex items-center justify-center py-8 text-muted">
                    <Loader2 className="w-5 h-5 animate-spin mr-2" /> {t('relationships.loading')}
                </div>
            ) : relationships.length === 0 ? (
                <div className="flex flex-col items-center gap-2 py-8 text-muted">
                    <Users className="w-8 h-8" />
                    <span>{t('relationships.empty')}</span>
                </div>
            ) : (
                <div className="divide-y divide-border">
                    {relationships.map((rel) => (
                        <div key={rel.id} className="flex items-center justify-between py-3">
                            <div>
                                <p className="font-medium text-foreground">{rel.relatedMemberName}</p>
                                <span className="badge badge-info">
                                    {t(`relationshipType.${rel.relationshipType}`)}
                                </span>
                            </div>
                            <button
                                onClick={() => handleDelete(rel.id)}
                                className="p-1.5 text-muted hover:text-danger hover:bg-surface-alt rounded transition"
                                title={t('relationships.deleteTitle')}
                            >
                                <Trash2 className="w-4 h-4" />
                            </button>
                        </div>
                    ))}
                </div>
            )}
        </Modal>
    );
}

export default MemberRelationshipsModal;
