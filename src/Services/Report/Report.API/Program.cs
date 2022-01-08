using MassTransit;
using Report.API.Consumers;
using Report.API.Infrastructure.Data;
using Report.API.Infrastructure.Models;
using Report.API.Services;

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
    x.AddConsumer<PersonDeletedEventConsumer>().Endpoint(e => e.Name = "person-deleted-created-event-queue");
    x.AddConsumer<ReportRequestedEventConsumer>().Endpoint(e =>
    {
        e.Name = "report-requested-event-queue";
        e.ConcurrentMessageLimit = 1;
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("192.168.99.100"); //TODO: 
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddMassTransitHostedService(true);

builder.Services.AddScoped<ILocationService, LocationService>();

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
