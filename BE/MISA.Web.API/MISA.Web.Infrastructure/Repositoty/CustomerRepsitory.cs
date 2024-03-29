using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Infrastructure.Repositoty
{
    public class CustomerRepsitory : BaseRepository<Customer>, ICustomerRepository
    {
        #region Constructor
        public CustomerRepsitory(IMISADbContext dbContext) : base(dbContext)
        {
        } 
        #endregion
    }
}
