using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.Services.Authentications;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly IExternalAuthentication _externalAuthentication;
        public FacebookLoginCommandHandler(IExternalAuthentication externalAuthentication)
        {
            _externalAuthentication = externalAuthentication;
        }
        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
           Token token= await _externalAuthentication.FacebookLoginAsync(request.AuthToken,900);
            return new()
            {
                Token = token
            };
           
        }
    }
}
