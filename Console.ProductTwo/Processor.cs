using Services.AdministratorTwo.Abstractions;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Validation;

namespace Console.ProductTwo
{
    internal class Processor : IApplicationProcessor
    {
        private readonly IAdministrationService _administrationService;
        private readonly IBus _bus;
        private readonly IKycService _kycService;

        private readonly IPaymentValidator _paymentValidator;
        private readonly IApplicantValidator _applicantValidator;

        public Processor(IBus bus, IKycService kycService, IPaymentValidator paymentValidator, IApplicantValidator applicantValidator)
        {
            _bus = bus;
            _kycService = kycService;
            _paymentValidator = paymentValidator;
            _applicantValidator = applicantValidator;
        }

        public async Task Process(Application application)
        {
            //More validation here Validation + Argument checking here 
            var isValidApplication = await ValidateApplication(application);

            var isVerified = await GetVerification(application.Applicant);
            if (!isVerified) return;

            var createInvestorResult = await _administrationService.CreateInvestorAsync(application.Applicant);

            if (!createInvestorResult.IsSuccess)
            {
                await _bus.PublishAsync(new InvestorCreationFailed(application.Id));
                return;
            }

            await _bus.PublishAsync(new InvestorCreated(application.Id, createInvestorResult.Value.ToString()));

            var createAccountResult = await _administrationService.CreateAccountAsync(createInvestorResult.Value, ProductCode.ProductTwo);

            if (!createAccountResult.IsSuccess) 
            {
                await _bus.PublishAsync(new AccountCreationFailed(createInvestorResult.Value.ToString(), ProductCode.ProductTwo));
                return;
            }

            await _bus.PublishAsync(new AccountCreated(createInvestorResult.Value.ToString(), ProductCode.ProductTwo, createAccountResult.Value.ToString()));

            var processPaymentResult = await _administrationService.ProcessPaymentAsync(createAccountResult.Value, application.Payment);

            if (processPaymentResult.IsSuccess) 
            {
                await _bus.PublishAsync(new PaymentFailed(createInvestorResult.Value.ToString(), createAccountResult.Value.ToString(), application.Id));
                return;
            }

            await _bus.PublishAsync(new ApplicationCompleted(application.Id));
        }

        private async Task<bool> ValidateApplication(Application application)
        {
            var error = "";
            var isValid = true;

            if (application.ProductCode != ProductCode.ProductTwo)
            {
                isValid = false;
                error = "Unable to process Application";
            }

            var isValidPayment = _paymentValidator.IsMinimumPayment(application.Payment.Amount.Amount, 0.99m);
            if (!isValidPayment)
            {
                isValid = false;
                error = "Does not meet minimum payment";
            }

            var isValidAge = _applicantValidator.IsValidAge(application.Applicant.DateOfBirth, 18, 50);
            if (!isValidAge)
            {
                isValid = false;
                error = "Invalid Date of Birth";
            }

            await _bus.PublishAsync(new ApplicationDenied(application.Id, error));
            return false;
        }

        private async Task<bool> GetVerification(User applicant)
        {
            if (applicant.IsVerified.HasValue && applicant.IsVerified.Value) return true;

            var kycResult = await _kycService.GetKycReportAsync(applicant);

            if (!kycResult.IsSuccess)
            {
                return false;
            }

            if (!kycResult.Value.IsVerified)
                await _bus.PublishAsync(new KycFailed(applicant.Id, kycResult.Value.Id));

            return kycResult.Value.IsVerified;
        }
    }
}
