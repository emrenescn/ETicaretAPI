using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;

        public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings(){
                Audience = new List<string>{"693364831559-pq7p62mvlm2c684hborsvs4s9j7n9dri.apps.googleusercontent.com"}
            };
            var payload=await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
            UserLoginInfo userLoginInfo = new UserLoginInfo(request.Providers, payload.Subject,request.Providers);
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
            bool result =user!=null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                result = true;
                if (user == null)
                {
                    user = new()
                    {
                        Id=Guid.NewGuid().ToString(),
                        UserName=payload.Email,
                        Email=payload.Email,
                        NameSurname=payload.Name
                    };
                   IdentityResult identityResult= await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
                await _userManager.AddLoginAsync(user, userLoginInfo);
            else
                throw new Exception("Invalid External Authentication");

            Token token = _tokenHandler.CreateAccessToken(5);
            return new()
            {
                Token = token
            };
            
        }
    }
}
