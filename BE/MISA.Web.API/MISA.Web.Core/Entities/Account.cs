using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Entities
{
    /// <summary>
    /// Đối tượng Tài khoản đăng nhập
    /// </summary>
    public class Account
    {
        // Tên đăng nhập của người dùng
        public string Username { get; set; }

        // Mật khẩu của người dùng
        public string Password { get; set; }

        // Họ và tên đầy đủ của người dùng
        public string? FullName { get; set; }

        // Vai trò của người dùng
        public string? Role { get; set; }
        // Email của người dùng
        public string? Email { get; set; }
        // Token để xác thực truy cập của người dùng
        public string? AccessToken { get; set; }

        // Token để làm mới truy cập của người dùng
        public string? RefreshToken { get; set; }

        // Ngày và giờ hết hạn của người dùng
        public DateTime? Expiration { get; set; }
    }
}
