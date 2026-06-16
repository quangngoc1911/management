import type { ListParams, Paginated } from '@/shared/lib/services/createCrudService';

export interface FamilyMember {
    id: string;
    userId?: string | null;
    fullName: string;
    nickname?: string | null;
    dateOfBirth?: string | null; // yyyy-MM-dd
    dateOfDeath?: string | null;
    gender?: number | null;
    avatarUrl?: string | null;
    phone?: string | null;
    email?: string | null;
    relationToHead?: number | null;
    isHouseholdHead: boolean;
    notes?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateFamilyMemberRequest {
    userId?: string | null;
    fullName: string;
    nickname?: string | null;
    dateOfBirth?: string | null;
    dateOfDeath?: string | null;
    gender?: number | null;
    avatarUrl?: string | null;
    phone?: string | null;
    email?: string | null;
    relationToHead?: number | null;
    isHouseholdHead: boolean;
    notes?: string | null;
}

export type UpdateFamilyMemberRequest = CreateFamilyMemberRequest;

export interface FamilyMemberQuery extends ListParams {
    gender?: number;
    relationToHead?: number;
    isHouseholdHead?: boolean;
}

export type PaginatedFamilyMembers = Paginated<FamilyMember>;

export interface MemberProfile {
    id: string;
    memberId: string;
    nationalId?: string | null;
    passportNo?: string | null;
    nationality?: string | null;
    ethnicity?: string | null;
    religion?: string | null;
    bloodType?: string | null;
    maritalStatus?: number | null;
    educationLevel?: number | null;
    occupation?: string | null;
    birthPlace?: string | null;
    currentAddress?: string | null;
    permanentAddress?: string | null;
    heightCm?: number | null;
    weightKg?: number | null;
    emergencyContactName?: string | null;
    emergencyContactPhone?: string | null;
    bio?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export type UpsertMemberProfileRequest = Omit<
    MemberProfile,
    'id' | 'memberId' | 'createdAt' | 'updatedAt'
>;

export interface MemberRelationship {
    id: string;
    memberId: string;
    memberName: string;
    relatedMemberId: string;
    relatedMemberName: string;
    relationshipType: number;
    startedAt?: string | null;
    endedAt?: string | null;
    isBiological?: boolean | null;
    notes?: string | null;
    createdAt?: string | null;
    updatedAt?: string | null;
}

export interface CreateMemberRelationshipRequest {
    memberId: string;
    relatedMemberId: string;
    relationshipType: number;
    startedAt?: string | null;
    endedAt?: string | null;
    isBiological?: boolean | null;
    notes?: string | null;
}

export interface UpdateMemberRelationshipRequest {
    relationshipType: number;
    startedAt?: string | null;
    endedAt?: string | null;
    isBiological?: boolean | null;
    notes?: string | null;
}
