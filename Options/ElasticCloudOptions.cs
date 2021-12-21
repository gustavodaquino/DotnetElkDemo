namespace DotnetElkDemo.Options;

public record ElasticCloudOptions
{
    public string CloudId { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
}