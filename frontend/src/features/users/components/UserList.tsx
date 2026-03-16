'use client'
import { useUsers, useCreateUser } from '../hooks/useUsers'
import { useState } from 'react'

export function UserList() {
    const { data: users, isLoading, isError } = useUsers()
    const { mutate: createUser, isPending } = useCreateUser()

    const [name, setName] = useState('')
    const [email, setEmail] = useState('')

    function handleSubmit(e: React.FormEvent) {
        e.preventDefault()
        if (!name || !email) return
        createUser({ name, email }, {
            onSuccess: () => {
                setName('')
                setEmail('')
            },
        })
    }

    if (isLoading) return <p>Đang tải...</p>
    if (isError) return <p className="text-red-500">Lỗi kết nối API</p>

    return (
        <div>
            {/* Form tạo user */}
            <form onSubmit={handleSubmit} className="flex gap-3 mb-8">
                <input
                    value={name}
                    onChange={e => setName(e.target.value)}
                    placeholder="Tên"
                    className="border px-3 py-2 rounded w-48"
                />
                <input
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                    placeholder="Email"
                    className="border px-3 py-2 rounded w-64"
                />
                <button
                    type="submit"
                    disabled={isPending}
                    className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 disabled:opacity-50"
                >
                    {isPending ? 'Đang lưu...' : 'Thêm user'}
                </button>
            </form>

            {/* Danh sách */}
            <table className="w-full border-collapse">
                <thead>
                    <tr className="bg-gray-100 text-left">
                        <th className="border px-4 py-2">Tên</th>
                        <th className="border px-4 py-2">Email</th>
                    </tr>
                </thead>
                <tbody>
                    {users?.map((user, i) => (
                        <tr key={user.id ?? i} className="hover:bg-gray-50">
                            <td className="border px-4 py-2">{user.name}</td>
                            <td className="border px-4 py-2">{user.email}</td>
                        </tr>
                    ))}
                    {users?.length === 0 && (
                        <tr>
                            <td colSpan={2} className="text-center py-4 text-gray-400">
                                Chưa có user nào
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    )
}