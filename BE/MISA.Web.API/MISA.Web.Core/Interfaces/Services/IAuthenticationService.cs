using MISA.Web.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Interfaces.Services
{
    /// <summary>
    /// Interface Authenication
    /// </summary>
    public interface  IAuthenticationService
    {
        /// <summary>
        /// Interface đăng nhập
        /// </summary>
        /// <param name="model">Đối tượng đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> Login(LoginModel model);
        /// <summary>
        ///  Interface làm mới token
        /// </summary>
        /// <param name="tokenModel">Đối tượng token cũ</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> RefreshToken(TokenModel tokenModel);
        /// <summary>
        ///  Interface đăng ký thông tin
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> Register(RegisterModel model);
        /// <summary>
        ///  Interface đăng ký thông tin quản trị viên
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> RegisterAdmin(RegisterModel model);
        /// <summary>
        ///  Interface Thu hồi token của user
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> Revoke(string username);
     /*   Task<object> RevokeAll();*/
    }
}
