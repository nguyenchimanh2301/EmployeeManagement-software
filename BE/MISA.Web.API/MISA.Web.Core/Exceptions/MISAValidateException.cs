using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Exceptions
{
    public class MISAValidateException:Exception
    {
        #region Property
        /// <summary>
        /// Câu cảnh báo lỗi
        /// </summary>
        public string? MsgErrorValidate { get; set; }
        public Dictionary<string, List<string>> MessageDetail
        {
            get
            {
                return MsgErrorValidateDaetail;
            }
        }
        #endregion
        #region Field
        Dictionary<string, List<string>> MsgErrorValidateDaetail = new Dictionary<string, List<string>>();
        #endregion
        #region Constructor
        public MISAValidateException(Dictionary<string, List<string>> msgDetail)
        {
            this.MsgErrorValidateDaetail = msgDetail;
        }
        public MISAValidateException(string MsgErrorValidate)
        {
            this.MsgErrorValidate = MsgErrorValidate;
        }
        public MISAValidateException() { }
        #endregion

    }
}
