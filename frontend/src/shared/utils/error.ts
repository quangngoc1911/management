export const getErrorMessage = (err: unknown): string => {
  if (err instanceof Error) return err.message;
  return "Đã có lỗi xảy ra";
};
