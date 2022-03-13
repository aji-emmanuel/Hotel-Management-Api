using hotel_booking_dto.PaymentDtos;
using hotel_booking_utilities.HttpClientService.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace hotel_booking_utilities.PaymentGatewaySettings
{
    public class FlutterwavePaymentHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _httpClientService;

        public FlutterwavePaymentHandler(IConfiguration configuration, IHttpClientService httpClientService)
        {
            _configuration = configuration;
            _httpClientService = httpClientService;
        }

        public async Task<FlutterwaveResponseDTO<FlutterwaveResponseDataDTO>> InitializePayment(FlutterwaveRequestDTO requestDTO)
        {
            var response = await _httpClientService.PostRequestAsync<FlutterwaveRequestDTO, FlutterwaveResponseDTO<FlutterwaveResponseDataDTO>>(
                    baseUrl: "https://api.flutterwave.com",
                    requestUrl: "v3/payments",
                    requestModel: requestDTO,
                    token: _configuration["Payment:FlutterwaveKey"]
                );
            return response;
        }

        public async Task<FlutterwaveResponseDTO<FlutterwaveVerifyResponseDataDTO>> VerifyTransaction(string transactionId)
        {
            try
            {
                var response = await _httpClientService.GetRequestAsync<FlutterwaveResponseDTO<FlutterwaveVerifyResponseDataDTO>>(
                baseUrl: "https://api.flutterwave.com",
                requestUrl: $"v3/transactions/{transactionId}/verify",
                token: _configuration["Payment:FlutterwaveKey"]
                );
                return response;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }
    }
}
