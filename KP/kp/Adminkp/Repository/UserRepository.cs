using Adminkp.Interfaces;
using Adminkp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Adminkp.Repository
{
    public class UserRepository : IUserInterface
    {
        private readonly Model.ApplicationContext _dbContext;

        public UserRepository()
        {
            _dbContext = new Model.ApplicationContext();
        }
        public IEnumerable<Users> GetAllList()
        {
            var query =
                from users in _dbContext.Users
                join roles in _dbContext.Roles on users.role_id equals roles.role_id
                select new Users
                {
                    users_id = users.users_id,
                    first_name = users.first_name,
                    last_name = users.last_name,
                    login_user = users.login_user,
                    password_user = users.password_user,
                    email = users.email,
                    role_id = roles.role_id
                };
            return query.ToList();
        }
        public Users Get(int userId)
        {
            var user =
                from users in _dbContext.Users
                join roles in _dbContext.Roles on users.role_id equals roles.role_id
                where users.users_id == userId
                select new Users
                {
                    users_id = users.users_id,
                    first_name = users.first_name,
                    last_name = users.last_name,
                    login_user = users.login_user,
                    password_user = users.password_user,
                    email = users.email,
                    role_id = roles.role_id
                };
            return user.FirstOrDefault();
        }
        public void AddUser(string firstName, string lastName, string login, string password, string email, int roleId)
        {
            try
            {
                Users newUser = new Users
                {
                    first_name = firstName,
                    last_name = lastName,
                    login_user = login,
                    password_user = password,
                    email = email,
                    role_id = roleId
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании и добавлении пользователя: {ex.Message}", ex);
            }
        }

        public void DelUser(int userId)
        {
           
                int currentId = GlobalVariablesAdmin.AdminID;
                if (userId == currentId)
                {
                    MessageBox.Show("Нельзя удалить самого себя");
                    return;
                }

                var userToDelete = _dbContext.Users.FirstOrDefault(u => u.users_id == userId);

                if (userToDelete != null)
                {
                    _dbContext.Users.Remove(userToDelete);
                    _dbContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Данный пользователь имеет бронирования");
                }
            
        }
    }
}
