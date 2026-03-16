import { UserList } from '@/features/users/components/UserList'

export default function UsersPage() {
    return (
        <main className="p-8">
            <h1 className="text-2xl font-semibold mb-6">Danh sách Users</h1>
            <UserList />
        </main>
    )
}