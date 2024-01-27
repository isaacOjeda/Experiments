using DurableTask.Core;

namespace DurableTask.Api.Workflows.CreatePayment;

public class CreatePaymentActivity(ILogger<CreatePaymentActivity> logger)
    : AsyncTaskActivity<CreatePaymentRequest, CreatePaymentResponse>
{
    protected override Task<CreatePaymentResponse> ExecuteAsync(TaskContext context, CreatePaymentRequest input)
    {
        logger.LogInformation("\nCreating payment for order {OrderId} with payment method {PaymentMethodId}\n",
            input.OrderId, input.PaymentMethodId);

        return Task.FromResult(new CreatePaymentResponse(Guid.NewGuid().ToString()));
    }
}


public record CreatePaymentRequest(string OrderId, string PaymentMethodId);
public record CreatePaymentResponse(string PaymentId);