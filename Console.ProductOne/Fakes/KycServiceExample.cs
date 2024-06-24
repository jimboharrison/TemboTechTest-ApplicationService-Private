using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Console.ProductOne.Fakes
{
    /// <summary>
    /// Added to use an example for registering class in DI container
    /// </summary>
    internal class KycServiceExample : IKycService
    {
        public Task<Result<KycReport>> GetKycReportAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
