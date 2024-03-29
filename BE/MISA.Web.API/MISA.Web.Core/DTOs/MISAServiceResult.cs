using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.DTOs
{
    public class MISAServiceResult
    {
        /// <summary>
        /// Trạng thái 
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Trả về mã lỗi
        /// 400 - Lỗi nghiệp vụ
        /// 500 - Có Exception
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Câu cảnh báo cho Dev
        /// </summary>
        public string? DevMsg { get; set; }
        /// <summary>
        /// Câu cảnh báo cho người dùng
        /// </summary>
        public string? UserMsg { get; set; }
        /// <summary>
        /// TraceId
        /// </summary>
        public string? TraceId { get; set; }
        /// <summary>
        /// Dữ liệu tả về
        /// </summary>
        public Array?  data { get; set; }
        /// <summary>
        /// Danh sách câu cảnh báo
        /// </summary>
        public Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

    }


}
