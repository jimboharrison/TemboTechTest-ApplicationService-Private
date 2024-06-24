namespace Services.Common.Abstractions.Validation
{
    public class PaymentValidator : IPaymentValidator
    {
        public bool IsMinimumPayment(decimal paymentAmount, decimal minimumPayment)
        {
            return paymentAmount >= minimumPayment;
        }
    }
}