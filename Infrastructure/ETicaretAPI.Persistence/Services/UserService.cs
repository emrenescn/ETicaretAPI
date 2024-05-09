using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    NameSurname = model.NameSurname,
                    UserName = model.UserName,
                    Email = model.Email
                }, model.Password);
            CreateUserResponse response = new () { Succeed = result.Succeeded };
            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturuldu";
            else
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code}-{error.Description}\n";
                }
            return response;
        }
    }
}
