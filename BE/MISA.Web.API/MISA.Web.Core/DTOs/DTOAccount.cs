using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.DTOs
{
    public class DTOAccount
    {
        /// <summary>
        /// Tài khoản
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }
    }
}
