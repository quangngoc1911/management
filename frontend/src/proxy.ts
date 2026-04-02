import { NextRequest, NextResponse } from "next/server";

export function proxy(request: NextRequest) {
  const token = request.cookies.get("accessToken")?.value;
  const { pathname } = request.nextUrl;

  const isPublicPath =
    pathname === "/" ||                // trang home
    pathname === "/login" ||           // login
    pathname.startsWith("/api") ||
    pathname.startsWith("/_next") ||
    pathname === "/favicon.ico";

  // ❌ chưa login → chỉ chặn private route
  if (!token && !isPublicPath) {
    const res = NextResponse.redirect(new URL("/login", request.url));
    res.cookies.set("redirect", pathname);
    return res;
  }

  // ❌ đã login mà vào login → đẩy vào dashboard
  if (token && pathname === "/login") {
    return NextResponse.redirect(new URL("/menus", request.url));
  }

  // ✅ còn lại cho đi bình thường
  return NextResponse.next();
}

export const config = {
  matcher: ["/((?!_next/static|_next/image|favicon.ico).*)"],
};