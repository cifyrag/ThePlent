using Stripe;
using ThePlant.Services.Stripe.Models;

namespace ThePlant.Services.Stripe;

public interface IStripeService
{
    Task<AccountLink?> CreateStripeAccountLinkAsync(string accountId, StripeAccountLinkCreate request);
    Task<Account?> CreateStripeAccountAsync(string email);
    Task<Account?> GetStripeAccountAsync(string accountId);
    Task<PaymentIntent> CreateIntentAsync(string stripeAccountId, StripePaymentIntentCreate request);
    Task<PaymentIntent?> CreateIntentMultipleAsync(StripePaymentIntentMultipleCreate request);
    Task<PaymentIntent?> GetPaymentIntentAsync(string paymentIntentId, string stripeAccountId);
    Task<BalanceTransaction?> GetBalanceTransactionAsync(string id, string accountId);
    Task<PaymentIntent?> CancelPaymentIntentAsync(string paymentIntentId, string stripeAccountId, string cancellationReason);
    Task<Refund?>  RefundPaymentIntentAsync(string paymentIntentId, string stripeAccountId);
    Task<Transfer?> CreateTransferAsync(string stripeAccountId ,StripePaymentTransferCreate request);

    Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentId, string stripeAccountId, string paymentMethod);
    
}