using hotel_booking_core.Interfaces;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto.PaymentDtos;
using hotel_booking_models;
using hotel_booking_utilities;
using hotel_booking_utilities.Exceptions;
using hotel_booking_utilities.PaymentGatewaySettings;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaystackPaymentHandler _paystack;
        private readonly FlutterwavePaymentHandler _flutterwave;

        public PaymentService(IUnitOfWork unitOfWork, PaystackPaymentHandler paystack, FlutterwavePaymentHandler flutterwave)
        {
            _unitOfWork = unitOfWork;
            _paystack = paystack;
            _flutterwave = flutterwave;
        }
        public async Task<bool> InitializePayment(decimal amount, Customer customer, string paymentService, string bookingId, string transactionRef)
        {

            Payment payment = new()
            {
                BookingId = bookingId,
                Amount = amount,
                MethodOfPayment = paymentService,
                TransactionReference = transactionRef,
                Status = "Pending"
            };

            await _unitOfWork.Payments.InsertAsync(payment);
            await _unitOfWork.Save();

            return true;
        }

        public async Task<bool> VerifyTransaction(string transactionRef, string paymentMethod, string transactionId = null)
        {
            if(paymentMethod.ToLower() == Payments.Paystack)
            {
                if(_paystack.VerifyTransaction(transactionRef).Data.Status == Payments.Success)
                {
                    return true;
                }
                return false;
            }
            else if (paymentMethod.ToLower() == Payments.Flutterwave)
            {
                var response = await _flutterwave.VerifyTransaction(transactionId);
                if(response.Data.Status == Payments.Successful)
                {
                    return true;
                }
                return false;
            }
            throw new PaymentException("Invalid Payment Method");
        }
    }
}
