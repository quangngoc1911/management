import { UserList } from '@/features/users/components/UserList';

export default function UsersPage() {
    console.log('USERS PAGE');
    return (
        <>
            <div className="page-header">
                <h1 className="text-2xl font-semibold text-foreground">Danh sách Users</h1>
            </div>
            <UserList />
        </>
    );
}
