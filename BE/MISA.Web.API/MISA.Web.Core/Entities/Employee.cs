using MISA.Web.Core.Atribute;
using MISA.Web.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Entities
{
    /// <summary>
    /// Lớp nhân viên
    /// </summary>
    public class Employee
    {

        /// <summary>
        /// Id nhân viên
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [NotEmpty]
        [PropertyName("Mã nhân viên")]
        [NotDupplicate]
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Họ và tên nhân  viên
        /// </summary>
        [NotEmpty]
        public string FullName { get; set; }
        /// <summary>
        /// Mã giới tính
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? PhoneNumber { get; set; }    
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Địa chỉ Email
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Số căn cước công dân
        /// </summary>
       public string? IdentityNumber { get; set; }
        /// <summary>
        /// Nơi cấp số căn cước công dân
        /// </summary>
        public string? IdentityPlace { get; set; }
        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdentityDate { get; set; }
        /// <summary>
        /// Id vị trí làm việc
        /// </summary>
/*        public Guid? PositionId { get; set; }*/
        /// <summary>
        /// Tình trạng làm việc
        /// </summary>
        public int? WorkStatus { get; set; }
        /// <summary>
        /// Lương của nhân viên
        /// </summary>
        public double? Salary { get; set; }
        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }
        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankAdress { get; set; }
        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string? CreditNumber { get; set; }
        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// Mã chức vụ
        /// </summary>
        public Guid? PositionId { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa gần gần nhất
        /// </summary>


        /// <summary>
        /// DTO Tên giới tính
        /// </summary>
        /*  public string? GenderName
          {
              get
              {
                  switch (Gender)
                  {
                      case Enum.Gender.Female:
                          return Core.Resource.MISAResourceVN.Gender_Female;
                      case Enum.Gender.Male:
                          return Core.Resource.MISAResourceVN.Gender_Male;
                      case Enum.Gender.Other:
                          return Core.Resource.MISAResourceVN.Gender_Other;
                      default:
                          return null;
                  }
              }
          }*/

    }

}

