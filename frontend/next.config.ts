const nextConfig = {
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
    ]
  },
}

export default nextConfig