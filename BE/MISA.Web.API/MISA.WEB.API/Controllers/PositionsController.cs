using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MISA.Web.Core.Const;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;

namespace MISA.Web.API.Controllers
{
    /// <summary>
    /// API Vị trí
    /// CreatedBy NCManh(26/1/2024)
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = ConstRole.ADMIN)]

    public class PositionsController :MISABaseController<Positions>
    {
        #region Field
        IPositionRepsitory _positionRepsitoty;
        IPositionService _positionService;
        #endregion
        #region Constructor
        public PositionsController(IPositionRepsitory positionRepsitory, IPositionService positionService) : base(positionService, positionRepsitory)
        {
            _positionRepsitoty = positionRepsitory;
            _positionService = positionService;
        }
        #endregion 
    }
}
