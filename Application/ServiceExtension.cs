using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.SignUp;
using Application.Wrappers;

using Domain.Models;
using Domain.Responses.GenericResponses;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceExtension {
    public static void AddApplicationServices (this IServiceCollection services) {
      services.AddScoped<IRequestHandler<SignUpRequest, GenericResponse>, SignUpRequestHandler>();
    }
  }
}
