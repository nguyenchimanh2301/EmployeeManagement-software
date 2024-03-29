using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MISA.Web.Core.Authentication;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Resource;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MISA.Web.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationsController : MISABaseController<Account>
    {
        #region Fields  
        IAcountRepsitory _acountRepsitory;
        IAccountService _accountServices;

        /* IAuthenticationRepsitory _authenticateRepsitory;
         Core.Interfaces.Services.IAuthenticationService _accountServices;*/
        #endregion
        #region Constructor
        public AuthenticationsController(IAcountRepsitory acountRepsitory, IAccountService accountServices):base(accountServices, acountRepsitory)
        {
            _acountRepsitory = acountRepsitory;
            _accountServices = accountServices;
        }
        /* public AuthenticationsController(IAuthenticationRepsitory authenticateRepsitory, Core.Interfaces.Services.IAuthenticationService authenticationService)
         {
             _authenticateRepsitory = authenticateRepsitory;
             _accountServices = authenticationService;
         }*/
        #endregion
        #region Methods
        /// <summary>
        /// API đăng nhập
        /// </summary>
        /// <param name="model">Đối tượng đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>Thành công -  trả về token ; Thât bại  Thông báo đăng nhập  thất bại</returns
        [HttpPost]
        [Route("login")]
        public async Task<object> Login( DTOAccount model)
        {
            var res = await _accountServices.Login(model);
            return res;
        }
        /// <summary>
        ///  API làm mới token
        /// </summary>
        /// <param name="tokenModel">Đối tượng token cũ</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns> refresh token mới và access token mới </returns>
        [HttpPost]
        [Route("refresh-token")]
        public async Task<object> RefreshToken(DTOToken tokenModel)
        {
            var res = await _accountServices.RefreshToken(tokenModel);
            return res;
        }
        /// <summary>
        ///  API đăng ký thông tinư2
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>Thông báo đăng ký thành công hoặc thất bại</returns>
        [HttpPost]
        [Route("register")]
        public async Task<object> Register([FromBody] Account model)
        {
            var res = await _accountServices.Register(model);
            return StatusCode(201,res);
        }
        /// <summary>
        ///  API đăng thu hồi token user
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>Thông báo thành công hoặc thất bại</returns>
        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<object> Revoke(string username)
        {
            var res = await _accountServices.Revoke(username);
            return res;
        }
        #endregion
    }
}
