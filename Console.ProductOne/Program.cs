using Console.ProductOne.Fakes;
using Microsoft.Extensions.DependencyInjection;
using Services.Common.Abstractions.Abstractions;

namespace Console.ProductOne
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IKycService, KycServiceExample>()
                .AddScoped<IApplicationProcessor, Processor>()
                //further services to be added here including logging
               .BuildServiceProvider();

            //program logic
            var processor = serviceProvider.GetService<IApplicationProcessor>();

            //example call to processor
            await processor.Process(BuildDummyApplication());
        }

        private static Services.Common.Abstractions.Model.Application BuildDummyApplication()
        {
            return new Services.Common.Abstractions.Model.Application
            {
                Id = Guid.NewGuid(),
                Applicant = new Services.Common.Abstractions.Model.User { },
                Payment = new Services.Common.Abstractions.Model.Payment(new Services.Common.Abstractions.Model.BankAccount
                {
                    AccountNumber = "12345678",
                    SortCode = "10-20-30"
                },
                 new Services.Common.Abstractions.Model.Money("GBP", 100)
                 ),
                ProductCode = Services.Common.Abstractions.Model.ProductCode.ProductOne
            };
        }
    }
}
