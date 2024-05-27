using Adminkp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adminkp.Interfaces
{
    public interface IUserInterface : IBaseInterface<Users>
    {
        IEnumerable<Users> GetAllList();
        Users Get(int userId);
        void AddUser(string firstName, string lastName, string login, string password, string email, int roleId);
        void DelUser(int userId);
    }
}
