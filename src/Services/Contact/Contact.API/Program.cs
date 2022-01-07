using Contact.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using Contact.API.Infrastructure.PipelineBehaviors;
using Contact.API.Infrastructure.Middlewares;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<ContactContext>(option => option.UseInMemoryDatabase("rageAgainstTheMachine"));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
        cfg.Host("192.168.99.100"); //TODO:
    });
});
builder.Services.AddMassTransitHostedService(true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
