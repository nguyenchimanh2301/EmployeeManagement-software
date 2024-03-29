using MISA.Web.Core.Authentication;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        IAuthenticationRepsitory authenticationRepsitory;

        #endregion
        #region Constructor
        public AuthenticationService(IAuthenticationRepsitory authenticationRepsitory)
        {
            this.authenticationRepsitory = authenticationRepsitory;
        }
        #endregion
        #region Methods
        public async Task<object> Login(LoginModel model)
        {
            var res = await authenticationRepsitory.Login(model);
            return res;
        }

        public async Task<object> RefreshToken(TokenModel tokenModel)
        {
            var res = await authenticationRepsitory.RefreshToken(tokenModel);
            return res;
        }

        public async Task<object> Register(RegisterModel model)
        {
            var res = await authenticationRepsitory.Register(model);
            return res;
        }

        public async Task<object> RegisterAdmin(RegisterModel model)
        {
            var res = await authenticationRepsitory.RegisterAdmin(model);
            return res;
        }

        public async Task<object> Revoke(string username)
        {
            var res = await authenticationRepsitory.Revoke(username);
            return res;
        }
        #endregion


    }
}
