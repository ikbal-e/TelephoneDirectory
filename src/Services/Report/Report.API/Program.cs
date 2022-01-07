using MassTransit;
using Report.API.Consumers;
using Report.API.Infrastructure.Data;
using Report.API.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ReportDatabaseSettings>(builder.Configuration.GetSection(nameof(ReportDatabaseSettings)));

builder.Services.AddSingleton<ReportContext>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ContactInfoCreatedEventConsumer>().Endpoint(e => e.Name = "contact-info-created-event-queue");
    x.AddConsumer<ContactInfoDeletedEventConsumer>().Endpoint(e => e.Name = "contact-info-deleted-event-queue");
    x.AddConsumer<PersonCreatedEventConsumer>().Endpoint(e => e.Name = "person-created-event-queue");
    x.AddConsumer<PersonDeletedEventConsumer>().Endpoint(e => e.Name = "person-deleted-created-event-queue"); ;

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("192.168.99.100"); //TODO: 
        cfg.ConfigureEndpoints(context);
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

app.MapControllers();

app.Run();
