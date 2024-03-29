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
    public class DepartmentsController : MISABaseController<Department>
    {
        #region Fields
        IDepartmentRepsitory _departmentRepsitoty;
        IDepartmentService _departmentService;
        #endregion
        #region Constructor
        public DepartmentsController(IDepartmentRepsitory departmentRepsitoty, IDepartmentService departmentService):base(departmentService,departmentRepsitoty)
        {
            _departmentRepsitoty = departmentRepsitoty;
            _departmentService = departmentService;
        }
        #endregion
    }
}
