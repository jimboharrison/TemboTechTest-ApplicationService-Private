using Services.AdministratorOne.Abstractions;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Console.ProductOne
{
    internal class Processor : IApplicationProcessor
    {
        private readonly IAdministrationService _administrationService;
        private readonly IBus _bus;
        private readonly IKycService _kycService;

        public Processor(IAdministrationService administrationService, IBus bus, IKycService kyycService)
        {
            _administrationService = administrationService ?? throw new NullReferenceException(nameof(administrationService));
            _bus = bus ?? throw new NullReferenceException(nameof(bus));
            _kycService = kyycService ?? throw new NullReferenceException(nameof(kyycService));
        }
        
        public async Task Process(Application application)
        {
            //More validation here Validation + Argument checking here 

            var kycResult = await _kycService.GetKycReportAsync(application.Applicant);

            throw new NotImplementedException();
        }
    }
}
