using DurableTask.Core;

namespace DurableTask.Api.Workflows;

public class CreatePaymentActivity : AsyncTaskActivity<string, string>
{
    private readonly ILogger<CreatePaymentActivity> _logger;

    public CreatePaymentActivity(ILogger<CreatePaymentActivity> logger)
    {
        _logger = logger;
    }

    protected override Task<string> ExecuteAsync(TaskContext context, string input)
    {
        _logger.LogInformation("Starting CreatePaymentActivity, Execution Id = " + context.OrchestrationInstance.ExecutionId + ", Instance id = " + context.OrchestrationInstance.InstanceId);
        _logger.LogInformation("Input = " + input);

        return Task.FromResult("CreatePaymentActivity");
    }
}
