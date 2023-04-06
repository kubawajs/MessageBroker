using MessageBroker.Api.MessageBroker.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using MessageBroker.Api.MessageBroker.EventBus;
using MessageBroker.Api.MessageBroker.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure message broker
builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection(MessageBrokerSettings.SectionName));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        var settings = context.GetRequiredService<MessageBrokerSettings>();
        configurator.Host(new Uri(settings.Host), host =>
        {
            host.Username(settings.Username);
            host.Password(settings.Password);
        });
    });
});

builder.Services.AddScoped<IEventBus, EventBus>();

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Example endpoint
app.MapGet("/api/messages", async (IEventBus eventBus) =>
{
    var timeStamp = DateTime.Now;
    var message = $"Message sent to RabbitMQ - {timeStamp}";
    await eventBus.PublishAsync(new HelloEvent(message, timeStamp));
    return new { Message = message, TimeStamp = timeStamp };
});

app.Run();