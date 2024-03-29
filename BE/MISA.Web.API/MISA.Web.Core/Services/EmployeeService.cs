using AutoMapper;
using Microsoft.AspNetCore.Http;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Interfaces.UnitOfWork;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore;
using OfficeOpenXml.DataValidation;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.Style;
using System.Text.RegularExpressions;
using MISA.Web.Core.Atribute;
using MISA.Web.Core.Resource;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using MISA.Web.Core.Enum;
using Microsoft.Extensions.Caching.Memory;
using MISA.Web.Core.Const;
using static Dapper.SqlMapper;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;


namespace MISA.Web.Core.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region field
        IEmployeeRepository _employeerepository;
        IDepartmentRepsitory _departmentRepsitory;
        IPositionRepsitory _positionRepsitory;
        IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IMemoryCache _cache;
        #endregion
        #region Constructor
        public EmployeeService(IEmployeeRepository employeerepository, IMapper mapper, IMemoryCache cache) : base(employeerepository)
        {
            _employeerepository = employeerepository;
            this.mapper = mapper;
            _cache = cache;
        }
        public EmployeeService(IUnitOfWork unitOfWork, IEmployeeRepository employeerepository, IMapper mapper, IMemoryCache cache, IPositionRepsitory positionRepsitory, IDepartmentRepsitory departmentRepsitory) : base(employeerepository)
        {
            _employeerepository = employeerepository;
            this.mapper = mapper;
            _cache = cache;
            _departmentRepsitory = departmentRepsitory;
            _positionRepsitory = positionRepsitory;
            _unitOfWork = unitOfWork;
        } 
        #endregion
        #region Method
        /// <summary>
        /// Validate dữ liệu đầu vào 
        /// </summary>
        /// <param name="entity">Đối tượng validate</param>
        /// <exception cref="MISAValidateException">Các lỗi bắt đuợc</exception>
        public override void ValidateData(Employee entity)
        {
            ValidateProperty(entity);
        }
        /// <summary>
        /// Hàm Chèn dữ liệu từ excel
        /// CreatedBy NCManh(15/1/2024)
        /// </summary>
        /// <returns>Tình trạng thêm của danh sách dữ liệu : true -thành công ; false - thất bại </returns>
        public async Task<IEnumerable<Employee>> ImportFile(IFormFile file , bool? commit)
        {
            var employeeRecords = new List<DTOImport>();
            var listEmployeeInsert = new List<Employee>();
            var errors = new Dictionary<string, List<string>>();
            if (_cache.TryGetValue(MISAResourceExcel.EmployeeImportCache, out List<Employee>? cachedEmployees))
            {
                return cachedEmployees;
            }
            CheckFileImport(file);
            await _unitOfWork.BeginTransactionAsync();
            using (var stream = new MemoryStream())
            {
                //copy tệp vào stream
                file?.CopyToAsync(stream);
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        var rowCount = worksheet.Dimension.Rows;
                        // Lấy danh sách các tiêu đề cột từ hàng đầu tiên của tệp Excel
                        var columnHeaders = new List<string>();
                        for (var col = 1; col <= worksheet.Dimension.Columns; col++)
                        {
                            var columnHeader = worksheet.Cells[1, col].Value?.ToString().Trim();
                            columnHeaders.Add(columnHeader);
                        }
                        foreach (var columnHeader in columnHeaders)
                        {
                            if (!ConstExcelColumnName.IsColumnDefined(columnHeader))
                            {
                                // Nếu cột không khớp với bất kỳ hằng số nào trong ConstExcelColumnName, đưa ra cảnh báo
                                errors.Add(MISAResourceVN.Error, new List<string> { MISAResourceVN.FileImportNotValid });
                                throw new MISAValidateException(errors);
                            }
                        }
                        // Duyệt qua từng dòng trong tệp Excel
                        for (var row = 2; row <= rowCount; row++) // Bắt đầu từ hàng thứ hai để bỏ qua tiêu đề
                        {
                            // Tạo một đối tượng DTO mới
                                var employee = new DTOImport();
                            // Ánh xạ dữ liệu từ các cột Excel vào thuộc tính tương ứng trong đối tượng Employee
                            for (var col = 1; col <= worksheet.Dimension.Columns; col++)
                            {
                                var columnHeader = columnHeaders[col - 1]; // -1 vì cột đầu tiên bắt đầu từ 1
                                var cellValue = worksheet.Cells[row, col].Value?.ToString().Trim();
                                employee.EmployeeId = Guid.NewGuid();
                                // Ánh xạ tên cột sang thuộc tính tương ứng trong đối tượng Employee
                                switch (columnHeader)
                                {
                                    case ConstExcelColumnName.EMPLOYEE_CODE:
                                        ValidateCodeImport(cellValue, employee, row);
                                        employee.EmployeeCode = cellValue;
                                        break;
                                    case ConstExcelColumnName.FULLNAME:
                                        if (string.IsNullOrEmpty(cellValue))
                                        {
                                            employee.DTOImportErrors.Add(string.Format(Resource.MISAResourceVN.FullNameNotEmpty, Resource.MISAResourceExcel.EmployeeName, employee.FullName));
                                            employee.Position = string.Format(Resource.MISAResourceExcel.RowIndex, row.ToString());
                                            employee.IsImport = false;
                                        }
                                        employee.FullName = cellValue;
                                        break;
                                    case ConstExcelColumnName.GENDER:
                                        employee.Gender = GetGender(cellValue);
                                        break;
                                    case ConstExcelColumnName.CREDIT_NUMBER:
                                        employee.CreditNumber = cellValue;
                                        break;
                                    case ConstExcelColumnName.DATEOFBIRTH:
                                        employee.DateOfBirth = ParseInputDate(cellValue);
                                        break;
                                    case ConstExcelColumnName.DEPARTMENT_ID:
                                        if (cellValue == null)
                                        {
                                            continue;
                                        }
                                        employee.DepartmentName = cellValue;
                                        var departmentId = await _departmentRepsitory.GetDataByName(cellValue);
                                        if (departmentId != null)
                                        {
                                            employee.DepartmentId = new Guid(departmentId.ToString());
                                        }
                                        else
                                        {
                                            employee.DTOImportErrors.Add(string.Format(Resource.MISAResourceVN.DeparmentNameNotValid, Resource.MISAResourceExcel.DepartmentName, employee.DepartmentName));
                                            employee.Position = string.Format(Resource.MISAResourceExcel.RowIndex, row.ToString());
                                            employee.IsImport = false;
                                        }
                                        break;
                                    case ConstExcelColumnName.POSITTION_ID:
                                        if (cellValue == null)
                                        {
                                            continue;
                                        }
                                        employee.PositionName = cellValue;
                                        var positionId = await _positionRepsitory.GetDataByName(cellValue);
                                        if (positionId != null)
                                        {
                                            employee.PositionId = new Guid(positionId.ToString());
                                        }
                                        else
                                        {
                                            employee.DTOImportErrors.Add(string.Format(Resource.MISAResourceVN.PosittionNameNotValid, Resource.MISAResourceExcel.PositionName, employee.PositionName));
                                            employee.Position = string.Format(Resource.MISAResourceExcel.RowIndex, row.ToString());
                                            employee.IsImport = false;
                                        }
                                        break;
                                    case ConstExcelColumnName.BANK_NAME:
                                        employee.BankName = cellValue;
                                        break;
                                    case ConstExcelColumnName.BANK_ADDRESS:
                                        employee.Address = cellValue;
                                        break;
                                }
                            }
                            if (employee.DTOImportErrors.Count == 0)
                            {
                                employee.IsImport = true;
                                var employeeImport = mapper.Map<Employee>(employee);
                                listEmployeeInsert.Add(employeeImport);
                            }
                            // Thêm bản ghi nhân viên vào danh sách
                            employeeRecords.Add(employee);
                        }
                    }
                }
               
            }
            if (listEmployeeInsert.Count > 0 && commit==true)
            {
                // Thực hiện thêm mới
                await _employeerepository.InsertMultiData(listEmployeeInsert);
                await _unitOfWork.CommitAsync();

            }
            await _unitOfWork.RollbackAsync();
            _cache.Set(MISAResourceExcel.EmployeeImportCache, employeeRecords, TimeSpan.FromMinutes(10));
            return employeeRecords;
        }
        /// <summary>
        /// Hàm nối các cột trong excel với các thuộc tính của đối tuợng
        /// </summary>
        /// <param name="columnHeader">Các tiêu đề cột trong excel</param>
        /// <param name="cellValue">Giá trị tương ứng</param>
        /// <param name="employee">Đối tượng Import</param>
        /// <param name="row">Vị trí dòng</param>
        /// CreatedBy NCmanh(20/3/2024)
        public async Task  MapObjectWithColumnExcel(string? columnHeader ,string  cellValue, DTOImport employee , int? row)
        {
            
        }
        /// <summary>
        /// Hàm kiểm tra file có tồn tại không
        /// CreatedBy NCManh(15/1/2024)
        /// </summary>
        /// <param name="file">tệp đầu vào</param>
        /// <exception cref="MISAValidateException">Cảnh báo file trống hoặc không hợp lệ</exception>
        /// <summary>
        /// Hàm lưu trữ và lấy tệp xuất khẩu trong cache
        /// CreatedBy NCManh(07/03/2024)
        /// </summary>
        /// <returns>Tệp xuất khẩu</returns>
        public async Task<MemoryStream> ExportFile()
        {
            if (_cache.TryGetValue(MISAResourceExcel.CacheExport, out MemoryStream? cachedResult))
            {
                // Nếu có trong cache, trả về kết quả từ cache
                return new MemoryStream(cachedResult.ToArray());
            }

            var memory = await ExportExcelFile();

            // Lưu kết quả vào cache
            _cache.Set(MISAResourceExcel.CacheExport, memory.ToArray());

            return memory;
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file execl
        /// CreatedBy NCManh(15/1/2024)
        /// </summary>
        /// <returns></returns>
        public async Task<MemoryStream> ExportExcelFile()
        {
            var list = await _employeerepository.GetAllDTOAsync();
            var employeeExport = mapper.Map<IEnumerable<DTOImport>>(list);
            var res = await BuildExcel(employeeExport);
            return res;
        }
        /// <summary>
        /// Hàm xuất dữ liệu ra file execlx
        /// </summary>
        /// <param name="list">Danh sách dữ liệu muốn xuất</param>
        /// CreatedBy NCManh(15/1/2024)
        /// <returns>Danh sách dữ liệu tệp</returns>
        public async Task<MemoryStream> ExporDataEmployee(List<DTOEmployee> list)
        {
            var employeeExport = mapper.Map<IEnumerable<DTOImport>>(list);
            var res = await BuildExcel(employeeExport);
            return res;
        }
        #endregion
        /// <summary>
        /// Hàm xuất dữ liệu ra file execlx
        /// </summary>
        /// <param name="list">Danh sách dữ liệu muốn xuất</param>
        /// CreatedBy NCManh(15/1/2024)
        /// <returns>Danh sách dữ liệu tệp</returns>
        public async Task<MemoryStream> ExportData(IEnumerable<DTOImport> list)
        {
            var res = await BuildExcel(list);
            return res;

        }

        /// <summary>
        /// Hàm tạo file excel mẫu
        /// </summary>
        /// CreatedBy NCManh(15/1/2024)
        /// <returns>Tệp mẫu</returns>
        public async Task<MemoryStream> ExportEmptyFile()
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(MISAResourceExcel.Sheet1);
                var arr = new string[] { MISAResourceExcel.Index, MISAResourceExcel.EmployeeCode ,
                    MISAResourceExcel.EmployeeName, MISAResourceExcel.Gender, MISAResourceExcel.DateOfBirth,
                    MISAResourceExcel.DepartmentName,MISAResourceExcel.PositionName, MISAResourceExcel.CreditNumber,
                    MISAResourceExcel.BankName, MISAResourceExcel.BankAdress };
                var header = 1;
                foreach (var x in arr)
                {
                    worksheet.Row(header).Height = 30; // Chiều rộng là 30
                    worksheet.Column(header).Width = 30; // Chiều rộng là 30
                    worksheet.Cells[1, header].Value = x.ToUpper();
                    using (var range = worksheet.Cells[1, header])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                    }
                    worksheet.Cells[1, header].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa ngang
                    worksheet.Cells[1, header].Style.VerticalAlignment = ExcelVerticalAlignment.Center;   // Căn giữa dọc
                    header++;
                }
                var memory = new MemoryStream();
                await excelPackage.SaveAsAsync(memory);
                memory.Position = 0;
                return memory;

            }
        }

        /// <summary>
        /// Xây dựng Mô hình excel để xuất tệp
        /// </summary>
        /// <param name="list">danh sách dữ liệu xuất tệp</param>
        /// CreatedBy NCManh(12/3/2024)
        /// <returns>Tệp excel xuất khẩu</returns>
        /// 
        public async Task<MemoryStream> BuildExcel(IEnumerable<DTOImport> list)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(MISAResourceExcel.Sheet1);
                var arr = new string[] { MISAResourceExcel.Index, MISAResourceExcel.EmployeeCode ,
                    MISAResourceExcel.EmployeeName, MISAResourceExcel.Gender, MISAResourceExcel.DateOfBirth,
                    MISAResourceExcel.DepartmentName,MISAResourceExcel.PositionName, MISAResourceExcel.CreditNumber,
                    MISAResourceExcel.BankName, MISAResourceExcel.BankAdress };
                var header = 1;
                foreach (var x in arr)
                {
                    worksheet.Row(header).Height = 30; // Chiều rộng là 30
                    worksheet.Column(header).Width = 30; // Chiều rộng là 30
                    worksheet.Cells[1, header].Value = x.ToUpper();
                    using (var range = worksheet.Cells[1, header])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                    }
                    worksheet.Cells[1, header].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa ngang
                    worksheet.Cells[1, header].Style.VerticalAlignment = ExcelVerticalAlignment.Center;   // Căn giữa dọc
                    header++;
                }
                var row = 2;
                var index = 1;
                foreach (var x in list)
                {
                    var modelTable = worksheet.Cells;
                    modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa ngang
                    modelTable.Style.VerticalAlignment = ExcelVerticalAlignment.Center;   // Căn giữa dọc
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.AutoFitColumns();
                    worksheet.Row(row).Height = 30;
                    worksheet.Column(row).Width = 30;
                    worksheet.Cells[row, 1].Value = index++;
                    worksheet.Cells[row, 2].Value = x.EmployeeCode;
                    worksheet.Cells[row, 3].Value = x.FullName;
                    worksheet.Cells[row, 4].Value = GetNameGender(x.Gender);
                    worksheet.Cells[row, 5].Value = ConvertDatetoString(x.DateOfBirth);
                    worksheet.Cells[row, 6].Value = x.DepartmentName;
                    worksheet.Cells[row, 7].Value = x.PositionName;
                    worksheet.Cells[row, 8].Value = x.CreditNumber;
                    worksheet.Cells[row, 9].Value = x.BankName;
                    worksheet.Cells[row, 10].Value = x.BankAdress;
                    worksheet.Cells[row, 11].Value = x.DTOImportErrors;
                    row++;
                }
                var memory = new MemoryStream();
                await excelPackage.SaveAsAsync(memory);
                memory.Position = 0;
                return memory;
            }
        }
        /// <summary>
        /// Hàm validate đối tượng nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên</param>
        ///   /// CreatedBy NC Mạnh(14/3/2024)
        /// <exception cref="MISAValidateException">Ngoại lệ thông báo</exception>
        public void ValidateProperty(Employee employee)
        {
            var errors = new Dictionary<string, List<string>>();
            if (string.IsNullOrEmpty(employee.FullName))
            {
                errors.Add(employee.FullName, new List<string> { MISAResourceVN.FullNameNotEmpty });
            }
            if (!IsCheckCode(employee.EmployeeCode))
            {
                errors.Add(employee.EmployeeCode, new List<string> { MISAResourceVN.CodeNotValid });
            }
            if (CheckValueExistByField(employee.EmployeeCode))
            {
                errors.Add(MISAResourceEN.EmployeeCode, new List<string> { string.Format(MISAResourceVN.CodeNotDupllicate, Resource.MISAResourceVN.EmployeeCode, employee.EmployeeCode) });
            }
            if (DateGreaterThanNow(employee.DateOfBirth))
            {
                errors.Add(MISAResourceVN.DateOfBirth, new List<string> { MISAResourceVN.DateOfBirthNotGreaterNow });
            }
            if (DateGreaterThanNow(employee.IdentityDate))
            {
                errors.Add(MISAResourceVN.IdentityDate, new List<string> { MISAResourceVN.IdentityDateNotGreaterNow });
            }
            if (!IsPhoneNumber(employee.PhoneNumber))
            {
                errors.Add(MISAResourceVN.PhoneNumber, new List<string> { MISAResourceVN.PhoneNumberNotValidMesssage });
            }
            if (!IsValidEmail(employee.Email))
            {
                errors.Add(MISAResourceVN.Email, new List<string> { MISAResourceVN.EmailNotValid });
            }
            if (errors.Count > 0)
            {
                throw new MISAValidateException(errors);
            }
            employee.ModifiedDate = DateTime.Now;
        }
        /// <summary>
        /// Validate mã nhân viên để import
        /// </summary>
        /// <param name="code">Mã nhân viên</param>
        /// <param name="employee">Đối tượng import</param>
        /// <param name="posiotion">Vị trí lõi</param>
        /// CreatedBy NC Mạnh(14/3/2024)
        public void ValidateCodeImport(string? code, DTOImport employee, int? posiotion)
        {
            if (string.IsNullOrEmpty(code))
            {
                employee.DTOImportErrors.Add(MISAResourceVN.EmployeeCode_NotEmpty);
                employee.Position = string.Format(Resource.MISAResourceExcel.RowIndex, posiotion.ToString());
                employee.IsImport = false;
            } 
            
            if (!IsCheckCode(code))
            {
                employee.DTOImportErrors.Add(MISAResourceVN.CodeNotValid);
                employee.Position = string.Format(Resource.MISAResourceExcel.RowIndex, posiotion.ToString());
                employee.IsImport = false;
            }
            if (CheckValueExistByField(code))
            {
                employee.DTOImportErrors.Add(string.Format(MISAResourceVN.CodeNotDupllicate, MISAResourceVN.EmployeeCode, employee.EmployeeCode));
                employee.Position = string.Format(Resource.MISAResourceExcel.RowIndex, posiotion.ToString());
                employee.IsImport = false;
            }
        }
        private void CheckFileImport(IFormFile file)
        {
            var errors = new Dictionary<string, List<string>>();
            if (file == null || file.Length < 0)
            {
                errors.Add(MISAResourceVN.Error, new List<string> { Resource.MISAResourceVN.FileNotEmpty });
                throw new MISAValidateException(errors);
            }
            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(MISAResourceVN.Error, new List<string> { Resource.MISAResourceVN.FileIsNotExcelFile });
                throw new MISAValidateException(errors);
            }
        }
        /// <summary>
        /// Hàm xóa cache
        /// CreatedBy NCManh(31/1/2024)
        /// </summary>
        public void ClearCache()
        {
            _cache.Remove(MISAResourceExcel.EmployeeImportCache);
        }

       
    }
}
