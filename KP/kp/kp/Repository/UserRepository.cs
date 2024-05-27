using kp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kp.Model;
using System.Data.Entity;

namespace kp.Repository
{
    public class UserRepository : IUserInterface
    {
        private readonly Model.AppContext dbContext;
        public UserRepository()
        {
            dbContext = new Model.AppContext();
        }
        public IEnumerable<Users> GetAllList()
        {
            return dbContext.Users.ToList();
        }
        public Users Get(int id)
        {
            return dbContext.Users.Find(id);
        }
        public bool AddUser(string username, string password, string firstname, string lastname, string address)
        {
            if (dbContext.Users.Any(u => u.login_user == username))
            {
                return false; 
            }

            var newUser = new Users
            {
                login_user = username,
                password_user = password,
                first_name = firstname,
                last_name = lastname,
                email = address,
                role_id = dbContext.Roles.FirstOrDefault(r => r.role_name == "Пользователь")?.role_id
            };
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            GlobalVariables.ClientID = newUser.users_id;
            return true; // Успешная регистрация
        }
    }
}
