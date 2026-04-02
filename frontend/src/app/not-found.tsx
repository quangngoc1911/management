import Link from 'next/link';

export default function NotFound() {
    return (
        <div className="min-h-screen flex items-center justify-center bg-background">
            <div className="text-center space-y-4">
                <p className="text-8xl font-bold text-border">404</p>
                <h1 className="text-xl font-semibold text-foreground">
                    Trang bạn muốn try cập không tồn tại
                </h1>
                <p className="text-sm text-muted">Đường dẫn bạn truy cập không hợp lệ.</p>
                <Link
                    href="/login"
                    className="inline-block bg-primary hover:bg-primary-hover
                               text-white px-5 py-2 rounded-md text-sm font-medium transition"
                >
                    Về trang đăng nhập
                </Link>
            </div>
        </div>
    );
}
