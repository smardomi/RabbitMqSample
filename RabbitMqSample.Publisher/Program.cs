using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMqSample.Logging;
using RabbitMqSample.Publisher.Data;
using RabbitMqSample.Publisher.ServiceWorkers;
using RabbitMqSample.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Host.RegisterLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHostedService<OutBoxServiceWorker>();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(RabbitMqConstants.RabbitMqHostAddress);
    });
}).AddMassTransitHostedService();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));


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
