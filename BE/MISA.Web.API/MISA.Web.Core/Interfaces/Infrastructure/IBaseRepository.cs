using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Interfaces.Infrastructure
{
    /// <summary>
    /// Interface các hàm thực hiện công việc thao tác với Database sử dụng ở tầng Core 
    /// trả về danh sách bản ghi,số bản ghi khi thực hiện thao tác dữ liệu
    /// </summary>
    /// <typeparam name="MISAEntity">Lớp đối tượng </typeparam>
    /// CreatedBy NC Mạnh (25/12/2023)
    public interface IBaseRepository<MISAEntity>
    {
        /// <summary>
        /// Hàm lấy về tất cả dữ liệu
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        /* IEnumerable<MISAEntity> GetAll();*/
        Task<IEnumerable<MISAEntity>> GetAllAsync();
        /// <summary>
        /// Hàm lấy về tất cả dữ liệu DTO
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// CreatedBy NC Mạnh (01/03/2023)
        /* IEnumerable<MISAEntity> GetAll();*/
        Task<IEnumerable<DTOEmployee>> GetAllDTOAsync();
        /// <summary>
        /// Hàm lấy về thông tin đối tượng theo tên
        /// </summary>
        /// <param name="name">tên của đôi tượng</param>
        /// <returns>Trả về thông tin của đối tượng</returns>
        /// CreatedBy NC Mạnh (01/3/2024)
        Task<object> GetDataByName(string? name);
        /// <summary>
        /// Hàm lấy về thông tin đối tượng theo Id
        /// </summary>
        /// <param name="entityId">Id của đôi tượng</param>
        /// <returns>Trả về thông tin của đối tượng</returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        Task<MISAEntity> GetByid(Guid entityId);
        /// <summary>
        /// Hàm thêm dữ liệu vào Database
        /// </summary>
        /// <param name="entity">Dữ liệu đối tượng thêm vào</param>
        /// <returns>Trả về số bản ghi thêm vào </returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        /// 
        Task<int> InsertData(MISAEntity entity);
        /// <summary>
        /// Hàm thêm nhiều dữ liệu vào Database
        /// </summary>
        /// <param name="entity">Dữ liệu đối tượng thêm vào</param>
        /// <returns>Trả về số bản ghi thêm vào </returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        /// 
        Task<int> InsertMultiData(List<MISAEntity> entity);
        /// <summary>
        /// Hàm cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cập nhật</param>
        /// <param name="Id">Id đối tượng cần cập nhật</param>
        /// <returns>Trả về số bản ghi thêm thành công</returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        Task<int> Update(MISAEntity entity, Guid entityId);
        /// <summary>
        /// Hàm xóa dữ liệu
        /// CreatedDate 26/12/2023
        /// </summary>
        /// <param name="Id">Id đối tượng cần xóa</param>
        /// <returns>Trả về số bản ghi xóa thành công</returns>
        /// CreatedBy NC Mạnh (25/12/2023)
        Task<int> Delete(Guid entityId);
        /// <summary>
        /// Hàm xóa nhiều dữ liệu
        /// </summary>
        /// <typeparam name="MISAEntity">Lớp dối tượng muốn xóa </typeparam>
        /// <param name="entityId"> Danh sách các id cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy NC Mạnh (04/01/2024)
        Task<int> DeleteMultiple(List<Guid> entityId);
        /// <summary>
        ///  Hàm lấy dữ liệu phân trang DTO
        /// </summary>
        /// <param name="pageSize">Số bản ghi</param>
        /// <param name="numberPage">Số trang</param>
        /// <returns>Danh sách bản ghi đã lọc</returns>
        /// CreatedBy NC Mạnh (30/1/2024)
        Task<IEnumerable<object>> FilterData(string? searchText, int? pageSize, int? numberPage);
        /// <summary>
        /// Hàm lọc dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        /// CreatedBy NC Mạnh (26/12/2023)
        /*     IEnumerable<MISAEntity> GetDataBy(string? data, string? column);*/
        /// <summary>
        /// Hàm kiểm tra trùng mã 
        /// </summary>
        /// <param name="code">Giá trị kiểm tra</param>
        /// <param name="Param">Tên trường kiểm tra</param>
        /// <returns>Đã tồn tại-true; chưa tồn tại-false</returns>
        /// CreatedBy NC Manh (03/01/2024)
        bool CheckEmployeeExistByCode(string? value);
        /// <summary>
        /// Hàm lấy về mã lớn nhất
        /// CreatedBy NC Mạnh (29/01/2024)
        /// </summary>
        /// <returns>Mã lớn nhát</returns>
        Task<string> GetMaxCode();
    }
}
