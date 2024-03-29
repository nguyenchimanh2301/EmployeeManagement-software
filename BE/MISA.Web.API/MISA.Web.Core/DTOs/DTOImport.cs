using MISA.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.DTOs
{

    /// <summary>
    /// Dto Employee
    /// </summary>
    public class DTOImport:Employee
    {
        #region Constructor
        public DTOImport()
        {
            DTOImportErrors = new List<string>();
        }
        #endregion
        #region Properties
        /// Danh sách lỗi
        /// </summary>
        public List<string> DTOImportErrors {  get; set; }
        /// Thành công
        /// </summary>
/*        public string? DTOImportSuccess { get; set; }*/
        /// <summary>
        /// Trạng thái import : true-thành công , false - thất bại
        /// </summary>
        public bool IsImport { get; set; }
        /// <summary>
        /// Vị trí dòng lỗi
        /// </summary>
        public string? Position { get; set; }
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? PositionName { get; set; }

        #endregion

    }
}
