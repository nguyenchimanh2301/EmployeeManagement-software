using AutoMapper;
using Dapper;
using MISA.Web.Core.Atribute;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Infrastructure.Interface;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MISA.Web.Infrastructure.Repositoty
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        #region field
        IMISADbContext _dbContext;
        #endregion
        #region constructor
        public BaseRepository(IMISADbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Method
        #region  Lấy tất cả dữ liệu
        public  async Task<IEnumerable<MISAEntity>> GetAllAsync()
        {
            var res = await _dbContext.GetAll<MISAEntity>();
            return res;
        }
        #endregion
        #region Lấy dữ liệu theo Id
        public async Task<MISAEntity> GetByid(Guid entityId)
        {
            var res = await _dbContext.GetByid<MISAEntity>(entityId);
            return res;
        }
        #endregion
        #region Thêm dữ liệu
        public async Task<int> InsertData(MISAEntity entity)
        {

            var res = await _dbContext.InsertData(entity);
            return res;
        }
        #endregion
        #region Cập nhật dữ liệu
        public async Task<int> Update(MISAEntity entity, Guid Id)
        {
            var res = await _dbContext.Update(entity, Id);
            return res;

        }
        #endregion
        #region Xóa nhiều
        public async Task<int> DeleteMultiple(List<Guid> entityId)
        {
            var res = await _dbContext.DeleteMultiple<MISAEntity>(entityId);
            return res;
        }
        #endregion

        #region Kiểm tra trùng mã
        public virtual bool CheckEmployeeExistByCode(string? value)
        {
            return true;
        }
        #endregion
        #region Xóa bản ghi
        public async Task<int> Delete(Guid entityId)
        {

            var res = await _dbContext.Delete<MISAEntity>(entityId);
            return res;
        }
        #endregion

        #region Phân trang DTO
        public virtual async Task<IEnumerable<object>> FilterData(string? searchText, int? pageSize, int? numberPage)
        {
            return new List<object>();
        }
        #endregion
        #region lấy về mã lớn nhất

        public virtual  async Task<string> GetMaxCode()
        {
            return await Task.FromResult("");
        }
        #endregion
        #region lấy về danh sách DTO
        public virtual async Task<IEnumerable<DTOEmployee>> GetAllDTOAsync()
        {
            return Enumerable.Empty<DTOEmployee>();
        }
        #endregion
        #region lấy về Id theo tên

        public virtual async Task<object> GetDataByName(string? name)
        {
            return new Guid();
        }

        public  async Task<int> InsertMultiData(List<MISAEntity> entity)
        {
            var res = await _dbContext.InsertMultiData(entity);
            return res;
        }
        #endregion
        #endregion

    }

}
