namespace MessageBroker.Api.MessageBroker.Events;

public sealed record HelloEvent(string Message, DateTime? TimeStamp = null);
