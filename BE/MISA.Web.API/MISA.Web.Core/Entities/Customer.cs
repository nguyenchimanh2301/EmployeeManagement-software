using MISA.Web.Core.Atribute;
using MISA.Web.Core.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MISA.Web.Core.Const;
using MISA.Web.Core.CustomValidatetion;
namespace MISA.Web.Core.Entities
{
    /// <summary>
    /// Lớp khách hàng
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Id khách hàng
        /// </summary>
        [PrimaryKey]
        public Guid CustomerId { get; set; }
        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid? CustomerGroupId { get; set; }
        /// <summary>
        /// Id công ty
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        [NotDupplicate]
        [PropertyName(Const.Const.CUSTOMER_CODE)]
        [NotEmpty]
        [MaxLength(20,ErrorMessage = Const.Const.MAXLENGCODE_REQUIRED)]
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Họ và tên khách hàng
        /// </summary>
        [NotEmpty]
        [PropertyName(Const.Const.CUSTOMER_NAME)]
        public string? FullName { get; set; }
        /// <summary>
        /// Số điện thoại khách hàng
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Gender Gender { get; set; }
        /*   public string? GenderName
           {
               get
               {
                   switch (Gender)
                   {
                       case Enum.Gender.Female:
                           return Resource.MISAResourceVN.Gender_Female;
                       case Enum.Gender.Male:
                           return Core.Resource.MISAResourceVN.Gender_Male;
                       case Enum.Gender.Other:
                           return Resource.MISAResourceVN.Gender_Other;
                       default:
                           return null;
                   }
               }
           }*/
        /// <summary>
        /// Ngày sinh
        /// </summary>
        [DateNotValid(ErrorMessage = Const.Const.DATENOT_VALID)]
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Dư nợ
        /// </summary>
        public int? DebitAmount { get; set; }
        /// <summary>
        /// Địa chỉ Email
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

    }
}
