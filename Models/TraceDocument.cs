namespace DotnetElkDemo.Models;

public record TraceDocument
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public bool FlagActive { get; init; } = true;
    public string CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string ModifiedBy { get; init; }
    public DateTime ModifiedAt { get; init; } = DateTime.UtcNow;
    public string Input { get; init; }
    public long Code { get; init; }
    public string Api { get; init; }
    public string Request { get; init; }
    public string Response { get; init; }
    public string Method { get; init; }
    public string UrlQuery { get; init; }
}