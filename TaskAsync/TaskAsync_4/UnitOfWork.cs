using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAsync_4.Interface;

namespace TaskAsync_4
{
	public class UnitOfWork : IUtinOfWork
	{
		public DbContext Context { get; }

		public UnitOfWork(DbContext dbContext)
		{
			this.Context = dbContext;
		}
		public async Task<int> CommitAsync()
		{
			int numberOfStateEntriesWritten = await this.Context?.SaveChangesAsync();

			return numberOfStateEntriesWritten;
		}

		public void Dispose()
		{
			Context?.Dispose();
		}
	}
}
