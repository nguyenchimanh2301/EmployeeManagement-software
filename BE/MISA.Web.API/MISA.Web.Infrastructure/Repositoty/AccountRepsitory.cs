using Dapper;
using Microsoft.AspNetCore.Identity;
using MISA.Web.Core.Authentication;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.UnitOfWork;
using MISA.Web.Core.Resource;
using MISA.Web.Infrastructure.Interface;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MISA.Web.Infrastructure.Repositoty
{
    public class AccountRepsitory : BaseRepository<Account>, IAcountRepsitory
    {
        #region Fields
        IUnitOfWork _unitOfWork;
        #endregion
        #region Constructor

        public AccountRepsitory(IMISADbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckAccountExist(DTOAccount account)
        {
            string command = $"SELECT * FROM Account WHERE Username = @name  AND Password = @pass " ;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@name", account.Username);
            parameters.Add($"@pass", account.Password);
            var entity = _unitOfWork.Connection.QueryFirstOrDefault<Employee>(sql: command, param: parameters, transaction: _unitOfWork.Transaction);
            if (entity != null)
                return true;
            return false;
        }
        #endregion
        #region Method

        public override bool CheckEmployeeExistByCode(string? value)
        {
            string command = $"SELECT * FROM Account WHERE Username = @name";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@name", value);
            var entity = _unitOfWork.Connection.QueryFirstOrDefault<Employee>(sql: command, param: parameters, transaction: _unitOfWork.Transaction);
            if (entity != null)
                return true;
            return false;
        }

        public async Task<Account> GetByUsername(string  username)
        {
            var parameters = new DynamicParameters();
            string procName = "Proc_GetAccount_ByUserName";
            parameters.Add("@p_Username", username);
            var res = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<Account>(sql: procName, param: parameters, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return res;
        }

        public Task<object> Login(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public Task<object> RefreshToken(TokenModel tokenModel)
        {
            throw new NotImplementedException();
        }

        public Task<object> Register(RegisterModel model)
        {
            throw new NotImplementedException();
        }

        public Task<object> RegisterAdmin(RegisterModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Revoke(string username)
        {

            var errors = new Dictionary<string, List<string>>();
            var user = await GetByUsername(username);
            if (user == null)
            {
                errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountIsNotExist });
                throw new MISAValidateException(errors);
            };
            user.RefreshToken = null;
            var res =  await UpdateAsync(user);
            return res;

        }

        public async Task<object> UpdateAsync(Account account)
        {
            var parameters = new DynamicParameters();
            string procName = "Proc_Update_Account";
            parameters.Add("@p_Username", account.Username);
            parameters.Add("@p_RefreshToken", account.RefreshToken);
            parameters.Add("@p_AccessToken", account.AccessToken);
            parameters.Add("@p_Expiration", account.Expiration);
            var res = await _unitOfWork.Connection.ExecuteAsync(sql: procName, param: parameters, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return res;
        }
        #endregion

    }
}
