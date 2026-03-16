import { WeatherList } from '@/features/weather/components/WeatherList'

export default function WeatherPage() {
    return (
        <main className="p-8">
            <h1 className="text-2xl font-semibold mb-6">Weather Forecast</h1>
            <WeatherList />
        </main>
    )
}