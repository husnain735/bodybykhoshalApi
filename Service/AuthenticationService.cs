﻿using AutoMapper;
using Azure.Core;
using bodybykhoshalApi.Context;
using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.HttpRequestHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static bodybykhoshalApi.Models.ViewModel.HttpResponse;

namespace bodybykhoshalApi.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthenticationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public string CreateUser(RegisterRequestHandler requestHandler)
        {
            try
            {
                var checkEmail = _dbContext.Users.Where(x => x.Email.Equals(requestHandler.Email) && !x.IsDeleted).FirstOrDefault();
                if (checkEmail == null)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(requestHandler.Password);
                    Guid newGuid = Guid.NewGuid();

                    var user = new Users
                    {
                        FirstName = requestHandler.FirstName,
                        LastName = requestHandler.LastName,
                        Email = requestHandler.Email,
                        PasswordHash = hashedPassword,
                        PhoneNumber = requestHandler.PhoneNumber,
                        UserGUID = newGuid.ToString(),
                        RoleId = requestHandler.RoleId,
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                    return "User Register";
                }
                return "Email Already Exists";
            }
            catch (Exception)
            {

                throw;
            }
        }
        public LoginResponseHandler LoginUser(LoginRequestHandler requestHandler)
        {
            try
            {
                var response = new LoginResponseHandler();

                var checkEmailExists = _dbContext.Users.Where(x => x.Email.Equals(requestHandler.Email)).FirstOrDefault();

                if (checkEmailExists != null)
                {
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(requestHandler.Password, checkEmailExists.PasswordHash);

                    if (isPasswordCorrect)
                    {
                        var key = Encoding.ASCII.GetBytes("YourSecretKeyHere");
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.Name, checkEmailExists.FirstName + " " + checkEmailExists.LastName),
                            new Claim(ClaimTypes.NameIdentifier, checkEmailExists.UserGUID),
                            new Claim(ClaimTypes.Email, checkEmailExists.Email),
                            new Claim(ClaimTypes.Role, checkEmailExists.RoleId.ToString()),
                            }),
                            Expires = DateTime.UtcNow.AddDays(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);
                        response.Token = tokenString;
                        response.Success = true;
                        response.RoleId = checkEmailExists.RoleId;
                        return response;
                    }
                }
                response.Success = false;
                return response; 
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
