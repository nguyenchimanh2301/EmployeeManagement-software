using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.API.Tests.ServiceTests
{
    public class EmployeeServiceFake : EmployeeService, IEmployeeService
    {
        #region Fields
        IEmployeeRepository _employeerepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache _cache;
        #endregion
        #region Constructor

        public EmployeeServiceFake(IEmployeeRepository employeerepository, IMapper mapper, IMemoryCache cache) : base(employeerepository, mapper, cache)
        {
            _employeerepository = employeerepository;
            this.mapper = mapper;
            _cache = cache;
        }
        #endregion
        #region Method

        public new Task<int> DeleteMultiple(List<Guid> entityId)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> ExportData(List<DTOImport> list)
        {
            return base.ExportData(list);
        }

        public Task<MemoryStream> ExportFile()
        {
            throw new NotImplementedException();
        }

      /*  public Task<IEnumerable<Employee>> ImportFile(IFormFile file)
        {
            return base.ImportFile(file);
        }
*/
        public async Task<int> Insert(Employee entity)
        {
            return await base.Insert(entity);
        }

        public async Task<int> UpdateService(Employee entity, Guid id)
        {
            return await base.UpdateService(entity,id);
        }
        #endregion

    }
}
