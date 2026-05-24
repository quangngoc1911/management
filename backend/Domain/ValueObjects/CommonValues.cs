namespace ManagementSystem.Domain.ValueObjects;

public sealed record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            throw new Exception("Email sai định dạng");

        Value = value.Trim().ToLowerInvariant();
    }

    public override string ToString() => Value;
}

public sealed record Slug
{
    public string Value { get; }

    public Slug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("Slug trống");

        Value = value.Trim().ToLowerInvariant().Replace(" ", "-");
    }

    public override string ToString() => Value;
}


public sealed record FileSize
{
    public long Bytes { get; init; }
    
    public FileSize(long Bytes)
    {
        if (Bytes < 0) throw new ArgumentException("Dung lượng không thể âm.");
    }

    public double ToMb() => Bytes / (1024.0 * 1024.0);
    public override string ToString() => $"{ToMb():F2} MB";
}


public sealed record DateRange
{
    public DateRange(DateTime Start, DateTime? End)
    {
        if (End.HasValue && End < Start)
            throw new ArgumentException("Ngày kết thúc không được trước ngày bắt đầu.");
    }
}