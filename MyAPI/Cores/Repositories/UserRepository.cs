using System;
using MyAPI.Cores.IRepositories;
using MyAPI.Models;

namespace MyAPI.Cores.Repositories
{
	public class UserRepository : IUserRepository
    {
        private DataContext _context { get; set; }
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> GetById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}

