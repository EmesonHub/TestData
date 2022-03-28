using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Test.Utilities
{
    public class ApplicationToken
    {
        public IConfiguration Configuration { get; }
        public ApplicationToken(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string BuildToken(string id, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
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
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"], //Issuer   
                            Configuration["Jwt:Issuer"],  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt_token;
        }
    }
}
