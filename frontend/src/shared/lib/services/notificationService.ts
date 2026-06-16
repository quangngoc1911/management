import { api } from '../axiosInstance';
import { createCrudService, unwrap, type ListParams } from './createCrudService';

export interface Notification {
    id: string;
    userId: string;
    title: string;
    body?: string | null;
    type: string;
    channel: string;
    entityType?: string | null;
    entityId?: string | null;
    isRead: boolean;
    readAt?: string | null;
    sentAt?: string | null;
    createdAt?: string | null;
}

export interface NotificationQuery extends ListParams {
    isRead?: boolean;
    type?: string;
    page?: number;
    pageSize?: number;
}

export interface PaginatedNotifications {
    items: Notification[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

const BASE = '/notifications';

export const notificationService = {
    ...createCrudService<Notification, never, never, NotificationQuery>(BASE),

    setRead: async (id: string, isRead: boolean): Promise<Notification> =>
        unwrap(await api.put(`${BASE}/${id}`, { isRead })),

    remove: async (id: string): Promise<void> => {
        const res = await api.delete(`${BASE}/${id}`);
        if (!res.data?.success) throw new Error(res.data?.message || 'Request failed');
    },
};

export default notificationService;
