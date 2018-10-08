using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAsync_4
{
	public class UserDbContext : DbContext
	{
		public UserDbContext(): base("userDbConnection")
		{}

		public	DbSet<User> Users { get; set; }
	}
}
