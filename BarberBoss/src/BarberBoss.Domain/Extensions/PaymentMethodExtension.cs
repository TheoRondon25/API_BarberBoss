using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports.PaymentMethodResource;

namespace BarberBoss.Domain.Extensions;
public static class PaymentMethodExtension
{
    public static string PaymentMethodToString(this PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.Cash => ResourceReportPaymentMethod.CASH,
            PaymentMethod.CreditCard => ResourceReportPaymentMethod.CREDIT_CARD,
            PaymentMethod.DebitCard => ResourceReportPaymentMethod.DEBIT_CARD,
            PaymentMethod.EletronicTransfer => ResourceReportPaymentMethod.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}
