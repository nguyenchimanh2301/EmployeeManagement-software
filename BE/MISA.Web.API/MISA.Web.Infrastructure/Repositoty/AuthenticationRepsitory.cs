using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.Web.Core.Authentication;
using MISA.Web.Core.Exceptions;
using MISA.Web.Core.Interfaces.Infrastructure;
using MISA.Web.Core.Resource;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Infrastructure.Repositoty
{
    public class AuthenticationRepsitory : IAuthenticationRepsitory
    {
        #region field
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        #endregion
        #region Constructor
        public AuthenticationRepsitory(
         UserManager<ApplicationUser> userManager,
         RoleManager<IdentityRole> roleManager,
         IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Hàm xử lý  đăng nhập
        /// </summary>
        /// <param name="model">Đối tượng đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>Accesstoken và Refreshtoken</returns>
        public async Task<object> Login( LoginModel model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                }
                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);

                return new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo,
                };
            }
            errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountIsNotExist });
            throw new MISAValidateException(errors);

        }
        /// <summary>
        ///  API đăng ký thông tin
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>Thông báo đăng ký thành công hoặc thất bại</returns>

        public async Task<object> Register([FromBody] RegisterModel model)
        {
            Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountIsExist });
                throw new MISAValidateException(errors);
            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountNotValid });
                return (StatusCodes.Status500InternalServerError,
                                 new MISAValidateException(errors));
            }

            return (new Response { Status = MISAResourceVN.Success , Message = MISAResourceVN.AccounTCreatedSuccess });
        }
        /// <summary>
        ///  Hàm đăng ký thông tin quản trị viên
        /// </summary>
        /// <param name="model">Đối tượng đăng ký</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns>Thông báo đăng ký thành công hoặc thất bại</returns>

        public async Task<object> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountIsExist });
                throw new MISAValidateException(errors);
            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountNotValid });
                throw new MISAValidateException(errors);
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return (new Response { Status = MISAResourceVN.Success, Message = MISAResourceVN.AccounTCreatedSuccess });

        }
        /// <summary>
        ///  Hàm làm mới token
        /// </summary>
        /// <param name="tokenModel">Đối tượng token cũ</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns> refresh token mới và access token mới </returns>
        public async Task<object> RefreshToken(TokenModel tokenModel)
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

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return (MISAResourceVN.InvalidToken);
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
                expiration = newAccessToken.ValidTo
            });
        }
        /// <summary>
        /// Hàm thu hồi token
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns> Thông báo trả về  </returns>
        public async Task<object> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                errors.Add(MISAResourceVN.Account, new List<string> { MISAResourceVN.AccountIsNotExist });
                throw new MISAValidateException(errors);
            };

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return (new Response { Status = MISAResourceVN.Success, Message = MISAResourceVN.AccounTCreatedSuccess });
        }
        /// <summary>
        /// </summary>
        /// CreatedBy NCManh(17/2/2024)
        /// <returns> refresh token mới và access token mới </returns>

        /*  [Route("revoke-all")]
          public async Task<IActionResult> RevokeAll()
          {
              var users = _userManager.Users.ToList();
              foreach (var user in users)
              {
                  user.RefreshToken = null;
                  await _userManager.UpdateAsync(user);
              }

              return NoContent();
          }*/
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
