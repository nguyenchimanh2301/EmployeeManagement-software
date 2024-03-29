using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web.Core.Const;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;

namespace MISA.Web.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = ConstRole.ADMIN)]
    public class CustomersController : MISABaseController<Customer>
    {
        #region Field
        ICustomerRepository _customerRepository;
        ICustomerService _customerService;
        #endregion
        #region Constructor
        public CustomersController(ICustomerRepository customerRepository, ICustomerService customerService)
            : base(customerService, customerRepository)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;

            #endregion
        }
    }
}
