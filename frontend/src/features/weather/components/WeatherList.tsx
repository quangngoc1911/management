'use client'
import { useWeather } from '../hooks/useWeather'

 
export function WeatherList() {
    const { data, isLoading, isError } = useWeather();

    if (isLoading) return <p className="text-sm text-muted animate-pulse">Đang tải...</p>;
    if (isError) return <p className="text-sm text-danger">Lỗi kết nối API</p>;

    return (
        <div className="table-wrapper">
            <table className="w-full text-sm">
                <thead>
                    {/* ✅ bg-surface-alt thay vì bg-gray-100 */}
                    <tr className="bg-surface-alt border-b border-border">
                        {['Ngày', '°C', '°F', 'Mô tả'].map((h) => (
                            <th
                                key={h}
                                className="px-4 py-3 text-left text-xs font-semibold
                                           text-muted uppercase tracking-wide"
                            >
                                {h}
                            </th>
                        ))}
                    </tr>
                </thead>
                <tbody className="divide-y divide-border">
                    {data?.map((w, i) => (
                        /* ✅ hover:bg-surface-alt thay vì hover:bg-gray-50 */
                        <tr key={i} className="hover:bg-surface-alt transition-colors">
                            <td className="px-4 py-3 text-foreground">{w.date}</td>
                            <td className="px-4 py-3 text-foreground">{w.temperatureC}</td>
                            <td className="px-4 py-3 text-foreground">{w.temperatureF}</td>
                            <td className="px-4 py-3 text-muted">{w.summary}</td>
                        </tr>
                    ))}
                    {(!data || data.length === 0) && (
                        <tr>
                            <td colSpan={4} className="px-4 py-10 text-center text-muted">
                                Không có dữ liệu
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}
