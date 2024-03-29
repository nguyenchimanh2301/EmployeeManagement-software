using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySharp;
namespace MISA.Web.Core.Const
{
    /// <summary>
    /// Các thuộc tính của bảng employee
    /// </summary>
    public class Const
    {
        public const string MAXLENGCODE_REQUIRED = "Mã Khách Hàng phải nhỏ hơn 20 kí tự";
        public const string DEPARTMENT_NAME = "Tên phòng ban";
        public const string CUSTOMER_CODE = "Mã Khách hàng";
        public const string CUSTOMER_NAME = "Tên Khách hàng";
        public const string DATENOT_VALID = "Ngày tháng không hợp lệ";
        public const string GENDER_MALE = "Nam";
        public const string GENDER_FEMALE = "Nữ";
        public const string GENDER_OTHER = "Khác";

    }
    /// <summary>
    /// Các vai trò của người dùng
    /// </summary>
    public class ConstRole
    {
        public const string USER = "User";
        public const string ADMIN = "Admin";

    }
    /// <summary>
    /// Các cột của file excel
    /// </summary>
    public class ConstExcelColumnName
    {
        public const string INDEX = "STT";
        public const string EMPLOYEE_CODE = "MÃ NHÂN VIÊN";
        public const string FULLNAME = "HỌ TÊN";
        public const string GENDER = "GIỚI TÍNH";
        public const string DATEOFBIRTH = "NGÀY SINH";
        public const string DEPARTMENT_ID= "TÊN PHÒNG BAN";
        public const string POSITTION_ID = "CHỨC VỤ";
        public const string CREDIT_NUMBER = "SỐ TÀI KHOẢN";
        public const string BANK_NAME = "TÊN NGÂN HÀNG";
        public const string BANK_ADDRESS = "CHI NHÁNH TK NGÂN HÀNG";
        /// <summary>
        /// Kiểm tra xem cột excel có tương đồng với các cột của đối tượng không
        /// </summary>
        /// <param name="columnName">cột tiêu đề excel</param>
        /// CreatedBy NCManh(18/3/2024)
        /// <returns></returns>
        public static bool IsColumnDefined(string columnName)
        {
            if (columnName == null)
                return true;
            /// Trả về true nếu độ trùng khớp > 60%
            return typeof(ConstExcelColumnName).GetFields()
                .Any(f => Fuzz.Ratio(f.GetValue(null).ToString().ToLower(), columnName.ToLower()) > 60);
            
        }
    }

}
