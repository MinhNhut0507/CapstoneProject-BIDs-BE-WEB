﻿using Business_Logic.Modules.UserModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserModule.Interface
{
    public interface IUserService
    {
        public Task<Guid?> AddNewUser(CreateUserRequest UserCreate);

        public Task UpdateUser(UpdateUserRequest UserUpdate);

        public Task<ICollection<User>> GetAll();

        public Task<ICollection<User>> GetUsersIsActive();

        public Task<ICollection<User>> GetUsersIsBan();

        public Task<ICollection<User>> GetUsersIsWaitting();

        public Task<User> GetUserByID(Guid? id);

        public Task<User> GetUserByName(string Name);

    }
}
