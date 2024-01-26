using DurableTask.Core;

namespace DurableTask;
public class TestOrchestration : TaskOrchestration<bool, string>
{
    public override async Task<bool> RunTask(OrchestrationContext context, string input)
    {
        Console.WriteLine("Is Replaying =" + context.IsReplaying + " InstanceId =" + context.OrchestrationInstance.InstanceId + " Execution ID =" + context.OrchestrationInstance.ExecutionId);
        Console.WriteLine("Running Orchestration");

        var result1 = await context.ScheduleTask<bool>(typeof(TestActivity1), "Test Input1");
        var result2 = await context.ScheduleTask<bool>(typeof(TestActivity2), "Test Input2");


        var result = result1 && result2;

        Console.WriteLine("Orchestration Finished");
        return result;
    }
}