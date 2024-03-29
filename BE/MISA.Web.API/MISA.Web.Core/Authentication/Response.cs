using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Authentication
{
    public class Response
    {
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// Thông báo
        /// </summary>
        public string? Message { get; set; }
    }
}
