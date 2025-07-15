using AdminPanel.BLL.Service;
using AdminPanel.DAL.Interfaces;
using AdminPanel.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.BLL.Managers
{
    public class UserManager : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserManager(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
        // ✅ Giriş kontrolü yapan metot
        public async Task<User?> AuthenticateAsync(string name, string password)
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.Name == name && u.Password == password);
        }
    }
}
