using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Interfaces.Services
{
    /// <summary>
    /// Interface các hàm thực hiện công việc xử lý nghệp vụ sử dụng ở tầng Core 
    /// như Validate , kiểm tra trùng lặp,
    /// </summary>
    /// <typeparam name="MISAEntity"></typeparam>
    /// CreatedBy NC Mạnh (25/12/2023)
    public interface IBaseService<MISAEntity>
    {
        /// <summary>
        /// Hàm thêm dữ liệu vào Database
        /// </summary>
        /// <param name="entity">Dữ liệu đối tượng thêm vào</param>
        /// <returns>Trả về số bản ghi thêm vào </returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        /// 
        Task<int> Insert(MISAEntity entity);
        /// <summary>
        /// Hàm cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cập nhật</param>
        /// <param name="Id">Id đối tượng cần cập nhật</param>
        /// <returns>Số bản ghi</returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        Task<int> UpdateService(MISAEntity entity, Guid id);
        /// <summary>
        /// Hàm xóa nhiều dữ liệu
        /// </summary>
        /// <typeparam name="MISAEntity">Lớp dối tượng muốn xóa </typeparam>
        /// <param name="entityId"> Danh sách các id cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy NC Mạnh (04/01/2024)
        Task<int> DeleteMultiple(List<Guid> entityId);
        /// <summary>
        /// Hàm xóa  dữ liệu
        /// </summary>
        /// <typeparam name="MISAEntity">Lớp dối tượng muốn xóa</typeparam>
        /// <param name="entityId"> Danh sách các id cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy NC Mạnh (04/01/2024)
        Task<int> Delete(Guid entityId);
        /// <summary>
        /// Hàm kiểm tra trùng mã 
        /// </summary>
        /// <param name="code">Giá trị kiểm tra</param>
        /// <param name="Param">Tên trường kiểm tra</param>
        /// <returns>Đã tồn tại-true; chưa tồn tại-false</returns>
        /// CreatedBy NC Manh (03/01/2024)
        bool CheckValueExistByField(string? value);
      


    }
}
