using Console.ProductOne.Models;
using Services.Common.Abstractions.Model;

namespace Console.ProductOne.Services
{
    internal interface IProductOneAdministrationService
    {
        Task<Result<CreateInvestorAndProcessPaymentResponse>> ProcessPayment(Application application);
    }
}
