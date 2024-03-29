using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Refreshtoken
        /// </summary>
        public string? RefreshToken { get; set; }
        /// <summary>
        /// Thời gian refresh token tồn tại
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
