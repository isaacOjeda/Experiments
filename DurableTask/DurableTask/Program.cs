using DurableTask;
using DurableTask.AzureStorage;
using DurableTask.Core;

// craete a Hub Worker using Azure Storage
var storageConnectionString = "UseDevelopmentStorage=true";
var taskHubName = "TestHub";

var azureStorageOrchestrationService = new AzureStorageOrchestrationService(new AzureStorageOrchestrationServiceSettings()
{
    StorageConnectionString = storageConnectionString,
    TaskHubName = taskHubName
});


// According args, start server or client
if (args.Length > 0 && args[0] == "server")
{
    await StartServer();
}
else
{
    await StartClient();
}


async Task StartServer()
{
    var taskHubWorker = new TaskHubWorker(azureStorageOrchestrationService);

    // register orchestrations and activities
    taskHubWorker.AddTaskOrchestrations(typeof(TestOrchestration));
    taskHubWorker.AddTaskActivities(typeof(TestActivity1));
    taskHubWorker.AddTaskActivities(typeof(TestActivity2));


    // start the worker
    try
    {
        Console.WriteLine("Starting...");
        await azureStorageOrchestrationService.CreateIfNotExistsAsync();
        await taskHubWorker.StartAsync();
        Console.WriteLine("Started. Press any key to quit.");
        Console.ReadLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

}



async Task StartClient()
{
    var client = new TaskHubClient(azureStorageOrchestrationService);
    var instanceId = await client.CreateOrchestrationInstanceAsync(typeof(TestOrchestration), "input");
    Console.WriteLine($"Started orchestration with ID = '{instanceId}'.");
}