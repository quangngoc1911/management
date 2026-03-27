// bảo vệ tất cả route dashboard, đặt ở src/, ngang với app/
import { NextRequest, NextResponse } from "next/server";

export function middleware(request: NextRequest) {
  const token = request.cookies.get("accessToken")?.value;
  const { pathname } = request.nextUrl;

  const isLoginPage = pathname === "/login";

  // Chưa đăng nhập + vào trang khác → redirect login
  if (!token && !isLoginPage) {
    return NextResponse.redirect(new URL("/login", request.url));
  }

  // Đã đăng nhập + vào login → redirect menus
  if (token && isLoginPage) {
    return NextResponse.redirect(new URL("/menus", request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};