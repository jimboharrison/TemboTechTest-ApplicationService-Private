namespace Console.ProductOne.Models
{
    public class CreateInvestorAndProcessPaymentResponse
    {
        public string Error { get; set; }

        public string Reference { get; set; }

        public string InvestorId { get; set; }

        public string AccountId { get; set; }

        public string PaymentId { get; set; }
    }
}
