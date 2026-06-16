'use client';

import { useState } from 'react';
import { notificationService } from '@/shared/lib/services/notificationService';
import type { Notification } from '@/shared/lib/services/notificationService';
import { getNotificationColumns } from '@/features/system/utils/notificationColumns';
import { PaginatedTable } from '@/components/DataTable';
import { ConfirmModal } from '@/components/Modal';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';
import { useToast } from '@/shared/hooks/useToast';

export default function NotificationsPage() {
    const toast = useToast();

    const list = usePaginatedList<Notification>({
        fetcher: notificationService.getAll,
        pageSize: 10,
        errorMessage: 'Không thể tải thông báo',
    });

    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    const [selected, setSelected] = useState<Notification | null>(null);

    const columns = getNotificationColumns({
        onToggleRead: async (n) => {
            try {
                await notificationService.setRead(n.id, !n.isRead);
                toast.success(n.isRead ? 'Đã đánh dấu chưa đọc' : 'Đã đánh dấu đã đọc');
                await list.reload();
            } catch (err) {
                toast.error(err instanceof Error ? err.message : 'Cập nhật trạng thái thất bại');
            }
        },
        onDelete: (n) => {
            setSelected(n);
            setDeleteModalOpen(true);
        },
    });

    const handleDelete = async () => {
        if (!selected) return;
        try {
            await notificationService.remove(selected.id);
            toast.success('Xóa thông báo thành công');
            setDeleteModalOpen(false);
            setSelected(null);
            await list.reload();
        } catch (err) {
            toast.error(err instanceof Error ? err.message : 'Xóa thông báo thất bại');
        }
    };

    return (
        <div className="space-y-6">
            <PageHeader title="Thông báo" subtitle="Thông báo của bạn" />

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
                    emptyText="Chưa có thông báo nào"
                />
            )}

            <ConfirmModal
                open={deleteModalOpen}
                onClose={() => setDeleteModalOpen(false)}
                title="Xóa thông báo"
                message={`Bạn có chắc chắn muốn xóa "${selected?.title}" không?`}
                confirmText="Xóa"
                onConfirm={handleDelete}
                danger
            />
        </div>
    );
}
