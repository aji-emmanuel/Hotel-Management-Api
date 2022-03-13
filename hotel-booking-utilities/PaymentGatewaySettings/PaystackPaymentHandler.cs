using Microsoft.Extensions.Configuration;
using PayStack.Net;
using System;

namespace hotel_booking_utilities.PaymentGatewaySettings
{
    public class PaystackPaymentHandler
    {
        private readonly IConfiguration _configuration;
        private readonly string secret;

        public PayStackApi PayStack { get; set; }
        public PaystackPaymentHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            secret = _configuration["Payment:PaystackKey"];
            PayStack = new PayStackApi(secret);
        }
        public TransactionInitializeResponse InitializePayment(TransactionInitializeRequest request)
        {

            var response = PayStack.Transactions.Initialize(request);
            if (response.Status)
            {
                return response;
            }
            throw new ArgumentException(response.Message);
        }

        public TransactionVerifyResponse VerifyTransaction(string transactionRef)
        {
            var response = PayStack.Transactions.Verify(transactionRef);
            return response;
        }
    }
}
