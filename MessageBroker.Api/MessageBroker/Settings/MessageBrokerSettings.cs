namespace MessageBroker.Api.MessageBroker.Settings;

public sealed class MessageBrokerSettings
{
    public static string SectionName = "MessageBroker";

    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
