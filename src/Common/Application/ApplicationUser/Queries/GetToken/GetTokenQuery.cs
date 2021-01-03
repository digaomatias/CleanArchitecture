﻿using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.ApplicationUser.Queries.GetToken
{
    public class GetTokenQuery :IRequestWrapper<LoginResponse>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class GetTokenQueryHandler : IRequestHandlerWrapper<GetTokenQuery, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public GetTokenQueryHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<LoginResponse>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.CheckUserPassword(request.Email, request.Password);

            if (user == null)
                return ServiceResult.Failed<LoginResponse>(ServiceError.ForbiddenError);


            return ServiceResult.Success(new LoginResponse
            {
                User = user,
                Token = _tokenService.CreateJwtSecurityToken(user.Id)
            });
        }

    }
}
