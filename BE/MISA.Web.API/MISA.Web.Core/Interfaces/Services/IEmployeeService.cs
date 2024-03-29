using Microsoft.AspNetCore.Http;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MISA.Web.Core.Interfaces.Services
{
    /// <summary>
    /// Interface Service bảng nhân viên
    /// Created By NCMANH(25/12/2023)
    /// </summary>
    public interface IEmployeeService:IBaseService<Employee>
    {
        /// <summary>
        /// Thực hiện nhập khẩu dữ liệu từ file
        /// </summary>
        /// <param name="file">File excel dữ liệu nhập khẩu</param>
        /// <returns>Danh sách dữ liệu nhập khẩu thành công</returns>
        /// createdBy NC Manh (16/1/2024)
       Task<IEnumerable<Employee>> ImportFile(IFormFile file,bool? commit);
        /// <summary>
        /// Thực hiện xuất khẩu dữ liệu từ file
        /// </summary>
        /// <returns>Danh sách dữ liệu xuất khẩu thành công</returns>
        /// createdBy NC Manh (24/1/2024)
        Task<MemoryStream> ExporDataEmployee(List<DTOEmployee> list);
        /// <summary>
        /// Thực hiện xuất khẩu dữ liệu từ file
        /// </summary>
        /// <returns>Danh sách dữ liệu xuất khẩu thành công</returns>
        /// createdBy NC Manh (24/1/2024)
        Task<MemoryStream> ExportFile();
        /// <summary>
        /// Thực hiện xuất khẩu dữ liệu từ danh sách
        /// </summary>
        /// <returns>Danh sách dữ liệu xuất khẩu thành công</returns>
        /// createdBy NC Manh (24/1/2024)
        Task<MemoryStream> ExportData(IEnumerable<DTOImport> list);
        /// <summary>
        /// Thực hiện xuất tệp mẫy
        /// </summary>
        /// <returns>Tệp mẫu</returns>
        /// createdBy NC Manh (24/1/2024)
        Task<MemoryStream> ExportEmptyFile();
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
