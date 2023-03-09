using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Wrappers;

using Domain.Models;

using Persistence.Entities;
using Persistence.Repositories.Specifications.User;
using Persistence.Repositories.UserRepository;

namespace Application.Services.SignUp
{
    public class SignUpValidator
    {

        private readonly IGenericRepository<TblUsuario> _repository;

        public SignUpValidator(IGenericRepository<TblUsuario> repository)
        {
            _repository = repository;
        }

        public async Task<bool> ValidatePersonId(UserSignUpData signUpData)
        {
            var getByPersonIdSpecification = new UserByPersonIdSpecification(signUpData.personId);
            return !await _repository.AnyAsync(getByPersonIdSpecification);
        }

        public async Task<bool> ValidateEmail(UserSignUpData signUpData)
        {
            var getByEmailSpecification = new UserByEmailSpecification(signUpData.email);
            return !await _repository.AnyAsync(getByEmailSpecification);
        }

    }
}
