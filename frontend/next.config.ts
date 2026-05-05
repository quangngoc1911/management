import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
    turbopack: {
        root: __dirname,
    },

    async rewrites() {
        return [
            {
                source: '/api/backend/:path*',
                destination: 'http://localhost:5218/api/:path*',
            },
            {
                source: '/WeatherForecast',
                destination: 'http://localhost:5218/WeatherForecast',
            },
        ];
    },
};

export default nextConfig;
