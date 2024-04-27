using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
           IdentityResult result= await _userManager.CreateAsync(
                new()
                {
                    NameSurname = request.NameSurname,
                    UserName=request.UserName,
                    Email=request.Email
                },request.Password) ;
            if (result.Succeeded)
                return new()
                {
                   Succeed=true,
                   Message="Kayıt Başarılı"
                };
            else


            
        }
    }
}
