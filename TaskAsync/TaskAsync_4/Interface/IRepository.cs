using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAsync_4.Interface
{
	public interface IRepositoryAsync<T> : IDisposable where T : class
	{
		Task<T> Get(int id);
		Task<T> Create(T item);
		Task<T> Update(T item);
		Task<T> Delete(int id);
	}
}
