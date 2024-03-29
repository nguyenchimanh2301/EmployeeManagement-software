using MISA.Web.Core.Atribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Entities
{
    /// <summary>
    /// Lớp vị trí
    /// CreatedBy NCMANH(23/1/2024)
    /// </summary>
    public class Positions
    {

        /// <summary>
        /// Id vị trí
        /// </summary>
        public Guid PositionId { get; set; }
        /// <summary>
        /// Mã vị trí
        /// </summary>
        public Guid ParenId { get; set; }
        /// <summary>
        /// Mã cha
        /// </summary>
        public string PositionCode { get; set; }
        /// <summary>
        /// Tên vị trí
        /// </summary>
        /// 
        [NotEmpty]
        public string PositionName { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Decription { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}