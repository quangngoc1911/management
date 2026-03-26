/\* ================================================================

GLOBALS.CSS — DESIGN SYSTEM

Tailwind v4 + next-themes (dark mode via class="dark" trên `<html>`)

NGUYÊN TẮC:

- Dùng token (bg-surface, text-foreground...) thay vì màu cứng
- Chỉ cần đổi giá trị trong :root / .dark là toàn app cập nhật

    ================================================================ \*/

/\* ================================================================

6. HƯỚNG DẪN SỬ DỤNG TỪNG THÀNH PHẦN

    Copy-paste trực tiếp vào component.

    ================================================================ \*/

/\*

┌─────────────────────────────────────────────────────────────┐

│ LAYOUT — Khung trang & vùng chứa │

└─────────────────────────────────────────────────────────────┘

Nền toàn trang:

    `<div className="min-h-screen bg-background text-foreground">`

Căn giữa trang (login, landing):

    `<div className="min-h-screen flex items-center justify-center bg-background">`

Layout sidebar + content:

    `<div className="flex h-screen">`

    `<aside className="w-64 bg-sidebar shrink-0">`...`</aside>`

    `<main className="flex-1 overflow-auto p-6">`...`</main>`

    `</div>`

Trang trong dashboard (dùng class .page):

    `<div className="page">`

    `<h1 className="text-2xl font-bold text-foreground">`Tiêu đề trang `</h1>`

    ...nội dung...

    `</div>`

Container có max-width:

    `<div className="max-w-4xl mx-auto px-6 py-8">`

┌─────────────────────────────────────────────────────────────┐

│ CARD / PANEL │

└─────────────────────────────────────────────────────────────┘

Card (dùng class .card):

    `<div className="card p-6">`

Card có shadow:

    `<div className="card p-6 shadow-card">`

Card có header + body:

    `<div className="card overflow-hidden">`

    `<div className="px-6 py-4 border-b border-border font-semibold text-foreground">`

    Tiêu đề

    `</div>`

    `<div className="p-6">`Nội dung `</div>`

    `</div>`

Panel nền phụ:

    `<div className="bg-surface-alt rounded-lg p-4">`

┌─────────────────────────────────────────────────────────────┐

│ TYPOGRAPHY │

└─────────────────────────────────────────────────────────────┘

h1 — Tiêu đề trang: text-2xl font-bold text-foreground

h2 — Tiêu đề section: text-lg font-semibold text-foreground

h3 — Tiêu đề card/dialog: text-base font-semibold text-foreground

Chữ thân bài: text-sm text-foreground

Chữ phụ / caption / hint: text-sm text-muted | text-xs text-muted

Label input: text-sm font-medium text-foreground

Link: text-primary hover:underline underline-offset-2

Chữ lỗi: text-xs text-danger

Chữ thành công: text-xs text-success

┌─────────────────────────────────────────────────────────────┐

│ FORM — Input, Textarea, Select, Checkbox │

└─────────────────────────────────────────────────────────────┘

Cách 1 — dùng class .input (ngắn gọn):

    `<input className="input" placeholder="..." />`

    `<input className="input input-error" />`   ← khi có lỗi

    `<textarea className="input resize-none h-32" />`

    `<select className="input">`

Cách 2 — dùng Tailwind trực tiếp (linh hoạt hơn):

    <input className="w-full bg-surface-alt border border-border rounded-md

    px-3 py-2 text-sm text-foreground placeholder:text-muted

    focus:outline-none focus:ring-2 focus:ring-primary

    disabled:opacity-50 disabled:cursor-not-allowed" />

Group label + input + error:

    `<div className="flex flex-col gap-1">`

    `<label className="text-sm font-medium text-foreground">`Email `</label>`

    `<input className="input" />`

    `<span className="text-xs text-danger">`Lỗi...

    `</div>`

Checkbox:

    `<input type="checkbox" className="w-4 h-4 rounded-sm accent-primary" />`

Form section nhóm field:

    `<div className="form-section">`

    `<h3>`Thông tin cơ bản `</h3>`

    `<div className="grid grid-cols-2 gap-4">`

    ...fields...

    `</div>`

    `</div>`

┌─────────────────────────────────────────────────────────────┐

│ BUTTON │

└─────────────────────────────────────────────────────────────┘

Primary — Lưu, Tạo mới, Đăng nhập:

    bg-primary hover:bg-primary-hover text-white

    px-4 py-2 rounded-md text-sm font-medium

    transition disabled:opacity-50 disabled:cursor-not-allowed

Danger — Xóa, không thể hoàn tác:

    bg-danger hover:bg-danger-hover text-white

    px-4 py-2 rounded-md text-sm font-medium transition

Secondary — Hủy, Quay lại:

    bg-surface-alt hover:bg-border text-foreground

    border border-border px-4 py-2 rounded-md text-sm font-medium transition

Ghost — icon button, toolbar:

    hover:bg-surface-alt text-muted hover:text-foreground

    px-3 py-2 rounded-md text-sm transition

Full width: thêm "w-full"

Loading state: thêm "disabled:opacity-50 disabled:cursor-not-allowed"

┌─────────────────────────────────────────────────────────────┐

│ TABLE │

└─────────────────────────────────────────────────────────────┘

Dùng class .table-wrapper để hỗ trợ scroll ngang:

    `<div className="table-wrapper">`

    `<table className="w-full text-sm">`

    `<thead>`

    `<tr className="bg-surface-alt border-b border-border">`

    <th className="px-4 py-3 text-left text-xs font-semibold

    text-muted uppercase tracking-wide">

    `<tbody className="divide-y divide-border">`

    `<tr className="hover:bg-surface-alt transition-colors">`

    `<td className="px-4 py-3 text-foreground">`

Empty state trong table:

    `<tr>`

    <td colSpan={n} className="px-4 py-10 text-center text-muted text-sm">

    Không có dữ liệu

    `</td>`

    `</tr>`

┌─────────────────────────────────────────────────────────────┐

│ BADGE / TAG / STATUS │

└─────────────────────────────────────────────────────────────┘

Base:

    <span className="inline-flex items-center px-2 py-0.5

    rounded-full text-xs font-medium">

Xanh lá — Hoạt động / Hiện:

    "bg-green-100  text-green-700  dark:bg-green-900/30  dark:text-green-400"

Đỏ — Lỗi / Dừng:

    "bg-red-100    text-red-700    dark:bg-red-900/30    dark:text-red-400"

Vàng — Chờ / Pending:

    "bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400"

Xám — Ẩn / Vô hiệu:

    "bg-surface-alt text-muted"

Xanh dương — Info:

    "bg-blue-100   text-blue-700   dark:bg-blue-900/30   dark:text-blue-400"

LƯU Ý: Badge dùng dark: trực tiếp vì màu cố định → chấp nhận được.

┌─────────────────────────────────────────────────────────────┐

│ SIDEBAR │

└─────────────────────────────────────────────────────────────┘

<aside className="w-64 bg-sidebar flex flex-col h-screen shrink-0">

    Header:   px-4 py-5 border-b border-sidebar-border  |  text-white font-bold

    Nav:      flex-1 px-3 py-4 space-y-1 overflow-y-auto

    Item thường:   text-sidebar-muted hover:bg-sidebar-border hover:text-white

    Item active:   bg-primary text-white

    Sub-menu:      ml-3 mt-1 pl-3 border-l border-sidebar-border space-y-1

    Footer:   px-3 py-4 border-t border-sidebar-border

    Đăng xuất:     text-danger hover:bg-sidebar-border

┌─────────────────────────────────────────────────────────────┐

│ ALERT / NOTIFICATION │

└─────────────────────────────────────────────────────────────┘

Lỗi: p-4 rounded-lg text-sm bg-red-50 text-red-700

    dark:bg-red-900/20    dark:text-red-400

Thành công: p-4 rounded-lg text-sm bg-green-50 text-green-700

    dark:bg-green-900/20  dark:text-green-400

Cảnh báo: p-4 rounded-lg text-sm bg-yellow-50 text-yellow-700

    dark:bg-yellow-900/20 dark:text-yellow-400

Info: p-4 rounded-lg text-sm bg-blue-50 text-blue-700

    dark:bg-blue-900/20   dark:text-blue-400

┌─────────────────────────────────────────────────────────────┐

│ MODAL / DIALOG │

└─────────────────────────────────────────────────────────────┘

Overlay: fixed inset-0 bg-black/50 flex items-center justify-center z-modal

Box: bg-surface border border-border rounded-xl shadow-modal

    w-full max-w-md mx-4 p-6

Header: flex items-center justify-between mb-4

    | h2: text-base font-semibold text-foreground

    | close btn: text-muted hover:text-foreground transition

Footer: flex justify-end gap-3 mt-6 pt-4 border-t border-border

┌─────────────────────────────────────────────────────────────┐

│ DROPDOWN MENU │

└─────────────────────────────────────────────────────────────┘

Wrapper (vị trí tương đối):

    `<div className="relative">`

Panel dropdown:

    <div className="absolute top-full left-0 mt-1 z-dropdown

    bg-surface border border-border rounded-lg shadow-modal

    min-w-[10rem] py-1 overflow-hidden">

Item trong dropdown:

    <button className="w-full text-left px-4 py-2 text-sm text-foreground

    hover:bg-surface-alt transition-colors">

Divider trong dropdown:

    `<div className="my-1 border-t border-border" />`

Item nguy hiểm (xóa):

    <button className="w-full text-left px-4 py-2 text-sm text-danger

    hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors">

┌─────────────────────────────────────────────────────────────┐

│ TOOLTIP │

└─────────────────────────────────────────────────────────────┘

<div className="relative group inline-flex">

    `<button>`Hover tôi `</button>`

    <div className="absolute bottom-full left-1/2 -translate-x-1/2 mb-2

    z-tooltip opacity-0 group-hover:opacity-100

    pointer-events-none transition-opacity duration-fast

    bg-gray-900 text-white text-xs px-2 py-1 rounded-md

    whitespace-nowrap dark:bg-gray-700">

    Nội dung tooltip

    `</div>`

</div>

┌─────────────────────────────────────────────────────────────┐

│ DIVIDER │

└─────────────────────────────────────────────────────────────┘

Ngang: `<hr className="border-border" />`

Ngang có chữ: flex items-center gap-4

    | hr: flex-1 border-border

    | span: text-xs text-muted

Dọc (flex row): `<div className="w-px h-5 bg-border mx-2" />`

┌─────────────────────────────────────────────────────────────┐

│ LOADING / SKELETON │

└─────────────────────────────────────────────────────────────┘

Text: text-sm text-muted animate-pulse

Skeleton line: h-4 w-48 bg-surface-alt rounded animate-pulse

Skeleton box: h-10 w-full bg-surface-alt rounded-md animate-pulse

Spinner nút: w-4 h-4 border-2 border-white/30 border-t-white

    rounded-full animate-spin

Spinner trang: w-8 h-8 border-2 border-border border-t-primary

    rounded-full animate-spin mx-auto

┌─────────────────────────────────────────────────────────────┐

│ EMPTY STATE │

└─────────────────────────────────────────────────────────────┘

<div className="flex flex-col items-center justify-center py-16 text-center">

    `<div className="text-4xl mb-4">`📭`</div>`

    `<p className="text-sm font-medium text-foreground mb-1">`Chưa có dữ liệu `</p>`

    `<p className="text-xs text-muted mb-4">`Thêm mới để bắt đầu sử dụng `</p>`

    `<button className="bg-primary ...">`Thêm mới `</button>`

</div>

┌─────────────────────────────────────────────────────────────┐

│ PAGINATION │

└─────────────────────────────────────────────────────────────┘

<div className="flex items-center justify-between px-4 py-3

    border-t border-border text-sm text-muted">

    `<span>`Hiển thị 1–10 / 42

    `<div className="flex items-center gap-1">`

    <button className="px-3 py-1.5 rounded-md hover:bg-surface-alt

    disabled:opacity-40 transition">

    ‹

    `</button>`

    `<button className="px-3 py-1.5 rounded-md bg-primary text-white">`1 `</button>`

    `<button className="px-3 py-1.5 rounded-md hover:bg-surface-alt transition">`2 `</button>`

    <button className="px-3 py-1.5 rounded-md hover:bg-surface-alt

    disabled:opacity-40 transition">

    ›

    `</button>`

    `</div>`

</div>

┌─────────────────────────────────────────────────────────────┐

│ SPACING — Khoảng cách chuẩn │

└─────────────────────────────────────────────────────────────┘

Giữa các section trong trang → space-y-8 (32px)

Giữa các block trong section → space-y-6 (24px)

Giữa các field trong form → space-y-4 (16px)

Giữa label và input → gap-1 (4px)

Giữa các button trong hàng → gap-3 (12px)

Padding card / panel → p-6 (24px)

Padding trang dashboard → p-6

Padding cell bảng → px-4 py-3

Padding header sidebar → px-4 py-5

┌─────────────────────────────────────────────────────────────┐

│ Z-INDEX — Thứ tự lớp │

└─────────────────────────────────────────────────────────────┘

Dùng z-dropdown, z-sticky, z-overlay, z-modal, z-toast, z-tooltip

thay vì z-10, z-50... để tránh conflict.

<div className="... z-dropdown">   ← dropdown menu

<div className="... z-modal">      ← dialog, modal

<div className="... z-toast">      ← toast notification

<div className="... z-tooltip">    ← tooltip

┌─────────────────────────────────────────────────────────────┐

│ QUY TẮC CHỌN CLASS — Tóm tắt nhanh │

└─────────────────────────────────────────────────────────────┘

Màu nền:

    Toàn trang              → bg-background

    Card / form / modal     → bg-surface

    Input / thead / tag     → bg-surface-alt

    Sidebar                 → bg-sidebar

Màu chữ:

    Chữ chính               → text-foreground

    Chữ phụ / hint          → text-muted

    Chữ lỗi                 → text-danger

    Chữ thành công          → text-success

    Link / active           → text-primary

    Chữ trong sidebar       → text-white  hoặc  text-sidebar-muted

Viền:

    Thông thường            → border border-border

    Lỗi                     → border border-danger

    Sidebar                 → border-sidebar-border

Nút:

    Chính                   → bg-primary text-white

    Nguy hiểm               → bg-danger text-white

    Phụ                     → bg-surface-alt border border-border text-foreground

    Ẩn / icon               → hover:bg-surface-alt text-muted

KHÔNG nên dùng:

    ✗  bg-white dark:bg-gray-800     →  bg-surface

    ✗  text-black dark:text-white    →  text-foreground

    ✗  border-gray-200 dark:...      →  border-border

    ✗  text-gray-500 dark:...        →  text-muted

    ✗  z-10, z-50 rải rác            →  z-dropdown, z-modal...

\*/

    tokens.css    # biến, màu, shadow

    theme.css# light / dark
        base.css         # reset, html, body
        components.css    # .input, .card, .page
        utilities.css     # class bổ sung
      globals.css         # import tất cả
