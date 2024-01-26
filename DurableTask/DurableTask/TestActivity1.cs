using DurableTask.Core;

namespace DurableTask;
public class TestActivity1 : AsyncTaskActivity<string, bool>
{


    protected override async Task<bool> ExecuteAsync(TaskContext context, string input)
    {
        Console.WriteLine("Starting Task 1, Execution Id = " + context.OrchestrationInstance.ExecutionId + ", Instance id = " + context.OrchestrationInstance.InstanceId);
        await Task.Delay(5000);
        Console.WriteLine("nEnding Task 1, Execution Id = " + context.OrchestrationInstance.ExecutionId + ", Instance id = " + context.OrchestrationInstance.InstanceId);
        return true;
    }
}
