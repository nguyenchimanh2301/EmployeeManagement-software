using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using MISA.Web.Infrastructure.Interface;
using AutoMapper;
using MISA.Web.Core.DTOs;
using System.Data;
using MISA.Web.Core.Interfaces.UnitOfWork;
using System.Data.Common;
using System.Data.SqlClient;

namespace MISA.Web.Infrastructure.Repositoty
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {

        #region Fields
        IUnitOfWork _unitOfWork;
        #endregion
        #region Constructor
        public EmployeeRepository(IMISADbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
        #region Method
        public override async Task<IEnumerable<DTOEmployee>> GetAllDTOAsync()
        {
            var sqlCommand = $"SELECT * from View_Employee";
            var res = await _unitOfWork.Connection.QueryAsync<DTOEmployee>(sql: sqlCommand, transaction: _unitOfWork.Transaction);
            return res;
        }
        public override async Task<string> GetMaxCode()
        {
            var procName = "Proc_GetMaxCodeEmployee";
            var res = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<string>(sql: procName, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return res;
        }
        /// <summary>
        /// Phân trang lấy từ dữ liệu từ View
        /// </summary>
        /// <param name="searchText">Chuỗi tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi </param>
        /// <param name="numberPage">Số trang</param>
        /// <returns>Danh sách dữ liệu đã lọc</returns>
        public override async Task<IEnumerable<object>> FilterData(string? searchText, int? pageSize, int? numberPage)
        {
           /* var parameters = new DynamicParameters();
            searchText = string.IsNullOrEmpty(searchText) ? null : searchText;
            var sqlCommand = $"Proc_GetEmployeeFillterPaging";
            parameters.Add("@searchText", searchText);
            parameters.Add("@pageSize", pageSize);
            parameters.Add("@offset", offset);
            var res = await _unitOfWork.Connection.QueryAsync<DTOEmployee>(sql: sqlCommand, param: parameters, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return res;*/
            var sqlCommand = $"SELECT * from View_Employee";
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(searchText))
            {

                sqlCommand += " WHERE EmployeeCode LIKE @search OR FullName LIKE @search OR PhoneNumber LIKE @search ";
            }
            parameters.Add("@search", $"%{searchText}%", System.Data.DbType.String);
            if (pageSize > 0 && numberPage > 0)
            {
                var startIndex = (numberPage - 1) * pageSize;
                sqlCommand += $" LIMIT {pageSize} OFFSET {startIndex} ";
            }
            {
                var res = await _unitOfWork.Connection.QueryAsync<DTOEmployee>(sql: sqlCommand, param: parameters, transaction: _unitOfWork.Transaction);
                return res;
            }
        }
        public override bool CheckEmployeeExistByCode(string? value)
        {
            string Command = $"SELECT * FROM Employee WHERE EmployeeCode = @code";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@code", value);
            var entity =  _unitOfWork.Connection.QueryFirstOrDefault<Employee>(sql: Command, param: parameters, transaction: _unitOfWork.Transaction);
            if (entity != null)
                return true;
            return false;
        }

        /*public override async Task<int> InsertMultiData(List<Employee> entities)
        {
            var sql = "INSERT INTO Employee ( EmployeeId, EmployeeCode, FullName) VALUES ";
            var build = "";
            foreach (var item in entities)
            {
                item.EmployeeId = Guid.NewGuid();
                build += $"('{item.EmployeeId}','{item.EmployeeCode}','{item.FullName}'),";
            }
            sql += build.Substring(0, build.Length - 1); ;
            var res = await _unitOfWork.Connection.ExecuteAsync(sql: sql, transaction: _unitOfWork.Transaction);
            return res;
        }*/
        #endregion

    }
}
