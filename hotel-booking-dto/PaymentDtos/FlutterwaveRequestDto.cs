using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_dto.PaymentDtos
{
    public class FlutterwaveRequestDTO
    {
        [JsonProperty("currency")]
        public string Currency { get; set; } = "NGN";
        [JsonProperty("amount")]
        [Required]
        public decimal Amount { get; set; }
        [JsonProperty("tx_ref")]
        [Required]
        public string TransactionReference { get; set; }
        [JsonProperty("redirect_url")]
        [Required]
        public string RedirectUrl { get; set; }
        [JsonProperty("payment_options")]
        [Required]
        public List<string> PaymentOptions { get; set; }
        [JsonProperty("customer")]
        [Required]
        public FlutterwaveCustomerDTO Customer { get; set; }
    }

    public class FlutterwaveCustomerDTO
    {
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class FlutterwaveResponseDTO<T> where T : class
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class FlutterwaveResponseDataDTO
    {
        public string Link { get; set; }
    }

    public class FlutterwaveVerifyResponseDataDTO
    {
        [JsonProperty("id")]
        public string TransactionId { get; set; }
        [JsonProperty("tx_ref")]
        public string TransactionReference { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
