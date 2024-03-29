using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web.Core.Authentication;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Services;
using MISA.Web.Infrastructure.Repositoty;
using MySqlConnector;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace MISA.Web.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class EmployeesController : MISABaseController<Employee>
    {
        #region Field

        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        #endregion
        #region Constructor
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService) : base(employeeService, employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }
        #endregion
        #region Method
        /// <summary>
        /// API chèn dữ liệu từ file excel vào database
        /// </summary>
        /// <param name="file">tệp dữ liệu</param>
        /// CreatedBy NCManh(28/01/2024)
        /// <returns>danh sách dữ liệu đã chèn</returns>
        [HttpPost("Import")]
        public async Task<IActionResult> ImportFile(IFormFile file, bool? commit)
        {
            var res = await _employeeService.ImportFile(file,commit);
            return StatusCode(200, res);
        }
        /// <summary>
        /// API xuất dữ liệu ra file excel từ database
        /// </summary>
        
        
        /// CreatedBy NCManh(28/01/2024)
        /// <returns>File excel dữ liệu</returns>
        [HttpGet("Export")]
        public async Task<IActionResult> ExportFile()
        {
            var memoryStream = await _employeeService.ExportFile();
            string fileName = $"Employees_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            Response.Headers.Add("Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"attachment; filename={fileName}"));
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }
        /// <summary>
        /// API xuất danh sách dữ liệu
        /// </summary>
        /// CreatedBy NCManh(28/01/2024)
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("ExportEmployee")]
        public async Task<IActionResult> ExporDataEmployee([FromBody] List<DTOEmployee> list)
        {
            var memoryStream = await _employeeService.ExporDataEmployee(list);
            string fileName = $"EmployeeDataEmployee_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            Response.Headers.Add("Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"attachment; filename={fileName}"));
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }
        /// <summary>
        /// API xuất danh sách dữ liệu import
        /// </summary>
        /// CreatedBy NCManh(28/01/2024)
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("ExportData")]
        public async Task<IActionResult> ExporData([FromBody] List<DTOImport> list)
        {
            var memoryStream = await _employeeService.ExportData(list);
            string fileName = $"EmployeeDataError_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            Response.Headers.Add("Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"attachment; filename={fileName}"));
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }
        /// <summa
        /// API xuất tệp mẫu
        /// </summary>
        /// CreatedBy NCManh(28/01/2024)
        /// <returns>Tệp mẫu</returns>
        [HttpGet("EmptyFile")]
        public async Task<IActionResult> ExporEmptyFile()
        {
            var memoryStream = await _employeeService.ExportEmptyFile();
            string fileName = $"EmployeeEmpty_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            Response.Headers.Add("Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"attachment; filename={fileName}"));
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }
        /// <summary>
        /// Hàm phân trang tìm kiếm dữ liệu nhân viên
        /// </summary>
        /// <param name="searchText">Từ khóa Tìm kiếm</param>
        /// <param name="pageSize">Kích cỡ trang</param>
        /// CreatedBy NCManh(28/01/2024)
        /// <param name="numberPage">Số trang</param>
        /// <returns></returns>
        
        [HttpGet("getpagingdto")]
        public async Task<IActionResult> GetPagingDto(string? searchText, int pageSize, int numberPage)
        
        {
            var data = await _employeeRepository.FilterData(searchText, pageSize, numberPage);
            return Ok(data);

        }
        /// <summary>
        /// API lấy về mã nhân viên lớn nhất
        /// </summary>
        /// CreatedBy NCManh(28/01/2024)
        /// <returns>Mã nhân viên lớn nhất</returns>
        [HttpGet("maxcode")]
        public async Task<string> GetmaxCode()
        {
           var res = await _employeeRepository.GetMaxCode();
            return res;
        }

        #endregion
    }

}

