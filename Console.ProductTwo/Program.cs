using Microsoft.Extensions.DependencyInjection;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Console.ProductTwo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IApplicationProcessor, Processor>()
               //further services to be added here including logging
               .BuildServiceProvider();

            //program logic
            var processor = serviceProvider.GetService<IApplicationProcessor>();

            //example call to processor
            await processor.Process(BuildDummyApplication());
        }

        private static Application BuildDummyApplication()
        {
            return new Application
            {
                Id = Guid.NewGuid(),
                Applicant = new User { },
                Payment = new Payment(new BankAccount
                {
                    AccountNumber = "12345678",
                    SortCode = "10-20-30"
                },
                 new Money("GBP", 100)
                 ),
                ProductCode = ProductCode.ProductTwo
            };
        }
    }
}
