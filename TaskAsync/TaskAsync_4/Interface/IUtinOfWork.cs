using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAsync_4.Interface
{
	public interface IUtinOfWork : IDisposable
	{
		DbContext Context { get; }
		Task<int> CommitAsync();
	}
}
