using BookApi.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookApi.Core.Services
{
    public interface IAuthService
    {
        string GenerateJWT(AppUser user, IList<string> roles);
        Task<SignInResult> Login(AppUser user, string password);
    }
}
