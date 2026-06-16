import { api } from '../axiosInstance';
import { createCrudService, unwrap } from './createCrudService';
import type {
    CreateFamilyMemberRequest,
    CreateMemberRelationshipRequest,
    FamilyMember,
    FamilyMemberQuery,
    MemberProfile,
    MemberRelationship,
    UpdateFamilyMemberRequest,
    UpdateMemberRelationshipRequest,
    UpsertMemberProfileRequest,
} from '@/features/family/types/family';

// Types live in the feature layer; re-exported here for existing import sites
// (family page + member modals + medical/education/health/medication/study pages).
export type {
    FamilyMember,
    CreateFamilyMemberRequest,
    UpdateFamilyMemberRequest,
    FamilyMemberQuery,
    PaginatedFamilyMembers,
    MemberProfile,
    UpsertMemberProfileRequest,
    MemberRelationship,
    CreateMemberRelationshipRequest,
    UpdateMemberRelationshipRequest,
} from '@/features/family/types/family';

// Backend route: api/[controller] -> api/FamilyMembers.
const BASE = '/FamilyMembers';
const RELATIONSHIP_BASE = '/member-relationships';

// Core CRUD comes from the shared factory; the 1:1 profile and relationship
// sub-resources are added on top.
const base = createCrudService<
    FamilyMember,
    CreateFamilyMemberRequest,
    UpdateFamilyMemberRequest,
    FamilyMemberQuery
>(BASE);

export const familyMemberService = {
    ...base,

    // 1:1 detailed profile. Returns null when the member has no profile yet (backend 404).
    getProfile: async (memberId: string): Promise<MemberProfile | null> => {
        try {
            const res = await api.get(`${BASE}/${memberId}/profile`);
            return res.data.success ? res.data.data : null;
        } catch (err: unknown) {
            const status = (err as { response?: { status?: number } })?.response?.status;
            if (status === 404) return null;
            throw err;
        }
    },

    upsertProfile: async (memberId: string, data: UpsertMemberProfileRequest): Promise<MemberProfile> =>
        unwrap<MemberProfile>(await api.put(`${BASE}/${memberId}/profile`, data)),

    getRelationships: async (memberId: string): Promise<MemberRelationship[]> =>
        unwrap<MemberRelationship[]>(await api.get(RELATIONSHIP_BASE, { params: { memberId } })),

    createRelationship: async (data: CreateMemberRelationshipRequest): Promise<MemberRelationship> =>
        unwrap<MemberRelationship>(await api.post(RELATIONSHIP_BASE, data)),

    updateRelationship: async (
        id: string,
        data: UpdateMemberRelationshipRequest,
    ): Promise<MemberRelationship> =>
        unwrap<MemberRelationship>(await api.put(`${RELATIONSHIP_BASE}/${id}`, data)),

    deleteRelationship: async (id: string): Promise<void> => {
        const res = await api.delete(`${RELATIONSHIP_BASE}/${id}`);
        if (!res.data?.success) throw new Error(res.data?.message || 'Request failed');
    },
};

export default familyMemberService;
