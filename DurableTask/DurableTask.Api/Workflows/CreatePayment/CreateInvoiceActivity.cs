using DurableTask.Core;

namespace DurableTask.Api.Workflows.CreatePayment;

public class CreateInvoiceActivity(ILogger<CreateInvoiceActivity> logger)
    : AsyncTaskActivity<CreateInvoiceRequest, CreateInvoiceResponse>
{
    protected override Task<CreateInvoiceResponse> ExecuteAsync(TaskContext context, CreateInvoiceRequest input)
    {
        logger.LogInformation("\nCreating invoice for order {OrderId} with payment {PaymentId}\n",
                       input.OrderId, input.PaymentId);

        return Task.FromResult(new CreateInvoiceResponse(Guid.NewGuid().ToString()));
    }
}

public record CreateInvoiceRequest(string OrderId, string PaymentId);
public record CreateInvoiceResponse(string InvoiceId);