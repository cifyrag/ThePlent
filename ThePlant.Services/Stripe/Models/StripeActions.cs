namespace ThePlant.Services.Stripe.Models;

public record StripeAccountLinkCreate(
    string Type,
    string SuccessUrl,
    string CancelUrl
    );

public record StripePaymentIntentCreate(
    long Amount,
    string Currency,
    long FeeAmount
    );

public record StripePaymentIntentMultipleCreate(
    long? ApplicationFeeAmount,
    string Currency,
    string GroupId,
    List<StripePaymentTransferCreate> TransferList 
    );

public record StripePaymentTransferCreate(
    long Amount,
    string Currency,
    string GroupId 
    );

public enum StripeActions
{
    AccountCreate = 1,
    AccountGet = 2,
    AccountList = 3,
    AccountUpdate = 4,
    AccountDelete = 5,
    AccountReject = 6,

    AccountLinkCreate = 7,

    PaymentIntentCreate = 10,
    PaymentIntentCreateMultiple = 11,
    PaymentIntentGet = 12,
    PaymentIntentCancel = 13,
    PaymentIntentRefund = 14,
    BalanceTransactionGet = 15,

    TransferCreate = 16
}

