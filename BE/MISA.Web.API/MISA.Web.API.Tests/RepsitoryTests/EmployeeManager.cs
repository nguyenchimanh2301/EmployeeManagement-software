using MISA.Web.Core.Entities;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Interfaces.UnitOfWork;
using MISA.Web.Infrastructure.Interface;
using MISA.Web.Infrastructure.MISADatabaseContext;
using MISA.Web.Infrastructure.Repositoty;
using MISA.Web.Infrastructure.UnitOfWork;
using Raven.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.API.Tests.RepsitoryTests
{
    public class EmployeeManager : BaseRepository<Employee>,IEmployeeRepository
    {
        #region Field
        IUnitOfWork _unitOfWork;
        #endregion
        #region Constructor
        public EmployeeManager(IMISADbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        } 
        #endregion

    }


}
