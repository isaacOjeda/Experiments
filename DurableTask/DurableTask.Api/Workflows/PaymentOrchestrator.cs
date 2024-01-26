using DurableTask.Core;

namespace DurableTask.Api.Workflows;

public class PaymentOrchestrator : TaskOrchestration<string, string>
{
    private readonly ILogger<PaymentOrchestrator> _logger;

    public PaymentOrchestrator(ILogger<PaymentOrchestrator> logger)
    {
        _logger = logger;
    }

    public override async Task<string> RunTask(OrchestrationContext context, string input)
    {
        _logger.LogInformation("Is Replaying =" + context.IsReplaying
            + " InstanceId =" + context.OrchestrationInstance.InstanceId
            + " Execution ID =" + context.OrchestrationInstance.ExecutionId);

        _logger.LogInformation("Running Orchestration");

        var result1 = await context.ScheduleTask<string>(typeof(CreatePaymentActivity), input);
        //var result2 = await context.ScheduleTask<string>(typeof(UpdatePaymentActivity), "Test Input2");

        _logger.LogInformation("Completed Orchestration");
        return result1;
    }
}