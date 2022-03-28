using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data.Models;
using Test.Data.Utilities;
using Test.Data.DTO.In;
using Test.Data.DTO.Out;

namespace Test.Data.Classes
{
    public class UserData
    {
        private readonly AppDBContext _Context = new AppDBContext();


        public async Task<object> GetUsersById(int Id)
        {
            return await _Context.Users.Where(x => x.AccountId == Id).ToListAsync();
        }

        public async Task<object> GetAllUsersByAccountId(int id)
        {
            return await _Context.Users.Where(x => x.AccountId == id).ToListAsync();
        }

        public async Task<object> GetAllUsersByAccountIdUserId(int accountid, int userid)
        {
            return await _Context.Users.Where(x => x.AccountId == accountid && x.Id == userid).ToListAsync();
        }

        public async Task<object> CreateAccountUser(New_User data)
        {

            if (data.Email == "") { return (new { code = 0, message = "The email is required" }); }
            if (data.FirstName == "") { return (new { code = 0, message = "The firstname is required" }); }

            //check if account exist
            var check_account = await _Context.Accounts.Where(x => x.Id == data.AccountId).FirstOrDefaultAsync();
            if (check_account == null) { return (new { code = 0, message = "The account entered not found" }); }

            //check duplicate email
            var check_duplicate_email = await _Context.Users.Where(x => x.Email == data.Email).FirstOrDefaultAsync();
            if (check_duplicate_email != null) { return (new { code = 0, message = "Email already exist" }); }

            var acc = new User
            {
                AccountId = data.AccountId,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email
            };

            _Context.Users.Add(acc);
            await _Context.SaveChangesAsync();

            return (new
            {
                code = 1,
                message = ApplicationMessage.SAVE
            });
        }

        public async Task<object> UpdateAccountUser(int id, New_User data)
        {

            if (data.Email == "") { return (new { code = 0, message = "The email is required" }); }
            if (data.FirstName == "") { return (new { code = 0, message = "The firstname is required" }); }

            var check_duplicate_email = await _Context.Users.Where(x => x.Email == data.Email && x.Id != id).FirstOrDefaultAsync();
            if (check_duplicate_email != null)
            {
                return (new
                {
                    code = 0,
                    message = "Duplicate record found for the email"
                });
            }

            var entity = await _Context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.FirstName = data.FirstName;
                entity.LastName = data.LastName;
                entity.Email = data.Email;

                await _Context.SaveChangesAsync();
                return (new
                {
                    code = 1,
                    message = ApplicationMessage.UPDATE
                });
            }
            return (new { code = 0, message = "No record found for the Id entered" });


        }

        public async Task<object> DeleteAccountUser(int id)
        {
            
            var chk = await _Context.Users.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (chk == null)
            {
                return (new
                {
                    code = 0,
                    message = ApplicationMessage.NOTFOUND
                });
            }

            //remove the account
            _Context.Remove(chk);
            await _Context.SaveChangesAsync();
            return (new
            {
                code = 1,
                message = ApplicationMessage.DELETE
            });


        }
    }
}
