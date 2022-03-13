using AutoMapper;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_dto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.TokenDto;
using hotel_booking_models;
using hotel_booking_models.Mail;
using hotel_booking_utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using hotel_booking_utilities.EmailBodyHelper;
using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Asn1.Ocsp;
using PayStack.Net;

namespace hotel_booking_core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly IMailService _mailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly ITokenRepository _tokenRepository;


       


        public AuthenticationService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, ILogger logger,
            IMailService mailService, IMapper mapper, ITokenGeneratorService tokenGenerator, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
            _mailService = mailService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _tokenRepository = tokenRepository;

        }

        /// <summary>
        /// Confirms the mail of a registered user
        /// </summary>
        /// <param name="confirmEmailDto"></param>
        /// <returns></returns>
        public async Task<Response<string>> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
            var response = new Response<string>();
            if(user == null)
            {
                response.Message = "User not found";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            var decodedToken = TokenConverter.DecodeToken(confirmEmailDto.Token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Email Confirmation successful";
                response.Data = user.Id;
                response.Succeeded = true;
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Message = GetErrors(result);
            response.Succeeded = false;
            return response;            
        }

        /// <summary>
        /// Sends a reset password token to a user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response<string>> ForgotPassword(string email)
        {
            var response = new Response<string>();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.Message = $"An email has been sent to {email} if it exists";
                response.Succeeded = true;
                response.Data = null;
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = TokenConverter.EncodeToken(token);
            var userRole = await _userManager.GetRolesAsync(user);

            var mailBody = await EmailBodyBuilder.GetEmailBody(user, userRole.ToList(), emailTempPath: "StaticFiles/Html/ForgotPassword.html", linkName: "ResetPassword", encodedToken, controllerName: "Auth");


            var mailRequest = new MailRequest()
            {
                Subject = "Reset Password",
                Body = mailBody,
                ToEmail = email
            };

            var emailResult = await _mailService.SendEmailAsync(mailRequest);
            if (emailResult)
            {
                response.Succeeded = true;
                response.Message = $"An email has been sent to {email} if it exists";
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Data = email;
                return response;
            }

            response.Succeeded = false;
            response.Message = "Something went wrong. Please try again.";
            response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return response;
        }       
      
        public async Task<Response<bool>> ChangePassword(string id, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Response<bool>.Success("Password Changed successfully", true, StatusCodes.Status200OK);
            }
            return Response<bool>.Fail("Ensure that your old password is correct", StatusCodes.Status404NotFound); 

        }
        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<LoginResponseDto>> Login(LoginDto model)
        {
            var response = new Response<LoginResponseDto>();
            _logger.Information("Login Attempt");
            var validityResult = await ValidateUser(model);

            if (!validityResult.Succeeded)
            {
                _logger.Error("Login operation failed");
                response.Message = validityResult.Message;
                response.StatusCode = validityResult.StatusCode;
                response.Succeeded = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); //sets refresh token for 7 days
            var result = new LoginResponseDto()
            {
                Id = user.Id,
                Token = await _tokenGenerator.GenerateToken(user),
                RefreshToken = refreshToken
            };
            
            await _userManager.UpdateAsync(user);

            _logger.Information("User successfully logged in");
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Login Successfully";
            response.Data = result;
            response.Succeeded = true;
            return response;
        }

        /// <summary>
        /// Registers a new user and sends a confirmation link to the user's email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<string>> Register(RegisterUserDto model)
        {
            var user = _mapper.Map<AppUser>(model);
            user.IsActive = true;
            var response = new Response<string>();
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Customer);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = TokenConverter.EncodeToken(token);
                    var userRole = await _userManager.GetRolesAsync(user);
                    var mailBody = await EmailBodyBuilder.GetEmailBody(user, userRole.ToList(), emailTempPath: "StaticFiles/Html/ConfirmEmail.html", linkName: "ConfirmEmail", encodedToken, controllerName: "Authentication");
                    var mailRequest = new MailRequest()
                    {
                        Subject = "Confirm Your Registration",
                        Body = mailBody,
                        ToEmail = model.Email
                    };

                    bool emailResult = await _mailService.SendEmailAsync(mailRequest); //Sends confirmation link to users email
                    if (emailResult)
                    {
                        _logger.Information("Mail sent successfully");
                        var customer = new Customer
                        {
                            AppUser = user
                        };
                        await _unitOfWork.Customers.InsertAsync(customer);
                        await _unitOfWork.Save();
                        response.StatusCode = (int)HttpStatusCode.Created;
                        response.Succeeded = true;
                        response.Data = user.Id;
                        response.Message = "User created successfully! Please check your mail to verify your account.";
                        transaction.Complete();
                        return response;
                    }
                    _logger.Information("Mail service failed");
                    transaction.Dispose();
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Succeeded = false;
                    response.Message = "Registration failed. Please try again";
                    return response;
                }
                response.Message = GetErrors(result);
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Succeeded = false;
                transaction.Complete();
                return response;
            };

        }

        /// <summary>
        /// Reset the password of a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<string>> ResetPassword(ResetPasswordDto model)
        {
            var response = new Response<string>();
            _logger.Information("Reset password attempt");
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                response.Message = "Invalid user!";
                response.Succeeded = false;
                response.Data = null;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }

            var decodedToken = TokenConverter.DecodeToken(model.Token); //Decode incoming token

            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var tokenProvider = _userManager.Options.Tokens.PasswordResetTokenProvider;

            var isValidToken = await _userManager.VerifyUserTokenAsync(user, tokenProvider, purpose, decodedToken);
            if (isValidToken)
            {
                _mapper.Map<AppUser>(model);
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
                response.Succeeded = true;
                response.Data = user.Id;
                response.Message = "Password has been reset successfully";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }

            response.StatusCode = StatusCodes.Status400BadRequest;
            response.Message = "Invalid Token";
            response.Succeeded = false;
            return response;
        }

        /// <summary>
        /// Update the password of a logged in user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<string>> UpdatePassword(UpdatePasswordDto model)
        {
            var response = new Response<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                response.Message = "Opps! something went wrong.";
                response.Succeeded = false;
                response.Data = null;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            _mapper.Map<AppUser>(model);
            response.Message = "Password has been changed successfully";
            response.Succeeded = true;
            response.Data = user.Id;
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }

        /// <summary>
        /// Validates a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns true if the user exists</returns>
        private async Task<Response<bool>> ValidateUser(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var response = new Response<bool>();
            if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.Message = "Invalid Credentials";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            if(!await _userManager.IsEmailConfirmedAsync(user) && user.IsActive)
            {
                response.Message = "Account not activated";
                response.Succeeded = false;
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                return response;
            }
            else
            {
                response.Succeeded = true;
                return response;
            }
        }

        /// <summary>
        /// Stringify and returns all the identity errors
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Identity Errors</returns>
        private static string GetErrors(IdentityResult result)
        {
            return result.Errors.Aggregate(string.Empty, (current, err) => current + err.Description + "\n");
        }

        
      

        public async Task<Response<RefreshTokenToReturnDto>> RefreshToken(RefreshTokenRequestDto token)
        {
            var response = new Response<RefreshTokenToReturnDto>();
            var refreshToken = token.RefreshToken;
            var userId = token.UserId;

            var user = await _tokenRepository.GetUserByRefreshToken(refreshToken, userId);

            if (user.RefreshToken != refreshToken|| user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                response.Data = null;
                response.Message = "Invalid request";
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                response.Succeeded = false;
                return response;
            }

            var result = new RefreshTokenToReturnDto
            {
                NewJwtAccessToken = await _tokenGenerator.GenerateToken(user),
                NewRefreshToken = _tokenGenerator.GenerateRefreshToken()
            };
            user.RefreshToken = result.NewRefreshToken;
            await _userManager.UpdateAsync(user);

            response.Data = result;
            response.Message = "Token Successfully refreshed";
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Succeeded = true;
            return response;
        }

        public async Task<bool> ValidateUserRole(string userId, string[] roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool result = false;
            foreach (var role in userRoles)
            {
                if (roles.Contains(role))
                {
                    result = true;
                    break;
                }
            }
            _logger.Information($"User with ID {userId} was {(result ? "Validated" : "Not validated")}");
            return result;
        }
    }
}
