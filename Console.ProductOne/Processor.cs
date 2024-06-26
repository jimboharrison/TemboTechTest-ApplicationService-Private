﻿using Console.ProductOne.Services;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Console.ProductOne
{
    internal class Processor : IApplicationProcessor
    {
        private readonly IProductOneAdministrationService _administrationService;
        private readonly IBus _bus;
        private readonly IKycService _kycService;

        private readonly IPaymentValidator _paymentValidator;
        private readonly IApplicantValidator _applicantValidator;

        public Processor(IProductOneAdministrationService administrationService,
            IBus bus,
            IKycService kyycService,
            IPaymentValidator paymentValidator,
            IApplicantValidator applicantValidator)
        {
            _administrationService = administrationService ?? throw new NullReferenceException(nameof(administrationService));
            _bus = bus ?? throw new NullReferenceException(nameof(bus));
            _kycService = kyycService ?? throw new NullReferenceException(nameof(kyycService));
            _paymentValidator = paymentValidator ?? throw new NullReferenceException(nameof(paymentValidator));
            _applicantValidator = applicantValidator ?? throw new NullReferenceException(nameof(applicantValidator));
        }

        public async Task Process(Application application)
        {
            //More validation here Validation + Argument checking here 
            var isValidApplication = await ValidateApplication(application);
            if (!isValidApplication) return;

            var isVerified = await GetVerification(application.Applicant);

            if (!isVerified) return;

            var processPaymentResponse =  await _administrationService.ProcessPayment(application);

            if (!processPaymentResponse.IsSuccess)
                return;

            //would be nicer to merge these into a single event for product one but will keep them split since product two does individually
            await _bus.PublishAsync(new InvestorCreated(application.Applicant.Id, processPaymentResponse.Value.InvestorId));
            await _bus.PublishAsync(new AccountCreated(processPaymentResponse.Value.InvestorId.ToString(), ProductCode.ProductOne, processPaymentResponse.Value.AccountId.ToString()));
            await _bus.PublishAsync(new ApplicationCompleted(application.Id));
        }

        private async Task<bool> ValidateApplication(Application application) {
            
            var error = "";
            var isValid = true;

            if (application.ProductCode != ProductCode.ProductOne)
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

            var isValidAge = _applicantValidator.IsValidAge(application.Applicant.DateOfBirth, 18, 39);
            if (!isValidAge)
            {
                isValid = false;
                error = "Invalid Date of Birth";
            }

            if (isValid) return true;

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
