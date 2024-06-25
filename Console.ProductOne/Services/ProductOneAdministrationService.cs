using Console.ProductOne.Models;
using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Console.ProductOne.Services
{
    /// <summary>
    /// This service acts as a client wrapper around the administration services.
    /// </summary>
    internal class ProductOneAdministrationService : IProductOneAdministrationService
    {
        private readonly IAdministrationService _administrationService;
        private readonly IBus _bus;

        public ProductOneAdministrationService(IAdministrationService administrationService, IBus bus)
        {
            _administrationService = administrationService;
            _bus = bus;
        }

        /// <summary>
        /// This method would be easily changable if we wanted to swap clients. We would just need to replace the below with 3 seperate calls to the client
        /// This probably does not need to live in common for now since it will only be used by the ProductOne
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public async Task<Result<CreateInvestorAndProcessPaymentResponse>> ProcessPayment(Application application)
        {
            var applicant = application.Applicant;
            var firstAddress = application.Applicant.Addresses.First();

            var request = new CreateInvestorRequest
            {
                FirstName = applicant.Forename,
                LastName = applicant.Surname,
                Addressline1 = firstAddress.Addressline1,
                Addressline2 = firstAddress.Addressline2,
                Addressline3 = firstAddress.Addressline3,
                PostCode = firstAddress.PostCode,
                DateOfBirth = applicant.DateOfBirth.ToString(),
                Product = application.ProductCode.ToString(),
                Nino = applicant.Nino,
                AccountNumber = application.Payment.BankAccount.AccountNumber,
                SortCode = application.Payment.BankAccount.SortCode,
                InitialPayment = (int)application.Payment.Amount.Amount // making an assumption here as unclear on requirement ( have noted in observations )
            };

            CreateInvestorResponse response; 

            try
            {
                response = _administrationService.CreateInvestor(request);
                return Result.Success(new CreateInvestorAndProcessPaymentResponse
                {
                    AccountId = response.AccountId,
                    InvestorId = response.InvestorId,
                    PaymentId = response.PaymentId,
                    Reference = response.Reference,
                    Error = null
                });
            }
            catch (AdministratorException ex)
            {
                //probably best to handle the domain events in the processor instead of here, but as an example...
                if (ex.Code == ErrorCodes.InvestorError)
                    await _bus.PublishAsync(new InvestorCreationFailed(applicant.Id));

                if (ex.Code == ErrorCodes.AccountError)
                    await _bus.PublishAsync(new AccountCreationFailed(applicant.Id, null, ProductCode.ProductTwo));

                if (ex.Code == ErrorCodes.PaymentError)
                    await _bus.PublishAsync(new PaymentFailed(null, null, application.Id));

                //will assume if any exception then the whole process has failed
                return Result.Failure<CreateInvestorAndProcessPaymentResponse>(new Error("system", ex.Code, "Admin service failed"));
            }
            
        }
    }
}
