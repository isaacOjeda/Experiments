using DurableTask.Api;
using DurableTask.Api.Workflows;
using DurableTask.AzureStorage;
using DurableTask.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<WorkflowWorker>();
builder.Services.AddScoped(sc =>
{
    var storageConnectionString = "UseDevelopmentStorage=true";
    var taskHubName = "TestHub";

    var azureStorageOrchestrationService = new AzureStorageOrchestrationService(
        new AzureStorageOrchestrationServiceSettings()
        {
            StorageConnectionString = storageConnectionString,
            TaskHubName = taskHubName
        });

    return new TaskHubClient(azureStorageOrchestrationService);
});
builder.Services.AddTransient<PaymentOrchestrator>();
builder.Services.AddTransient<CreatePaymentActivity>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/api/payments", async (string test, TaskHubClient client) =>
{
    var instanceId = await client.CreateOrchestrationInstanceAsync(typeof(PaymentOrchestrator), test);

    return Results.Ok(new
    {
        instanceId
    });
});

app.Run();

