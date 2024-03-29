using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MISA.Web.API.Tests.RepsitoryTests;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Enum;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Interfaces.UnitOfWork;
using MISA.Web.Core.Services;
using MISA.Web.Infrastructure.Interface;
using MISA.Web.Infrastructure.MISADatabaseContext;
using MISA.Web.Infrastructure.Repositoty;
using MISA.Web.Infrastructure.UnitOfWork;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework.Constraints;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.API.Tests.ServiceTests
{
    [TestFixture]
    public class EmployeeServiceTest
    {
        #region Fields
        MySqlDbcontext db;
        IEmployeeRepository employeerepository;
        IMapper mapper;
        IMemoryCache cache;
        EmployeeService _employeeService;
        #endregion
        #region Methods

        [SetUp]
        public void SetUp()
        {
            employeerepository = Substitute.For<IEmployeeRepository>();
            mapper = Substitute.For<IMapper>();
            cache = Substitute.For<IMemoryCache>();
            _employeeService = new EmployeeService(employeerepository, mapper, cache);

        }
       
        /// <summary>
        /// Unit test hàm kiểm tra trùng lặp mã nhân viên
        /// CreatedBy NCManh(26/2/2024)
        /// </summary>
        [Test]
        public void CheckEmployeeExistByCode_NotExistEmployee_ConflictExection()
        {
            ///Arrange
            var code = "NV-797778";
            //act
            _employeeService.CheckValueExistByField(code).Returns(false);
            /* _employeeService.Received(1).CheckValueExistByField(code);*/
            var boolemployeCode = _employeeService.CheckValueExistByField(code);
            // Assert
            Assert.That(boolemployeCode, Is.EqualTo(false));
        }

        /// <summary>
        /// Unit test kiểm tra xóa nhiều bản ghi
        /// </summary>
        /// CreatedBy NCManh(29/2/2024)
        /// <returns>Trả về số bản ghi xóa thành công</returns>
        [Test]
        public async Task DeleteMultiple_ListData_ReturnSuccess()
        {
            ///Arrange
            var id = new List<Guid>
            {
              new Guid("040656f6-9835-42db-b47d-420046a5bec3"),
              new Guid("040656f6-9835-42db-b47d-420046a5bec3"),
            };
            ///Act
            _employeeService.DeleteMultiple(id).Returns(2);
            var res = await _employeeService.DeleteMultiple(id);
            ///Assert
            Assert.That(res, Is.EqualTo(2));
        }
        /// <summary>
        /// Unit test kiểm tra thêm dữ liệu nhân viên
        /// </summary>
        /// CreatedBy NCManh(26/2/2024)
        /// <returns></returns>k
        [Test]
        public async Task AddEmployeeData_ReturnSuccess()
        {
            ///Arrange
            var employee = new Employee
            {
                EmployeeCode = "NV-8587781131326",
                FullName = "Manh",
                Gender = 0,
                PhoneNumber = null,
                Address = null,
                Email = null,
                CreatedDate = null,
                ModifiedDate = null,
                DateOfBirth = null,
                IdentityNumber = null,
                IdentityPlace = null,
                WorkStatus = null,
                Salary = null,
                BankName = null,
                BankAdress = null,
                CreditNumber = null,
                DepartmentId = Guid.Parse("35e773ea-5d44-2dda-26a8-6d513e380bde"),
                PositionId = Guid.Parse("2000637a-2a69-2e7e-a135-f63bd3f197bd")
            };
            ///Act
            _employeeService.Insert(employee).Returns(1);
            var res = await _employeeService.Insert(employee);
            ///Assert
            Assert.That(res, Is.EqualTo(1));
        }
        /// <summary>
        /// Hàm kiểm tra thêm nhân viên 
        /// Returns : MISAValidateException(Lỗi)
        /// CreatedBy: NCM(29/2/2024)
        /// </summary>
        [Test]
        public async Task Insert_WithExceptionEmailandCode_ShouldReturnMISAValidateException()
        {
            ///Arrange
            var employee = new Employee
            {
                EmployeeCode = "688505",
                FullName = "Manh",
                Gender = 0,
                PhoneNumber = null,
                Address = null,
                Email = "aaaaaa",
                CreatedDate = null,
                ModifiedDate = null,
                DateOfBirth = null,
                IdentityNumber = null,
                IdentityPlace = null,
                WorkStatus = null,
                Salary = null,
                BankName = null,
                BankAdress = null,
                CreditNumber = null,
                DepartmentId = Guid.Parse("35e773ea-5d44-2dda-26a8-6d513e380bde"),
                PositionId = Guid.Parse("2000637a-2a69-2e7e-a135-f63bd3f197bd")
            };
            ///Act
            ///Assert
            Assert.ThrowsAsync<MISAValidateException>(async () => await _employeeService.Insert(employee));
        }
        /// <summary>
        /// Unit test kiểm tra sửa dữ liệu nhân viên
        /// </summary>
        /// CreatedBy NCManh(26/2/2024)
        /// <returns>success-thành công</returns>
        [Test]
        public async Task UpdateEmployeeData_ReturnSuccess()
        {
            ///Arrange
            var id = new Guid("025aac52-3e1b-433f-b6ab-5ad0b9c137ba");
            var employee = new Employee
            {
                EmployeeCode = "NV-688505",
                FullName = "Manh",
                Gender = 0,
                PhoneNumber = null,
                Address = null,
                Email = null,
                CreatedDate = null,
                ModifiedDate = null,
                DateOfBirth = null,
                IdentityNumber = null,
                IdentityPlace = null,
                WorkStatus = null,
                Salary = null,
                BankName = null,
                BankAdress = null,
                CreditNumber = null,
                DepartmentId = Guid.Parse("35e773ea-5d44-2dda-26a8-6d513e380bde"),
                PositionId = Guid.Parse("2000637a-2a69-2e7e-a135-f63bd3f197bd")
            };
            ///Act
            _employeeService.UpdateService(employee, id).Returns(1);
            var res = await _employeeService.UpdateService(employee, id);
            ///Assert
            Assert.That(res, Is.EqualTo(1));


        }
        /// <summary>
        /// Unit test kiểm tra sửa dữ liệu nhân viên
        /// </summary>
        /// CreatedBy NCManh(26/2/2024)
        /// <returns>Exception</returns>
        [Test]
        public async Task UpdateEmployeeData_ReturnException()
        {
            ///Arrange
            var id = new Guid("025aac52-3e1b-433f-b6ab-5ad0b9c137ba");
            var employee = new Employee
            {
                EmployeeCode = "NV-688505",
                FullName = "Manh",
                Gender = 0,
                PhoneNumber = null,
                Address = null,
                Email = "abcd",
                CreatedDate = null,
                ModifiedDate = null,
                DateOfBirth = null,
                IdentityNumber = null,
                IdentityPlace = null,
                WorkStatus = null,
                Salary = null,
                BankName = null,
                BankAdress = null,
                CreditNumber = null,
                DepartmentId = Guid.Parse("35e773ea-5d44-2dda-26a8-6d513e380bde"),
                PositionId = Guid.Parse("2000637a-2a69-2e7e-a135-f63bd3f197bd")
            };
            ///Act
            ///Assert
            Assert.ThrowsAsync<MISAValidateException>(async () => await _employeeService.UpdateService(employee,id));
        }
        /// <summary>
        /// Unit test kiểm tra xóa dữ liệu nhân viên
        /// </summary>
        /// CreatedBy NCManh(26/2/2024)et
        /// <returns> 1 : success-thành công ; failed-thất bại</returns>t
        [Test]
        public async Task DeleteEmployeeData_ReturnSuccess()
        {
            //arange
            var id = new Guid("ae18221c-aeaf-4e16-8c32-0ee0f84fe680");
            ///Act
            _employeeService.Delete(id).Returns(1);
            var res = await _employeeService.Delete(id);
            ///Assert
            Assert.That(res, Is.EqualTo(1));
        }
        /// <summary>
        /// Unit test kiểm tra xóa dữ liệu nhân viên
        /// </summary>
        /// CreatedBy NCManh(26/2/2024)et
        /// <returns>Exception</returns>t
        [Test]
        public async Task DeleteEmployeeData_ReturnException()
        {
            //arange
            var id = new Guid("ae18221c-aeaf-4e16-8c32-0ee0f84fe680");
            ///Act
            ///Assert
            Assert.ThrowsAsync<MISAValidateException>(async () => await _employeeService.Delete(id));
        }
        /// <summary>
        /// Unit test kiểm tra chèn dữ liệu từ file Excel
        /// </summary>
        /// CreatedBy NCManh(29/2/2024)
        /// <returns>Danh sách import thành công</returns>
        /// 
      /*  [Test]
        public async Task ImportFile_ValidFile_ReturnsListOfEmployees()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Arrange
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Employees_20240302175649.xlsx");
            var file = ConvertPathToIFormFile(filePath);

            // Act
            var result = await _employeeService.ImportFile(file);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<DTOImport>>(result);
        }
*/
        private IFormFile ConvertPathToIFormFile(string filePath)
        {
            // Kiểm tra xem tệp tin có tồn tại không
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Tạo một MemoryStream từ nội dung của tệp tin
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                // Tạo đối tượng IFormFile từ MemoryStream và tên tệp tin
                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(filePath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    ContentDisposition = $"form-data; name=\"fileImport\"; filename=\"{Path.GetFileName(filePath)}\"",
                };
                return formFile;
            }
        }
/*
    [Test]
        public async Task ImportDataEmployee_ReturnList()
        {
            // Arrange
            var filePath = @"C:\Users\ASUS\Desktop\file.xlsx";
            // Act
            if (IsExcelFile(filePath))
            {
                var file = ConvertPathToIFormFile(filePath);

                var result = await _employeeService.ImportFile(file);
                // Assert
                Assert.IsNotNull(file);
                Assert.IsNotNull(result);
            }
        }*/
        public bool IsExcelFile(string filePath)
        {
            // Lấy phần mở rộng của tệp
            string extension = Path.GetExtension(filePath);

            // Kiểm tra xem phần mở rộng có phải là của tệp Excel không
            return extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                   extension.Equals(".xls", StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// Chuyển đường dẫn file sang IFormFile
        /// </summary>
        /// <param name="filePath">Chuỗi đường dẫn đến file</param>
        /// <returns>IFormFile sau chuyển đổi</returns>
        /// <exception cref="FileNotFoundException">Thông báo lỗi</exception>
        /// CreatedBy: NCManh(29/2/2024)
        /*public IFormFile ConvertPathToIFormFile(string filePath)
        {
            // Kiểm tra xem tệp tin có tồn tại không
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("K thấy", filePath);
            }

            // Đọc nội dung của tệp tin từ đường dẫn
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Tạo một MemoryStream từ nội dung của tệp tin
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                // Tạo đối tượng IFormFile từ MemoryStream và tên tệp tin
                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(filePath))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    ContentDisposition = $"form-data; name=\"fileImport\"; filename=\"{Path.GetFileName(filePath)}\"",
                };
                return formFile;
            }
        }
*/
        #endregion

    }
}
