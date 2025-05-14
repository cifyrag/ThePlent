using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ThePlant.Services.Stripe.Models;
using Stripe;
using ThePlant.EF.Settings;

namespace ThePlant.Services.Stripe;

public class StripeService: IStripeService
{
    private readonly StripeSettings _stripeSettings;
    private readonly ILogger<StripeService> _logger;

    public StripeService(IOptions<StripeSettings> stripeSettings, ILogger<StripeService> logger)
    {
        _stripeSettings = stripeSettings.Value;
        _logger = logger;
        
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public async Task<AccountLink?> CreateStripeAccountLinkAsync(string accountId, StripeAccountLinkCreate request)
    {
        try
        {
            var options = new AccountLinkCreateOptions
            {
                Account = accountId,
                ReturnUrl = request.SuccessUrl,
                RefreshUrl = request.CancelUrl,
                Type = request.Type,
            };
            var service = new AccountLinkService();
            var accountLink = await service.CreateAsync(options);

            return accountLink ?? null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Stripe account link for account {AccountId}: {Message}", accountId, ex.Message);
            
            return null;
        }
    }

    public async Task<Account?> CreateStripeAccountAsync(string email)
    {
        try
        {
            var options = new AccountCreateOptions
            {
                Type = "express",
                Email = email,
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions
                    {
                        Requested = true
                    },
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true
                    }
                },
                Country = "SE" 
            };

            var service = new AccountService();

            var account = await service.CreateAsync(options);

            if (account != null && !string.IsNullOrEmpty(account.Id))
            {
                return account;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Stripe account for email {Email}: {Message}", email, ex.Message);
            
            return null;
        }
    }

    public async Task<Account?> GetStripeAccountAsync(string accountId)
    {
        try
        {
            var service = new AccountService();

            var account = await service.GetAsync(accountId);

            return account ?? null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Stripe account {AccountId}: {Message}", accountId, ex.Message);
            
            return null;
        }
    }

    public async Task<PaymentIntent> CreateIntentAsync(string stripeAccountId,
        StripePaymentIntentCreate request)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount+request.FeeAmount,
                Currency = request.Currency,
                ApplicationFeeAmount = request.FeeAmount,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
                {
                    Enabled = true
                }
            };
        
            var service = new PaymentIntentService();
        
            var requestOptions = new RequestOptions
            {
                StripeAccount = stripeAccountId,
            };
        
            var paymentIntent = await service.CreateAsync(options, requestOptions);
        
            return paymentIntent ?? null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PaymentIntent for account {StripeAccountId}: {Message}",
                stripeAccountId, ex.Message);
            
            return null;
        }
    }

    public async Task<PaymentIntent?> CreateIntentMultipleAsync(StripePaymentIntentMultipleCreate request)
    {
        try
        {
            var totalAmount = request.TransferList.Sum(x => x.Amount) + (long)request.ApplicationFeeAmount;
            var options = new PaymentIntentCreateOptions
            {
                Amount = totalAmount,
                Currency = request.Currency,
                ApplicationFeeAmount = request.TransferList.Count == 1 ? request.ApplicationFeeAmount : null,
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                TransferGroup = request.GroupId
            };
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PaymentIntent for multiple transfers: {Message}", ex.Message);
            
            return null;
        }
    }

    public async Task<PaymentIntent?> GetPaymentIntentAsync(string paymentIntentId, string stripeAccountId)
    {
        try
        {
            var service = new PaymentIntentService();
            var requestOptions = new RequestOptions
            {
                StripeAccount = stripeAccountId
            };
            var paymentIntent = await service.GetAsync(paymentIntentId, null, requestOptions);

            return paymentIntent ?? null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving PaymentIntent {PaymentIntentId} for account {StripeAccountId}: {Message}",
                paymentIntentId, stripeAccountId, ex.Message);
            
            return null;
        }
    }

    public async Task<BalanceTransaction?> GetBalanceTransactionAsync(string id, string accountId)
    {
        try
        {
            var service = new BalanceTransactionService();
            var requestOptions = new RequestOptions
            {
                StripeAccount = accountId
            };
            var balanceTransaction = await service.GetAsync(id, null, requestOptions);

            return balanceTransaction ?? null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving balance transaction {Id} for account {AccountId}: {Message}",
                id, accountId, ex.Message);
            
            return null;
        }
    }

    public async Task<PaymentIntent?> CancelPaymentIntentAsync(string paymentIntentId, string stripeAccountId, string cancellationReason)
    {
        try
        {
            var service = new PaymentIntentService();

            var requestOptions = new RequestOptions
            {
                StripeAccount = stripeAccountId
            };

            var cancelOptions = new PaymentIntentCancelOptions
            {
                CancellationReason = string.IsNullOrEmpty(cancellationReason) ? "abandoned" : cancellationReason
            };

            var paymentIntent = await service.CancelAsync(paymentIntentId, cancelOptions, requestOptions);

            return paymentIntent ?? null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling PaymentIntent {PaymentIntentId} for account {StripeAccountId}: {Message}",
                paymentIntentId, stripeAccountId, ex.Message);
            
            return null;
        }
    }

    public async Task<Refund?> RefundPaymentIntentAsync(string paymentIntentId, string stripeAccountId)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId
            };
            var requestOptions = new RequestOptions
            {
                StripeAccount = stripeAccountId
            };
            var service = new RefundService();
            var refund = await service.CreateAsync(options, requestOptions);

            return refund ?? null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating refund for PaymentIntent {PaymentIntentId} for account {StripeAccountId}: {Message}",
                paymentIntentId, stripeAccountId, ex.Message);
            
            return null;
        }
    }

    public async Task<Transfer?> CreateTransferAsync(string stripeAccountId, StripePaymentTransferCreate request)
    {
        try
        {
            var options = new TransferCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Destination = stripeAccountId,
                TransferGroup = request.GroupId
            };
            var service = new TransferService();

            var transfer = await service.CreateAsync(options);

            if (transfer != null)
            {
                return transfer;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transfer for account {StripeAccountId}: {Message}", stripeAccountId, ex.Message);
        }

        return null;
    }

    public async Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentId, string stripeAccountId, string paymentMethod)
    {
        var service = new PaymentIntentService();

        var confirmOptions = new PaymentIntentConfirmOptions
        {
            PaymentMethod = paymentMethod,
        };

        var requestOptions = new RequestOptions
        {
            StripeAccount = stripeAccountId
        };

        try
        {
            var paymentIntent = await service.ConfirmAsync(paymentIntentId, confirmOptions, requestOptions);

            if (paymentIntent != null)
            {
                return paymentIntent;
            }
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Error confirming PaymentIntent: {PaymentIntentId} for account {StripeAccountId}",
                paymentIntentId, stripeAccountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error confirming PaymentIntent: {PaymentIntentId} for account {StripeAccountId}",
                paymentIntentId, stripeAccountId);
        }

        return null;
    }

}