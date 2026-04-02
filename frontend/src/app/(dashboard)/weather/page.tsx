import { WeatherList } from '@/features/weather/components/WeatherList';

export default function WeatherPage() {
    return (
        <>
            <div className="page-header">
                <h1 className="text-2xl font-semibold text-foreground">Weather Forecast</h1>
            </div>
            <WeatherList />
        </>
    );
}
