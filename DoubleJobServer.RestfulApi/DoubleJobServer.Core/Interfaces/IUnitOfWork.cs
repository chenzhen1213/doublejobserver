using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoubleJobServer.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
    }
}
