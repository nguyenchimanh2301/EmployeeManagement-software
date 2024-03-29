using MISA.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.DTOs
{
    /// <summary>
    /// DTO map với employee để lấy ra dữ liệu 
    /// </summary>
    public class DTOEmployee:Employee
    {
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? PositionName { get; set; }
    }
}
