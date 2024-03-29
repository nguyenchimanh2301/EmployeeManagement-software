using MISA.Web.Core.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Infrastructure.Interface
{
     public interface IMISADbContext
    {
        #region Property   
        /// <summary>
        /// Chuỗi kết nối
        /// </summary>
        IDbConnection Connection { get; }
        #endregion
        #region Method

        /// <summary>
        /// Hàm lấy về tất cả dữ liệu
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// CreatedBy NC Mạnh (03/01/2024)
    /*    IEnumerable<MISAEntity> GetAll<MISAEntity>();*/
        Task<IEnumerable<MISAEntity>>GetAll<MISAEntity>();
        /// <summary>
        /// Hàm lấy về thông tin đối tượng theo Id
        /// </summary>
        /// <param name="entityId">Id của đôi tượng</param>
        /// <returns>Trả về thông tin của đối tượng</returns>
        /// CreatedBy NC Mạnh (03/01/2024)
        Task<MISAEntity> GetByid<MISAEntity>(Guid entityId);
        /// <summary>
        /// Hàm thêm dữ liệu vào Database
        /// </summary>
        /// <param name="entity">Dữ liệu đối tượng thêm vào</param>
        /// <returns>Trả về số bản ghi thêm vào </returns>
        /// CreatedBy NC Mạnh (03/01/2024)
        /// 
        Task<int> InsertData<MISAEntity>(MISAEntity entity);
        /// <summary>
        /// Hàm thêm nhiều dữ liệu vào Database
        /// </summary>
        /// <param name="entity">Dữ liệu đối tượng thêm vào</param>
        /// <returns>Trả về số bản ghi thêm vào </returns>
        /// CreatedBy NC Mạnh (03/01/2024)
        /// 
        Task<int> InsertMultiData<MISAEntity>(List<MISAEntity> entity);
        /// <summary>
        /// Hàm cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cập nhật</param>
        /// <param name="Id">Id đối tượng cần cập nhật</param>
        /// <returns>Số bản ghi</returns>
        /// CreatedBy NC Mạnh (03/01/2024)
        Task<int> Update <MISAEntity>(MISAEntity entity, Guid entityId);
        /// <summary>
        /// Hàm xóa dữ liệu
        /// CreatedDate 26/12/2023
        /// </summary>
        /// <param name="Id">Id đối tượng cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy NC Mạnh (03/01/2024)
        Task<int> Delete <MISAEntity>(Guid entityId);
        /// <summary>
        /// Hàm xóa nhiều dữ liệu
        /// </summary>
        /// <typeparam name="MISAEntity">Lớp dối tượng muốn xóa </typeparam>
        /// <param name="entityId"> Danh sách các id cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy NC Mạnh (04/01/2024)
        Task<int> DeleteMultiple<MISAEntity>(List<Guid> entityId);
        /// <summary>
        /// Hàm lấy về thông tin đối tượng theo tên
        /// </summary>
        /// <param name="name">tên của đôi tượng</param>
        /// <returns>Trả về thông tin của đối tượng</returns>
        /// CreatedBy NC Mạnh (01/3/2024)
        Task<Guid> GetDataByName<MISAEntity>(string? name);

        #endregion
    }
}
