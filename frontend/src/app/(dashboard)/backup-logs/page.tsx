'use client';

import { backupLogService } from '@/shared/lib/services/backupLogService';
import type { BackupLog } from '@/shared/lib/services/backupLogService';
import { getBackupLogColumns } from '@/features/system/utils/backupLogColumns';
import { PaginatedTable } from '@/components/DataTable';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';

export default function BackupLogsPage() {
    const list = usePaginatedList<BackupLog>({
        fetcher: backupLogService.getAll,
        pageSize: 15,
        errorMessage: 'Không thể tải nhật ký sao lưu',
    });

    const columns = getBackupLogColumns();

    return (
        <div className="space-y-6">
            <PageHeader
                title="Nhật ký sao lưu"
                subtitle="Lịch sử sao lưu dữ liệu (chỉ đọc, Admin)"
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
                    emptyText="Chưa có bản ghi sao lưu nào"
                />
            )}
        </div>
    );
}
