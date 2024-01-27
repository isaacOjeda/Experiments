using DurableTask.Core;

namespace DurableTask.Api.Workflows.CreatePayment;

public class PaymentOrchestrator : TaskOrchestration<string, CreatePaymentRequest>
{
    private readonly ILogger<PaymentOrchestrator> _logger;

    public PaymentOrchestrator(ILogger<PaymentOrchestrator> logger)
    {
        _logger = logger;
    }

    public override async Task<string> RunTask(OrchestrationContext context, CreatePaymentRequest input)
    {
        _logger.LogInformation("Is Replaying =" + context.IsReplaying
            + " InstanceId =" + context.OrchestrationInstance.InstanceId
            + " Execution ID =" + context.OrchestrationInstance.ExecutionId);

        _logger.LogInformation("Running Orchestration");

        _logger.LogInformation("Calling CreatePayment");
        var paymentResponse = await context.ScheduleTask<CreatePaymentResponse>(typeof(CreatePaymentActivity), input);

        _logger.LogInformation("Calling CreateInvoice");
        var invoiceResponse = await context.ScheduleTask<CreateInvoiceResponse>(typeof(CreateInvoiceActivity),
            new CreateInvoiceRequest(input.OrderId, paymentResponse.PaymentId));


        _logger.LogInformation("Orchestration completed");

        return "success";
    }
}