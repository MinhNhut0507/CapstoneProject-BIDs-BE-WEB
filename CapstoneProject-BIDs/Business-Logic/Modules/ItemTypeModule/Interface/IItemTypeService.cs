﻿using Business_Logic.Modules.ItemTypeModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemTypeModule.Interface
{
    public interface IItemTypeService
    {
        public Task<ItemType> AddNewItemType(CreateItemTypeRequest ItemTypeCreate);

        public Task<ItemType> UpdateItemType(UpdateItemTypeRequest ItemTypeUpdate);

        public Task<ItemType> DeleteItemType(Guid? ItemTypeDeleteID);

        public Task<ICollection<ItemType>> GetAll();

        public Task<ItemType> GetItemTypeByID(Guid? id);

        public Task<ItemType> GetItemTypeByName(string Name);

    }
}
