using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.Dtos.Email;
using SocialNetwork.Core.Application.Enums;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Persistence.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }   

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                response.HasError = true;
                response.Error = $"There is no account registered with {request.Email}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.Email}";
                return response;
            }

            if(!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"The email '{request.Email}' is not confirmed ";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.Username = user.UserName;
            response.ImagePath = user.ImagePath;

            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = roleList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"The username '{request.UserName}' has been already taken";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email); 
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"The email '{request.Email}' has been already registered";
                return response;
            }

            var user = new ApplicationUser 
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ImagePath = request.ImagePath,
                UserName = request.UserName,
                PhoneNumber = request.Phone

            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                var verificationUri = await SendVerificationEmailUri(user, origin);
                await _emailService.SendEmailAsync(new EmailRequest()
                {
                    To = user.Email,
                    Body = $"Please confirm your account by clicking this URL {verificationUri}",
                    Subject = "Confirm Account"
                });
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error has occurred tying to register the user {request.UserName}";
                return response;
            }

            return response;

        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return $"There is no account registered with this email {user.Email}";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error has occurred while trying to confirm the email '{user.Email}'";
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null)
            {
                response.HasError = true;
                response.Error = $"There is no account registered with the email '{request.Email}'";
                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);
            await _emailService.SendEmailAsync(new EmailRequest()
            {
                To = user.Email,
                Body = $"Please reset your password by clicking this URL {verificationUri}",
                Subject = "Reset Password"
            });

            return response;
        } 

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null )
            {
                response.HasError = true;
                response.Error = $"There is no account registered with this email '{request.Email}'";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }
            return response;

        }

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);    
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }

        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }

        public async Task<List<UserInfo>> GetAllBasicUsersAsync()
        {
            List<UserInfo> userInfo = new List<UserInfo>();
            UserInfo userAdd = new UserInfo();
            var allUserBasic =  await _userManager.Users.ToListAsync();

            foreach (var item in allUserBasic)
            {
                userAdd.Id = item.Id;
                userAdd.FirstName = item.FirstName;
                userAdd.LastName = item.LastName;
                userAdd.Username = item.UserName;

                userInfo.Add(userAdd);
            }

            return userInfo;
        }

        public async Task<UserInfo> GetByUsernameBasicUserAsync(string username)
        {
            UserInfo userInfo = new UserInfo();

            var userBasic = await _userManager.FindByNameAsync(username);

            if(userBasic != null)
            {
                userInfo.Id = userBasic.Id;
                userInfo.FirstName = userBasic.FirstName;
                userInfo.LastName = userBasic.LastName;
                userInfo.Username = userBasic.UserName;
            }

            else
            {
                userInfo = null;
            }

            return userInfo;
        }

        public async Task<UserInfo> GetByIdBasicUserAsync(string usernameId)
        {
            UserInfo userInfo = new UserInfo();

            var userBasic = await _userManager.FindByIdAsync(usernameId);

            if (userBasic != null)
            {
                userInfo.Id = userBasic.Id;
                userInfo.FirstName = userBasic.FirstName;
                userInfo.LastName = userBasic.LastName;
                userInfo.Username = userBasic.UserName;
            }

            else
            {
                userInfo = null;
            }

            return userInfo;
        }
    }

}
