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
            IPaymentValidator paymentValidator
            , IApplicantValidator applicantValidator)
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
            if (!isValidPayment) return; // would likely return a bad result or throw exception here to be caught and processed. Need to inform user why it failed

            var isValidAge = _applicantValidator.IsValidAge(application.Applicant.DateOfBirth, 18, 39);
            if(!isValidAge) return;

            throw new NotImplementedException();
        }
    }
}
