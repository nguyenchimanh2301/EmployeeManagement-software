using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.CustomValidatetion
{
    public class DateNotValid : ValidationAttribute
    {/// <summary>
    /// Hàm validate ngày nhập
    /// CreatedBy NCMANH(15/12/2023)
    /// </summary>
    /// <param name="value">Giá trị nhập</param>
    /// <param name="validationContext">Thông báo validate dữ liệu</param>
    /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                var today = DateTime.Now;
                if (today < date)
                {
                    return new ValidationResult(Resource.MISAResourceVN.DateOfBirthNotGreaterNow);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult(Resource.MISAResourceVN.DateOfBirthNotValid);
            }

        }
    }
}
