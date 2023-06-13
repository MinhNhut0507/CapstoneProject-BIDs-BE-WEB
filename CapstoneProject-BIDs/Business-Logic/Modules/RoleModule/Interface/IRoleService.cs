﻿using Business_Logic.Modules.RoleModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.RoleModule.Interface
{
    public interface IRoleService
    {
        public Task<Role> AddNewRole(CreateRoleRequest RoleCreate);

        public Task<Role> UpdateRole(UpdateRoleRequest RoleUpdate);

        public Task<Role> DeleteRole(int RoleDeleteID);

        public Task<ICollection<Role>> GetAll();

        public Task<Role> GetRoleByID(int id);

        public Task<Role> GetRoleByName(string Name);

    }
}
