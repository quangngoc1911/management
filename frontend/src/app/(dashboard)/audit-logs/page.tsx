'use client';

import { auditLogService } from '@/shared/lib/services/auditLogService';
import type { AuditLog } from '@/shared/lib/services/auditLogService';
import { getAuditLogColumns } from '@/features/system/utils/auditLogColumns';
import { PaginatedTable } from '@/components/DataTable';
import { PageLoading } from '@/components/LoadingSpinner';
import { PageHeader } from '@/components/PageHeader';
import { ErrorBanner } from '@/components/ErrorBanner';
import { usePaginatedList } from '@/shared/hooks/usePaginatedList';

export default function AuditLogsPage() {
    const list = usePaginatedList<AuditLog>({
        fetcher: auditLogService.getAll,
        pageSize: 15,
        errorMessage: 'Không thể tải nhật ký kiểm toán',
    });

    const columns = getAuditLogColumns();

    return (
        <div className="space-y-6">
            <PageHeader
                title="Nhật ký kiểm toán"
                subtitle="Lịch sử thao tác trên hệ thống (chỉ đọc, Admin)"
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
                    emptyText="Chưa có nhật ký nào"
                />
            )}
        </div>
    );
}
