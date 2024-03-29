using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MISA.Web.Infrastructure.Repositoty;
using MISA.Web.Infrastructure.Interface;
using MISA.Web.Infrastructure.MISADatabaseContext;
using MISA.Web.Core.Interfaces.UnitOfWork;
using MISA.Web.Core.Entities;
using MISA.Web.API.Tests.RepsitoryTests;
using MISA.Web.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Raven.Client.Exceptions;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.DTOs;

namespace MISA.Web.API.Tests.ServiceTests
{
    [TestFixture]
    public class EmployeeRepsitoryTests
    {
        #region Fields
        IUnitOfWork _unitOfWork;
        UnitOfWork unitOfWork;
        IMISADbContext _misadbContext;
        EmployeeRepository EmployeeRepository;
        IConfiguration _configuration;
        #endregion
        #region Method

        [SetUp]
        public void Setup()
        {
            _configuration = Substitute.For<IConfiguration>();
            _configuration.GetConnectionString("DefaultConnection").Returns("Server=8.222.228.150;Port=3306;Database=W08.NCMANH.MF1374;User Id=nvmanh;Password=12345678");
            _misadbContext = Substitute.For<IMISADbContext>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            unitOfWork = new UnitOfWork(_configuration.GetConnectionString("DefaultConnection"));
            EmployeeRepository = new EmployeeRepository(_misadbContext, unitOfWork);
        }

        /// <summary>
        /// Hàm kiểm tra lấy tất cả nhân viên
        /// Returns: danh sách nhân viên
        /// </summary>
        /// CreatedBy: NCManh(26/2/2024)
        [Test]
        public async Task GetAll_EmployeeDTO_ReturnData()
        {
            // Act
            var result = await EmployeeRepository.GetAllDTOAsync();

            // Assert
            Assert.That(result, Is.Not.Empty);
        }

        #endregion
    }

 
}
