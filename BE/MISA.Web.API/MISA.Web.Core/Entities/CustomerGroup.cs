using MISA.Web.Core.Atribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Entities
{
    /// <summary>
    /// Lớp nhóm khách hàng
    /// </summary>
    public class CustomerGroup
    {
        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid CustomerGroupId { get; set; }
        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        /// 
        [NotEmpty]
        public string? CustomerGroupName { get; set; }
        /// <summary>
        /// Ngày tạo        
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public DateTime CreatedBy { get; set;}
    }
}
