using DurableTask.Api.Workflows;
using DurableTask.AzureStorage;
using DurableTask.Core;

namespace DurableTask.Api;

public class WorkflowWorker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private TaskHubWorker _taskHubWorker;

    public WorkflowWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var storageConnectionString = "UseDevelopmentStorage=true";
        var taskHubName = "TestHub";

        var azureStorageOrchestrationService = new AzureStorageOrchestrationService(new AzureStorageOrchestrationServiceSettings()
        {
            StorageConnectionString = storageConnectionString,
            TaskHubName = taskHubName
        });


        _taskHubWorker = new TaskHubWorker(azureStorageOrchestrationService);


        await _taskHubWorker
            .AddTaskOrchestrations(new ServiceProviderObjectCreator<TaskOrchestration>(typeof(PaymentOrchestrator), _serviceProvider))
            .AddTaskActivities(new ServiceProviderObjectCreator<TaskActivity>(typeof(CreatePaymentActivity), _serviceProvider))
            .StartAsync();

        //while (!cancellationToken.IsCancellationRequested)
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        //}
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _taskHubWorker.StopAsync(true);
    }
}
