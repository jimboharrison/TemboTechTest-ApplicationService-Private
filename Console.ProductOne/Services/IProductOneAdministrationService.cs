using Services.Common.Abstractions.Model;

namespace Console.ProductOne.Services
{
    internal interface IProductOneAdministrationService
    {
        Result<CreateInvestorAndProcessPaymentResponse> ProcessPayment(Application application);
    }
}
