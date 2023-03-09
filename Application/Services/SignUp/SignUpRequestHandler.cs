using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Services.SignUp;
using Application.Wrappers;

using BCrypt.Net;

using Domain.Models;
using Domain.Responses.GenericResponses;

using MediatR;

using Persistence.Contexts;
using Persistence.Entities;
using Persistence.Repositories.UserRepository;

namespace Application.Services.SignUp
{

    public class SignUpRequest : GenericRequest<UserSignUpData> { }

    public class SignUpRequestHandler : IRequestHandler<SignUpRequest, GenericResponse>
    {
        private readonly IGenericRepository<TblUsuario> _repository;

        public SignUpRequestHandler(IGenericRepository<TblUsuario> repository)
        {
            _repository = repository;
        }

        public async Task<GenericResponse> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            var validator = new SignUpValidator(_repository);
            var responseFactory = new GenericResponseFactory();

            if (!await validator.ValidateEmail(request.Body))
            {
                return responseFactory.GetErrorValidation("Ya existe un usuario con ese email!");
            }
            else if (!await validator.ValidatePersonId(request.Body))
            {
                return responseFactory.GetErrorValidation("Ya existe un usuario con esa contraseña!");
            }
            else
            {
                var userDTO = new TblUsuario
                {
                    RowId = new Guid(),
                    Cedula = request.Body.personId,
                    Nombre = request.Body.name,
                    Apellido = request.Body.surname,
                    Password = request.Body.password,
                    Correo = request.Body.email,
                    Telefono = request.Body.phone,
                    Estado = true,
                    FechaCreacion = DateTime.Now
                };

                await _repository.AddAsync(userDTO);
                await _repository.SaveChangesAsync();

                return responseFactory.GetCreated(null);
            }
        }
    }
}
