namespace ManagementSystem.Application.Contracts;

public interface IDateTime
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}