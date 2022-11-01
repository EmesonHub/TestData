using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Data.Classes;
using Test.Data.DTO.In;

namespace Test.Data.Interfaces
{
    public interface IUserData 
    {
        Task<object> GetUsersById(int id);

        Task<object> GetAllUsersByAccountId(int id);

        Task<object> GetAllUsersByAccountIdUserId(int accountid, int userid);

        Task<object> CreateAccountUser(New_User data);

        Task<object> UpdateAccountUser(int id, New_User data);

        Task<object> DeleteAccountUser(int id);

    }
}
