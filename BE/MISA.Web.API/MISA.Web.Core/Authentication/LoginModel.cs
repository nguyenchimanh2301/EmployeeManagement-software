using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Authentication
{
    public class LoginModel
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string? Username { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string? Password { get; set; }



    }
}
