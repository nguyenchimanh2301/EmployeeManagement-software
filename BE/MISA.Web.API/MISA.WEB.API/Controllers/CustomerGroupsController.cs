
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web.Core.Const;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Resource;
namespace MISA.Web.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = ConstRole.ADMIN)]
    public class CustomerGroupsController : MISABaseController<CustomerGroup>
    {
        #region Field
        ICustomerGroupRepsitory _customerGroupRepsitory;
        ICustomerGroupService _customerGroupService;
        #endregion
        #region Constructor
        public CustomerGroupsController(ICustomerGroupService customerGroupService, ICustomerGroupRepsitory customerGroupRepsitory):base(customerGroupService, customerGroupRepsitory)
        {
            _customerGroupService = customerGroupService;
            _customerGroupRepsitory = customerGroupRepsitory;
        }
        #endregion
    }
}
