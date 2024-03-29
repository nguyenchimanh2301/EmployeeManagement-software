using Microsoft.AspNetCore.Http;
using MISA.Web.Core.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Exceptions
{
    /// <summary>
    /// lớp HandeExceptionMiddleware 
    /// CreatedBy NCManh(15/12/2023)
    /// </summary>
    public class HandeExceptionMiddleware
    {
        #region Field
        private RequestDelegate _next;

        #endregion
        #region Constructor
        public HandeExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion
        #region Method
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (MISAValidateException ex )
            {
                var serviceResult = new MISAServiceResult();
                serviceResult.StatusCode = System.Net.HttpStatusCode.BadRequest;
                context.Response.StatusCode  = (int)System.Net.HttpStatusCode.BadRequest;
                serviceResult.Errors = ex.MessageDetail;
                foreach (var message in ex.MessageDetail)
                {
                    serviceResult.UserMsg = message.Value[0].ToString();
                    serviceResult.DevMsg = message.Value[0].ToString();
                }
                var res = JsonConvert.SerializeObject(serviceResult);
                await context.Response.WriteAsync(res);
            }
            catch(Exception ex) {
                var serviceResult = new MISAServiceResult();
                serviceResult.UserMsg = ex.Message;
                serviceResult.DevMsg = ex.Message;
                var res = JsonConvert.SerializeObject(serviceResult);
                context.Response.StatusCode = 500;
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(res);
            }
            #endregion

        }
    }
}
