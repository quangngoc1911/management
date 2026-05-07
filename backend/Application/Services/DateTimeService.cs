using ManagementSystem.Application.Contracts;

namespace ManagementSystem.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        // Trả về thời gian hiện tại (thường dùng Utc để đồng nhất)
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}