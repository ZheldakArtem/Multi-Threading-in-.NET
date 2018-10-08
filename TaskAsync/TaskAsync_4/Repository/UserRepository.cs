using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAsync_4.Interface;

namespace TaskAsync_4.Repository
{
	public class UserRepository : IRepositoryAsync<User>
	{
		private readonly DbContext _context;
		private readonly IUtinOfWork _uow;

		public UserRepository(IUtinOfWork uow)
		{
			_uow = uow;
			_context = uow.Context;
		}

		public async Task<User> Create(User item)
		{
			var createdUser = await Task.Run(() => _context.Set<User>().Add(item));
			await _uow.CommitAsync();

			return createdUser;
		}

		public async Task<User> Delete(int id)
		{
			var deletedUser = await Task.Run(() =>
			{
				 var delUser = _context.Set<User>().FirstOrDefault(u => u.Id == id);

				return delUser != null ? _context.Set<User>().Remove(delUser) : null;
			 });

			await _uow.CommitAsync();

			return deletedUser;
		}

		public async Task<User> Get(int id)
		{
			var user = await Task.Run(() =>
			{
				return _context.Set<User>().FirstOrDefault(u => u.Id == id);
			});

			await _uow.CommitAsync();

			return user;
		}

		public async Task<User> Update(User item)
		{
			var updatedUser = await Task.Run(() => {
				var user = _context.Set<User>().FirstOrDefault(u => u.Id == item.Id);

				if (user != null)
				{
					user.Name = item.Name;
					user.Surname = item.Surname;
					user.Age = item.Id;
				}

				return user;
			});

			await _uow.CommitAsync();

			return updatedUser;
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
