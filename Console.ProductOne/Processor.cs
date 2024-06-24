using Services.AdministratorOne.Abstractions;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Validation;

namespace Console.ProductOne
{
    internal class Processor : IApplicationProcessor
    {
        private readonly IAdministrationService _administrationService;
        private readonly IBus _bus;
        private readonly IKycService _kycService;

        private readonly IPaymentValidator _paymentValidator;
        private readonly IApplicantValidator _applicantValidator;

        public Processor(IAdministrationService administrationService,
            IBus bus,
            IKycService kyycService,
            IPaymentValidator paymentValidator,
            IApplicantValidator applicantValidator)
        {
            _administrationService = administrationService ?? throw new NullReferenceException(nameof(administrationService));
            _bus = bus ?? throw new NullReferenceException(nameof(bus));
            _kycService = kyycService ?? throw new NullReferenceException(nameof(kyycService));
            _paymentValidator = paymentValidator;
            _applicantValidator = applicantValidator;
        }

        public async Task Process(Application application)
        {
            //More validation here Validation + Argument checking here 

            var isValidPayment = _paymentValidator.IsMinimumPayment(application.Payment.Amount.Amount, 0.99m);
            if (!isValidPayment) {
                await _bus.PublishAsync(new ApplicationDenied(application.Id, "Does not meet minimum payment"));
                return; 
            }

            var isValidAge = _applicantValidator.IsValidAge(application.Applicant.DateOfBirth, 18, 39);
            if (!isValidAge)
            {
                await _bus.PublishAsync(new ApplicationDenied(application.Id, "Invalid Date of Birth"));
                return;
            }

            var isVerified = await GetVerification(application.Applicant);

            if (!isVerified) 
            {
                await _bus.PublishAsync(new ApplicationDenied(application.Id, "KYC checks failed"));
                return;
            }

            

            throw new NotImplementedException();
        }

        private async Task<bool> GetVerification(User applicant)
        {
            if (applicant.IsVerified.HasValue && applicant.IsVerified.Value) return true;

            var kycResult = await _kycService.GetKycReportAsync(applicant);

            if (!kycResult.IsSuccess)
            {
                //log something here to indicate
                return false;
            }

            return kycResult.Value.IsVerified;
        }
    }
}
