'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Eye, EyeOff, Loader2 } from 'lucide-react';
import { useAuth } from '@/features/auth/hooks/useAuth';

const loginSchema = z.object({
    email: z.string().email('Email không hợp lệ'),
    password: z.string().min(6, 'Mật khẩu phải có ít nhất 6 ký tự'),
});

type LoginFormData = z.infer<typeof loginSchema>;

export default function LoginPage() {
  const router = useRouter();
  const { login, loading, error } = useAuth();
  const [showPassword, setShowPassword] = useState(false);
  
  const {
      register,
      handleSubmit,
      formState: { errors },
  } = useForm<LoginFormData>({
      resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginFormData) => {
      await login(data);
  };

  return (
      <div className="min-h-screen flex items-center justify-center bg-background p-4">
          <div className="w-full max-w-md">
              {/* Logo / Title */}
              <div className="text-center mb-8">
                  <h1 className="text-2xl font-bold text-foreground">Hệ thống Quản lý Tài liệu</h1>
                  <p className="text-muted mt-2 text-sm">Đăng nhập để tiếp tục</p>
              </div>

              {/* Login Form */}
              <div className="card p-8">
                  <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                      {/* Email */}
                      <div>
                          <label
                              htmlFor="email"
                              className="block text-sm font-medium text-foreground mb-2"
                          >
                              Email
                          </label>
                          <input
                              id="email"
                              type="email"
                              {...register('email')}
                              className={`input ${errors.email ? 'input-error' : ''}`}
                              placeholder="Nhập email của bạn"
                          />
                          {errors.email && (
                              <p className="text-danger text-xs mt-1">{errors.email.message}</p>
                          )}
                      </div>

                      {/* Password */}
                      <div>
                          <label
                              htmlFor="password"
                              className="block text-sm font-medium text-foreground mb-2"
                          >
                              Mật khẩu
                          </label>
                          <div className="relative">
                              <input
                                  id="password"
                                  type={showPassword ? 'text' : 'password'}
                                  {...register('password')}
                                  className={`input pr-10 ${errors.password ? 'input-error' : ''}`}
                                  placeholder="Nhập mật khẩu"
                              />
                              <button
                                  type="button"
                                  onClick={() => setShowPassword(!showPassword)}
                                  className="absolute right-3 top-1/2 -translate-y-1/2 text-muted hover:text-foreground"
                              >
                                  {showPassword ? (
                                      <EyeOff className="w-4 h-4" />
                                  ) : (
                                      <Eye className="w-4 h-4" />
                                  )}
                              </button>
                          </div>
                          {errors.password && (
                              <p className="text-danger text-xs mt-1">{errors.password.message}</p>
                          )}
                      </div>

                      {/* Error Message */}
                      {error && (
                          <div className="p-3 rounded-md bg-danger/10 text-danger text-sm">
                              {error}
                          </div>
                      )}

                      {/* Submit Button */}
                      <button
                          type="submit"
                          disabled={loading}
                          className="w-full flex items-center justify-center gap-2 bg-primary hover:bg-primary-hover text-white py-2.5 rounded-md font-medium transition disabled:opacity-50"
                      >
                          {loading && <Loader2 className="w-4 h-4 animate-spin" />}
                          {loading ? 'Đang đăng nhập...' : 'Đăng nhập'}
                      </button>
                  </form>

                  {/* Register Link */}
                  <div className="mt-6 text-center text-sm text-muted">
                      Chưa có tài khoản?{' '}
                      <Link
                          href="/register"
                          className="text-primary hover:text-primary-hover font-medium"
                      >
                          Đăng ký ngay
                      </Link>
                  </div>
              </div>
          </div>
      </div>
  );
}
