'use client'
import { useWeather } from '../hooks/useWeather'

export function WeatherList() {
    const { data, isLoading, isError } = useWeather()

    if (isLoading) return <p>Đang tải...</p>
    if (isError) return <p className="text-red-500">Lỗi kết nối API</p>

    return (
        <table className="w-full border-collapse">
            <thead>
                <tr className="bg-gray-100 text-left">
                    <th className="border px-4 py-2">Ngày</th>
                    <th className="border px-4 py-2">°C</th>
                    <th className="border px-4 py-2">°F</th>
                    <th className="border px-4 py-2">Mô tả</th>
                </tr>
            </thead>
            <tbody>
                {data?.map((w, i) => (
                    <tr key={i} className="hover:bg-gray-50">
                        <td className="border px-4 py-2">{w.date}</td>
                        <td className="border px-4 py-2">{w.temperatureC}</td>
                        <td className="border px-4 py-2">{w.temperatureF}</td>
                        <td className="border px-4 py-2">{w.summary}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    )
}
