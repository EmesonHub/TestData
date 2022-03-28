using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data.DTO.Out;
using Test.Data.DTO.In;
using Test.Data.Models;
using Test.Data.Utilities;

namespace Test.Data.Classes
{
    public class AccountData
    {
        private readonly AppDBContext _Context=new AppDBContext();
        UserData Data_User1 = new UserData();

        public async Task<object> GetAllAccounts()
        {
            return await _Context.Accounts.ToListAsync();
        }

        public async Task<object> GetAnAccountById(int id)
        {
            var data = await _Context.Accounts.Where(x => x.Id == id).Select(i => new Account_Users
            {
                Id = i.Id,
                CompanyName = i.CompanyName,
                Website = i.Website
            }).FirstOrDefaultAsync();

            if (data != null) { data.AssociatedUsers = await Data_User1.GetUsersById(data.Id); }

            return data;
        }

        public async Task<object> CreateNewAccount(New_Account data)
        {

            if (data.CompanyName == "") { return (new { code = 0, message = "The company is required" }); }

            //check duplicate
            var check_duplicate = await _Context.Accounts.Where(x => x.CompanyName == data.CompanyName).FirstOrDefaultAsync();
            if (check_duplicate != null) { return (new { code = 0, message = "Duplicate company name found" }); }


            var acc = new Account
            {
                CompanyName = data.CompanyName,
                Website = data.Website
            };

            _Context.Accounts.Add(acc);
            await _Context.SaveChangesAsync();

            return (new
            {
                code = 1,
                message = ApplicationMessage.SAVE
            });
        }

        public async Task<object> UpdateAccount(int id, New_Account data)
        {

            if (data.CompanyName == "") { return (new { code = 0, message = "The company name is required" }); }

            var check_duplicate = await _Context.Accounts.Where(x => x.CompanyName == data.CompanyName && x.Id != id).FirstOrDefaultAsync();
            if (check_duplicate != null)
            {
                return (new
                {
                    code = 0,
                    message = "Duplicate record found for the company name"
                });
            }

            var entity = await _Context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.CompanyName = data.CompanyName;
                entity.Website = data.Website;

                await _Context.SaveChangesAsync();
                return (new
                {
                    code = 1,
                    message = ApplicationMessage.UPDATE
                });
            }
            return (new { code = 0, message = "No record found for the Id entered" });


        }

        public async Task<object> DeleteAnAccount(int id)
        {
            var entity = await _Context.Accounts.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return (new
                {
                    code = 0,
                    message = ApplicationMessage.NOTFOUND
                });
            }

            //remove all associated users
            var users = await _Context.Users.Where(x => x.AccountId == id).ToListAsync();
            foreach (User us1 in users)
            {
                _Context.Remove(us1);
            }

            //remove the account
            _Context.Remove(entity);
            await _Context.SaveChangesAsync();
            return (new
            {
                code = 1,
                message = ApplicationMessage.DELETE
            });

        }
    }
}
