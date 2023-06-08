﻿using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.BanHistoryModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.BanHistoryModule
{
    public class BanHistoryService : IBanHistoryService
    {
        private readonly IBanHistoryRepository _BanHistoryRepository;
        private readonly IUserRepository _UserRepository;
        public BanHistoryService(IBanHistoryRepository BanHistoryRepository, IUserRepository UserRepository)
        {
            _BanHistoryRepository = BanHistoryRepository;
            _UserRepository = UserRepository;
        }

        public async Task<ICollection<BanHistory>> GetAll()
        {
            return await _BanHistoryRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public Task<ICollection<BanHistory>> GetBanHistorysIsValid()
        {
            return _BanHistoryRepository.GetBanHistorysBy(x => x.Status == true, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<BanHistory> GetBanHistoryByUserID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BanHistory = await _BanHistoryRepository.GetFirstOrDefaultAsync(x => x.UserId == id);
            if (BanHistory == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return BanHistory;
        }

        public async Task<BanHistory> GetBanHistoryByUserName(string userName)
        {
            if (userName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var user = _UserRepository.GetFirstOrDefaultAsync(x => x.UserName == userName).Result;
            var BanHistory = await _BanHistoryRepository.GetFirstOrDefaultAsync(x => x.UserId == user.UserId);
            if (BanHistory == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return BanHistory;
        }

        public async Task<Guid?> AddNewBanHistory(CreateBanHistoryRequest BanHistoryRequest)
        {

            ValidationResult result = new CreateBanHistoryRequestValidator().Validate(BanHistoryRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newBanHistory = new BanHistory();

            newBanHistory.BanId = Guid.NewGuid();
            newBanHistory.UserId = BanHistoryRequest.UserId;
            newBanHistory.Reason = BanHistoryRequest.Reason;
            newBanHistory.CreateDate = DateTime.Now;
            newBanHistory.UpdateDate = DateTime.Now;
            newBanHistory.Status = true;

            await _BanHistoryRepository.AddAsync(newBanHistory);
            return newBanHistory.BanId;
        }

        public async Task UpdateBanHistory(UpdateBanHistoryRequest BanHistoryRequest)
        {
            try
            {
                var BanHistoryUpdate = _BanHistoryRepository.GetFirstOrDefaultAsync(x => x.BanId == BanHistoryRequest.BanHistoryId).Result;

                if (BanHistoryUpdate == null)
                {
                    throw new Exception(ErrorMessage.BanHistoryError.BAN_HISTORY_NOT_FOUND);
                }

                ValidationResult result = new UpdateBanHistoryRequestValidator().Validate(BanHistoryRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                BanHistoryUpdate.Reason = BanHistoryRequest.Reason;
                BanHistoryUpdate.UpdateDate = DateTime.Now;
                BanHistoryUpdate.Status = BanHistoryRequest.Status;

                await _BanHistoryRepository.UpdateAsync(BanHistoryUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task DeleteBanHistory(Guid? BanHistoryDeleteID)
        {
            try
            {
                if (BanHistoryDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                BanHistory BanHistoryDelete = _BanHistoryRepository.GetFirstOrDefaultAsync(x => x.BanId == BanHistoryDeleteID && x.Status == true).Result;

                if (BanHistoryDelete == null)
                {
                    throw new Exception(ErrorMessage.BanHistoryError.BAN_HISTORY_NOT_FOUND);
                }

                BanHistoryDelete.Status = false;
                await _BanHistoryRepository.UpdateAsync(BanHistoryDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
