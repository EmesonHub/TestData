using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Test.Data.DTO;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Test.Data.Utilities
{
    public class ApplicationToken : ControllerBase
    {
        //public IConfiguration Configuration { get; }
        //public ApplicationToken(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public string BuildToken(string id, string username)
        {
            var Key_ = JWTClass.JWT_Key;
            var Issuer_ = JWTClass.JWT_Issuer;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key_));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("valid", "1"),
                new Claim("userid", id),
                new Claim("username", username)
            };

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(Issuer_, //Issuer   
                            Issuer_,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt_token;
        }

       
        public TokenObj ValidateToken(HttpContext http)
        {
            var tkn = new TokenObj { };

            if (http!=null)
            {
                tkn.Id = int.Parse(http.User?.Claims.Where(p => p.Type == "userid").FirstOrDefault().Value.ToString());
                tkn.Name = http.User?.Claims.Where(p => p.Type == "username").FirstOrDefault().Value.ToString();
                tkn.Message = "Ok";
                tkn.Code = 1;
            }
            else
            {
                tkn.Id = tkn.Code = 0;
                tkn.Message = "Unauthorized";
                tkn.Name = "";
            }

            return tkn;
        }
    }
}
