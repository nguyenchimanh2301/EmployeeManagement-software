using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.Web.Core.Authentication;
using MISA.Web.Core.DTOs;
using MISA.Web.Core.Entities;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Interfaces.Services;
using MISA.Web.Core.Resource;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.Web.Core.Services
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        #region Fields
        IAcountRepsitory _accountRepsitory;
        Dictionary<string, List<string>> errors;
        private readonly IConfiguration _configuration;
        IMapper mapper;
        #endregion
        #region Constructor

        public AccountService(IAcountRepsitory accountRepsitory, IConfiguration configuration, IMapper mapper) : base(accountRepsitory)
        {
            _accountRepsitory = accountRepsitory;
            errors = new Dictionary<string, List<string>>();
            _configuration = configuration;
            this.mapper = mapper;
        }

        #endregion
        #region Method

        public override void ValidateData(Account entity)
        {
            if (CheckValueExistByField(entity.Username))
            {
                errors.Add(MISAResourceEN.EmployeeCode, new List<string> { string.Format(MISAResourceVN.CodeNotDupllicate, Resource.MISAResourceVN.EmployeeCode, entity.Username) });
            }
            if (errors.Count > 0)
            {
                throw new MISAValidateException(errors);
            }
        }
        public async Task<object> Login(DTOAccount entity)
        {
            if (!_accountRepsitory.CheckAccountExist(entity))
            {
                errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountIsNotExist });
                throw new MISAValidateException(errors);
            }
            else
            {
                var userLogin = await _accountRepsitory.GetByUsername(entity.Username);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, entity.Username),
                    new Claim(ClaimTypes.Role, userLogin.Role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                userLogin.RefreshToken = refreshToken;
                userLogin.Expiration = DateTime.Now.AddDays(refreshTokenValidityInDays);
                await _accountRepsitory.UpdateAsync(userLogin);
                return new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo,
                };
            }
        }
        /// <summary>
        /// Hàm tạo token mới
        /// </summary>
        /// <param name="authClaims"></param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>new token</returns>
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        
        public async Task<object> RefreshToken(DTOToken tokenModel)
        {
            if (tokenModel is null)
            {
                return (MISAResourceVN.InvalidRequest);
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return (MISAResourceVN.InvalidToken);
            }

            string username = principal.Identity.Name;

            var user = await _accountRepsitory.GetByUsername(username);

            if (user == null || user.RefreshToken != refreshToken || user.Expiration <= DateTime.Now)
            {
                return (MISAResourceVN.InvalidToken);
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _accountRepsitory.UpdateAsync(user);

            return new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
                expiration = newAccessToken.ValidTo
            };
        }

        public async Task<object> Register(Account account)
        {
            ValidateData(account);
            var res = await _accountRepsitory.InsertData(account);
            return res;
        }

        public Task<object> RegisterAdmin(Account model)
        {
            throw new NotImplementedException();
        }

        public Task<object> Revoke(string username)
        {
            var res  = _accountRepsitory.Revoke(username);
            return res;
        }
        /// <summary>
        /// Hàm tạo refresh token mới
        /// </summary>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>new refreshtoken</returns>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>principal</returns>
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException(MISAResourceVN.InvalidToken);

            return principal;

        }
        #endregion
    }
}
