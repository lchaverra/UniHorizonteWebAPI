﻿using System.IdentityModel.Tokens.Jwt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Application.Wrappers;
using Domain.DTO;
using MediatR;
using Domain.Responses.GenericResponses;

namespace Application.Services.ValidatorJWT {

  public class GeneratorJWTRequest : GenericRequest<SignInData> { }

  public class GeneratorJWTRequestHandler : IRequestHandler<GeneratorJWTRequest, GenericResponse> {
    private readonly string secretKey;

    public GeneratorJWTRequestHandler(string secretKey) {
      this.secretKey = secretKey;
    }

    public string GenerateToken(string email, string password) {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(secretKey);

      var tokenDescriptor = new SecurityTokenDescriptor {

        Subject = new ClaimsIdentity(new[]{
          new Claim(ClaimTypes.NameIdentifier, email),
          new Claim(ClaimTypes.Name, password)
        }),

        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public Task<GenericResponse> Handle(GeneratorJWTRequest request, CancellationToken cancellationToken) {
      throw new NotImplementedException();
    }

    public ClaimsPrincipal ValidateToken(string token) {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(secretKey);

      var validationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
      };

      try {
        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        if(validatedToken is JwtSecurityToken jwtSecurityToken
          && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) {
          return principal;
        }
      } catch(Exception) {
        Console.WriteLine("Token Invalido");
      }

      return null;
    }

  }
}