using DurableTask.Core;

namespace DurableTask;
public class TestActivity2 : AsyncTaskActivity<string, bool>
{
    protected override async Task<bool> ExecuteAsync(TaskContext context, string input)
    {

        Console.WriteLine(" Starting Task 2, Execution Id = " + context.OrchestrationInstance.ExecutionId + ", Instance id = " + context.OrchestrationInstance.InstanceId);
        await Task.Delay(10000);
        Console.WriteLine("Ending Task 2, Execution Id = " + context.OrchestrationInstance.ExecutionId + ", Instance id = " + context.OrchestrationInstance.InstanceId);
        return true;
    }
}