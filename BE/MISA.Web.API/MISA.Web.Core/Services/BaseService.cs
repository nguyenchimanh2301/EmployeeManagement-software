using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using MISA.Web.Core.Atribute;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Enum;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Resource;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using static Dapper.SqlMapper;

namespace MISA.Web.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region Field
        IBaseRepository<MISAEntity> _baserepository;
        private string _className;
        #endregion
        #region Constructor
        public BaseService(IBaseRepository<MISAEntity> baserepository)
        {
            _baserepository = baserepository;
            _className = typeof(MISAEntity).Name;
        }
        #endregion
        #region Method

        #endregion
        public async Task<int> Insert(MISAEntity entity)
        {
            ValidateData(entity);
            var res = await _baserepository.InsertData(entity);
            return res;

        }
        public async Task<int> UpdateService(MISAEntity entity, Guid entityId)
        {
            ValidateData(entity);
            var res = await _baserepository.Update(entity, entityId);
            return res;
        }
        public async Task<int> DeleteMultiple(List<Guid> entityId)
        {
            var res = await _baserepository.DeleteMultiple(entityId);
            return res;
        }

        public async Task<int> Delete(Guid entityId)
        {
            var res = await _baserepository.Delete(entityId);
            if (res == 1)
            {
                return res;
            }
            throw new MISAValidateException();

        }
        /// <summary>
        /// Hàm Validate dữ liệu đầu vào
        /// </summary>
        /// <param name="entity">Dữ liệu của đối tượng đầu vào </param>
        /// <exception cref="MISAValidateException">Trả về một cảnh báo gồm mã lỗi,cảnh báo cho lập trình viên , cảnh báo cho người dùng </exception>
        public virtual  void ValidateData(MISAEntity entity)
        {

        }
        
        /// <summary>
        /// Hàm lấy về gender giới tính
        /// </summary>
        /// <param name="gender">Mã giới tính</param>
        /// <returns>Gender giới tính 0-Nam ,1-Nữ ,2-Khác</returns>
        public Gender GetGender(string gender)
        {
            Gender gender1 = 0;
            switch (gender)
            {
                case Const.Const.GENDER_MALE:
                    return Enum.Gender.Male;
                case Const.Const.GENDER_FEMALE:
                    return Enum.Gender.Female;
                case Const.Const.GENDER_OTHER:
                    return Enum.Gender.Other;
                    ;
                default:
                    return gender1;
            }
        }

        /// <summary>
        /// Hàm convert Datetime thành string để hiển thị ra excel
        /// CreatedBy NCManh(20/1/2024)
        /// </summary>
        /// <param name="inputDateTime">Ngày tháng năm dạng Date time</param>
        /// <returns>Ngày tháng năm kiểu chuỗi</returns>
        public static string ConvertDatetoString(DateTime? inputDateTime)
        {
            return inputDateTime?.ToString(MISAResourceExcel.Date);
        }
        /// <summary>
        /// Hàm convert string thành Datetime 
        /// CreatedBy NCManh(20/1/2024)
        /// </summary>
        /// <param name="date>Ngày tháng năm dạng chuỗi</param>
        /// <returns>Ngày tháng năm dạng datetime</returns>
        public DateTime ConvertToDateTime(string? date)
        {
            
            // Sử dụng phương thức ParseExact để chuyển đổi chuỗi thành đối tượng DateTime
            DateTime dateTime;
            DateTime.TryParseExact(date, MISAResourceExcel.Date, null, System.Globalization.DateTimeStyles.None, out dateTime);
            return dateTime;

        }
        /// <summary>
        /// Hàm Kiểm tra xem ngày nhập lớn hơn ngày hiện tại không 
        /// CreatedBy NCManh(20/1/2024)
        /// </summary>
        /// <returns>Có - true ; không - false</returns>
        public static bool DateGreaterThanNow(DateTime? date)
        {
            DateTime dateNow = DateTime.Now;
            return date > dateNow;
        }

        public virtual bool CheckValueExistByField(string? value)
        {
            var res = _baserepository.CheckEmployeeExistByCode(value);
            return res;
        }
        /// <summary>
        /// Hàm Validate email
        /// </summary>
        /// <param name="email">dữ liêu emaill đầu vào</param>
        // <returns> kiểm tra Email có hợp kệ không true-có , false - không</returns>
        public virtual bool IsValidEmail(string? email)
        {
            // Định nghĩa parttern của email

            var pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            //Kiểm tra xem định dạng hợp lệ không
            if (!string.IsNullOrEmpty(email))
            {
                return Regex.IsMatch(email, pattern);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Hàm kiểm tra địng dạng mã 
        /// </summary>
        /// <param name="code">Mã  đầu vào</param>
        /// <returns>kiểm tra Mã có hợp kệ không true-có , false - không</returns>
        public virtual bool IsCheckCode(string code)
        {
            // Định nghĩa parttern của code
            //Kiểm tra xem định dạng hợp lệ
            if (!string.IsNullOrEmpty(code))
            {
                Regex regex = new Regex(@"^NV-");
                return regex.IsMatch(code);
            }
            return false;
        }
        /// <summary>
        /// Hàm lấy về tên giới tính
        /// </summary>
        /// <param name="gender">gender giới tính</param>
        /// <returns>Tên giới tính 0-Nam ,1-Nữ ,2-Khác</returns>
        public string GetNameGender(Gender gender)
        {
            switch (gender)
            {
                case Enum.Gender.Female:
                    return Core.Resource.MISAResourceVN.Gender_Female;
                case Enum.Gender.Male:
                    return Core.Resource.MISAResourceVN.Gender_Male;
                case Enum.Gender.Other:
                    return Core.Resource.MISAResourceVN.Gender_Other;
                default:
                    return Core.Resource.MISAResourceVN.Gender_Female;
            }
        }
        /// <summary>
        /// Hàm kiểm tra địng số điện thoại
        /// </summary>
        /// <param name="input">Số điện thoại nhập</param>
        /// CreatedBy NCManh(20/1/2024)
        /// <returns>kiểm tra số điện thoại có hợp kệ không true-có , false - không</returns>
        public static bool IsPhoneNumber(string? input)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại bắt đầu bằng 0 và có tổng cộng 10 chữ số
            string pattern = @"^0\d{9}$";
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }
            // Kiểm tra chuỗi với biểu thức chính quy
            return Regex.IsMatch(input, pattern);
        }
        /// <summary>
        /// Hàm convert string sang datetime
        /// CreatedBy NCManh(20/1/2024)
        /// </summary>
        /// <param name="input">Ngày tháng năm dạng chuỗi</param>
        /// <returns>Ngày tháng năm kiểu Datime</returns>
        public DateTime? ParseInputDate(string? input)
        {
            if (input == null)
            {
                // Trả về null khi đầu vào là null
                return null;
            }

            DateTime parsedDate = DateTime.MinValue;
            string pattern1 = @"^(\d{2})/(\d{2})/(\d{4})$"; // dd/MM/yyyy
            string pattern2 = @"^(\d{2})-(\d{2})-(\d{4})$"; // dd-MM-yyyy
            string pattern3 = @"^(\d{2})/(\d{4})$"; // MM/yyyy
            string pattern4 = @"^(\d{2})-(\d{4})$"; // MM-yyyy
            string pattern5 = @"^(\d{4})$"; // yyyy

            Match match1 = Regex.Match(input, pattern1);
            Match match2 = Regex.Match(input, pattern2);
            Match match3 = Regex.Match(input, pattern3);
            Match match4 = Regex.Match(input, pattern4);
            Match match5 = Regex.Match(input, pattern5);

            if (match1.Success)
            {
                int day = int.Parse(match1.Groups[1].Value);
                int month = int.Parse(match1.Groups[2].Value);
                int year = int.Parse(match1.Groups[3].Value);
                parsedDate = new DateTime(year, month, day);
            }
            else if (match2.Success)
            {
                int day = int.Parse(match2.Groups[1].Value);
                int month = int.Parse(match2.Groups[2].Value);
                int year = int.Parse(match2.Groups[3].Value);
                parsedDate = new DateTime(year, month, day);
            }
            else if (match3.Success)
            {
                int month = int.Parse(match3.Groups[1].Value);
                int year = int.Parse(match3.Groups[2].Value);
                parsedDate = new DateTime(year, month, 1); // Set ngày là 1
            }
            else if (match4.Success)
            {
                int month = int.Parse(match4.Groups[1].Value);
                int year = int.Parse(match4.Groups[2].Value);
                parsedDate = new DateTime(year, month, 1); // Set ngày là 1
            }
            else if (match5.Success)
            {
                int year = int.Parse(match5.Groups[1].Value);
                parsedDate = new DateTime(year, 1, 1); // Set tháng và ngày là 1
            }

            return parsedDate;
        }


    }
}
