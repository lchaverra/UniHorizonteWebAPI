﻿using System.IdentityModel.Tokens.Jwt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Application.Wrappers;
using Domain.DTO;
using MediatR;
using Domain.Responses.GenericResponses;
using Domain.Models;
using Application.Services.Tokens.TokenGenerator;

namespace Application.Services.Tokens.Generator {

  public class TokenGenerator : ITokenGenerator {
    private readonly string secretKey;

    public TokenGenerator(IConfiguration configuration) {
      secretKey = configuration.GetSection("SecretKey").Value ?? string.Empty;
    }

    public string GenerateToken(UserCredential credentials) {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(secretKey);

      var tokenDescriptor = new SecurityTokenDescriptor {

        Subject = new ClaimsIdentity(new[] {
          new Claim(ClaimTypes.Email, credentials.email),
          new Claim(ClaimTypes.Name, credentials.name),
          new Claim(ClaimTypes.Surname, credentials.surname)
        }),

        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}