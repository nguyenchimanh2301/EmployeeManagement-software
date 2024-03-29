using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Enum
{
    /// <summary>
    /// Enum giới tính
    /// </summary>
    /// Created By NCMANH
    public enum Gender
    {
        /// <summary>
        /// Nữ
        /// </summary>
        Female = 1, //Nữ
        /// <summary>
        /// Nam
        /// </summary>
        Male = 0,//Name
        /// <summary>
        /// Khác
        /// </summary>
        Other = 2
    }
    /// <summary>
    /// Enum tình trạng công việc
    /// </summary>
    public enum WorkStatus
    { 
        /// <summary>
        /// Đang làm việc 1
        /// </summary>
     Working =1,
     /// <summary>
     /// Đã ngừng 0
     /// </summary>
     Stop = 0
    }

}
