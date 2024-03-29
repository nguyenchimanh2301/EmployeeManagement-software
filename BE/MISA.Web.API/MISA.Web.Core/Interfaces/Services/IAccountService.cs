using MISA.Web.Core.Authentication;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Interfaces.Services
{
    /// <summary>
    /// Interface Tài khoản người dùng
    /// </summary>
    public interface IAccountService : IBaseService<Account>
    {
        /// <summary>
        /// Interface đăng nhập
        /// </summary>
        /// <param name="model">Đối tượng đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> Login(DTOAccount model);
        /// <summary>
        ///  Interface làm mới token
        /// </summary>
        /// <param name="tokenModel">Đối tượng token cũ</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> RefreshToken(DTOToken tokenModel);
        /// <summary>
        ///  Interface đăng ký thông tin
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> Register(Account model);
        /// <summary>
        ///  Interface đăng ký thông tin quản trị viên
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> RegisterAdmin(Account model);
        /// <summary>
        ///  Interface Thu hồi token của user
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> Revoke(string username);
        /*   Task<object> RevokeAll();*/
    }

}
