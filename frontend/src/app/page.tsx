import Link from 'next/link';

export default function HomePage() {
    return (
        <div className="min-h-screen flex items-center justify-center bg-background">
            <div className="text-center space-y-4">
                <h1 className="text-3xl font-bold text-foreground">Chào mừng</h1>
                <p className="text-muted text-sm">Vui lòng đăng nhập để sử dụng hệ thống</p>
                <Link
                    href="/login"
                    className="inline-block bg-primary hover:bg-primary-hover
                               text-white px-6 py-2 rounded-md text-sm font-medium transition"
                >
                    Đăng nhập
                </Link>
            </div>
        </div>
    );
}
