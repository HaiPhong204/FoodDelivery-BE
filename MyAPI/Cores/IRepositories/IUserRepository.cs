using System;
using MyAPI.Models;

namespace MyAPI.Cores.IRepositories
{
	public interface IUserRepository
	{
        Task<UserModel?> GetById(Guid id);
    }
}

