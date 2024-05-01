using ETicaretAPI.Application.Exceptions;
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
                    Id=Guid.NewGuid().ToString(),
                    NameSurname = request.NameSurname,
                    UserName=request.UserName,
                    Email=request.Email
                },request.Password) ;
                CreateUserCommandResponse response= new CreateUserCommandResponse() { Succeed=result.Succeeded};
            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturuldu";
            else
                foreach(var error in result.Errors)
                {
                    response.Message += $"{error.Code}-{error.Description}\n";
                }
            return response;


            
        }
    }
}
