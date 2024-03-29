using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.DTOs
{
    public class DTOToken
    {
        /// <summary>
        /// Token xác nhận truy cập người dùng
        /// </summary>
        public string? AccessToken { get; set; }
        // Token để làm mới truy cập của người dùng
        public string? RefreshToken { get; set; }
    }
}
