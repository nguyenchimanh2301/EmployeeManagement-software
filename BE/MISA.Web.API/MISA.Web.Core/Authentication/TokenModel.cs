using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Authentication
{
    public class TokenModel
    {
        /// <summary>
        /// Token truy cập
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// Token làm mới
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
