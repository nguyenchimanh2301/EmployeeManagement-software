using MISA.Web.Core.Authentication;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Interfaces.Infrastructure
{
    public interface IAcountRepsitory: IBaseRepository<Account>
    {
        /// <summary>
        /// Interface đăng nhập
        /// </summary>
        /// <param name="model">Đối tượng đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> Login(LoginModel model);
        /// <summary>
        /// Interface lấy thông tin tài khoản theo username
        /// </summary>
        /// <param name="account">Đối tượng đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<Account> GetByUsername(string  username);
        /// <summary>
        /// Interface cập nhật thông tin tài khoản 
        /// </summary>
        /// <param name="account">Đối tượng cập nhật</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> UpdateAsync(Account account);
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
        /// Hàm kiểm tra tồn tại tại tài khoản 
        /// </summary>
        /// <param name="account">Giá trị kiểm tra</param>
        /// <returns>Đã tồn tại-true; chưa tồn tại-false</returns>
        /// CreatedBy NC Manh (03/01/2024)
        bool CheckAccountExist(DTOAccount account);
        /// <summary>
        ///  Interface đăng ký thông tin quản trị viên
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        Task<object> RegisterAdmin(RegisterModel model);
        /// <summary>
        ///  Interface Thu hồi token
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        /// 
        Task<object> Revoke(string username);
        /*        Task<object> RevokeAll();*/
    }
}
