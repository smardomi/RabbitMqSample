using GreenPipes;
using MassTransit;
using RabbitMqSample.Consumer.Consumers;
using RabbitMqSample.Logging;
using RabbitMqSample.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Host.RegisterLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ItemCreatedEventConsumer>();

var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => typeof(IEventConsumer).IsAssignableFrom(p))
    .Select(x => x.Assembly) 
    .ToArray();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumers(assemblies);

    config.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(RabbitMqConstants.RabbitMqHostAddress);
        configurator.ReceiveEndpoint("Product-queue", c =>
        {
            c.ConfigureConsumers(context);
        });

        configurator.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5),
                                                                                       TimeSpan.FromMinutes(15),
                                                                                       TimeSpan.FromMinutes(30)));
        configurator.UseMessageRetry(r => r.Immediate(5));
    });
}).AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
