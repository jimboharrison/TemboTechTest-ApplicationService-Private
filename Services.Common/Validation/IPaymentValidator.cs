using Services.Common.Abstractions.Model;

namespace Services.Common.Abstractions.Validation
{
    public interface IPaymentValidator
    {
        bool IsMinimumPayment(decimal paymentAmount, decimal minimumPayment);
    }
}
