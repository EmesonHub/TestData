using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Data;
using Test.Data.Models;
using System.Security.Claims;
using Test.Data.Utilities;
using Test.Data.DTO.In;
using Test.Data.DTO;
using Test.Data.Classes;

namespace Test.Controller
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {

        private readonly AppDBContext _Context;

        private readonly IConfiguration _Configuration;

        private readonly ILogger<TestController> _Logger;
        private static ILogger StaticLogger = ApplicationLogging.CreateLogger<TestController>();

        UserData Data_User = new UserData();
        AccountData Data_Account = new AccountData();
        ApplicationToken AppToken = new ApplicationToken();

        public TestController(AppDBContext context, IConfiguration configuration, ILogger<TestController> logger)
        {
            _Context = context;
            _Configuration = configuration;
            _Logger = logger;
        }

        /// <summary>
        /// Get Token
        /// </summary>
        /// <returns></returns>
        
        [AllowAnonymous]
        [HttpGet("/generate/token")]
        public object Token()
        {
            var token = AppToken.BuildToken("1", "Eme Ole");
            return Ok(new
            {
                Token = token
            });
        }




        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet("/accounts")]
        public async Task<object> GetAllAccounts()
        {
            //var tkn = new TokenObj { };

            //var idCliam = HttpContext.User?.Claims.Where(p => p.Type == "userid").FirstOrDefault().Value;
            //if (User.Identity is ClaimsIdentity identity)
            //{
            //    IEnumerable<Claim> claims = identity.Claims;
            //    tkn.Id = int.Parse(claims.Where(p => p.Type == "userid").FirstOrDefault()?.Value.ToString());
            //    tkn.Name = claims.Where(p => p.Type == "username").FirstOrDefault()?.Value.ToString();
            //    tkn.Message = "Ok";
            //    tkn.Code = 1;
            //}
            //else
            //{
            //    tkn.Id = tkn.Code = 0;
            //    tkn.Message = "Unauthorized";
            //    tkn.Name = "";
            //}

            var token = AppToken.ValidateToken(HttpContext);

            return await Data_Account.GetAllAccounts();
        }

        /// <summary>
        /// Get an account by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("/accounts/{id}")]
        public async Task<object> GetAccountById(int id)
        {
            return await Data_Account.GetAnAccountById(id);
        }

        /// <summary>
        /// Add new account
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/accounts")]
        public async Task<object> CreateAccount([FromBody] New_Account data)
        {
            return Ok(await Data_Account.CreateNewAccount(data));
        }





        /// <summary>
        /// Update account record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("/accounts/{id}")]
        public async Task<object> PutAccount(int id, [FromBody] New_Account data)
        {
            return Ok( await Data_Account.UpdateAccount(id, data));
        }




        /// <summary>
        /// Delete account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/accounts/{id}")]
        public async Task<object> DeleteAccount(int id)
        {


            //var identity = User.Identity as ClaimsIdentity;
            //var userid = 0;
            //if (identity != null)
            //{
            //    IEnumerable<Claim> claims = identity.Claims;

            //    var name = claims.Where(p => p.Type == "userid").FirstOrDefault()?.Value;

            //    userid = int.Parse(name);

            //}

            //if (userid == 0) { return Ok(new { code = 0, message = ApplicationMessage.INVALIDTOKEN }); }


            return Ok(await Data_Account.DeleteAnAccount(id));

        }







        /// <summary>
        /// Get an account by id and its users
        /// </summary>
        /// <returns></returns>
        [HttpGet("/accounts/{accountid}/users")]
        public async Task<object> GetAllAccountUserss(int accountid)
        {
            return await Data_User.GetAllUsersByAccountId(accountid);
        }



        /// <summary>
        /// Get an account by id and by userid
        /// </summary>
        /// <returns></returns>
        [HttpGet("/accounts/{accountid}/users/{userid}")]
        public async Task<object> GetAllAccountUsersById(int accountid, int userid)
        {
            return await Data_User.GetAllUsersByAccountIdUserId(accountid, userid);
        }


        /// <summary>
        /// Add new account user
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/accounts/users")]
        public async Task<object> CreateAccountUser([FromBody] New_User data)
        {
            
            return Ok(await Data_User.CreateAccountUser(data));
        }





        /// <summary>
        /// Update account user record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("/accounts/users/{id}")]
        public async Task<object> PutAccountUser(int id, [FromBody] New_User data)
        {
            return Ok(await Data_User.UpdateAccountUser(id, data));
        }




        /// <summary>
        /// Delete account user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/accounts/users/{id}")]
        public async Task<object> DeleteAccountUser(int id)
        {
            return Ok(await Data_User.DeleteAccountUser(id));

        }
    }
}
