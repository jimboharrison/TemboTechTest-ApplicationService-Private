using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.ProductOne
{
    public class Processor : IApplicationProcessor
    {
        public Task Process(Application application)
        {
            throw new NotImplementedException();
        }
    }
}
