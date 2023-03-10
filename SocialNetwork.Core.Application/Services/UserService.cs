﻿using AutoMapper;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Friends;
using SocialNetwork.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }   

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterBasicUserAsync(registerRequest, origin);

        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRquest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPasswordAsync(forgotRquest, origin);
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetRequest);

        }

        public async Task<List<UserFriendViewModel>> GetAllBasicUsersAsync()
        {
            var users = await _accountService.GetAllBasicUsersAsync();

            return _mapper.Map<List<UserFriendViewModel>>(users); 
            
        }

        public async Task<UserFriendViewModel> GetByUsernameBasicUserAsync(string name)
        {
            var users = await _accountService.GetByUsernameBasicUserAsync(name);

            return _mapper.Map<UserFriendViewModel>(users);

        }

        public async Task<UserFriendViewModel> GetByIdBasicUserAsync(string usernameId)
        {
            var users = await _accountService.GetByIdBasicUserAsync(usernameId);

            return _mapper.Map<UserFriendViewModel>(users);

        }
    }
}
