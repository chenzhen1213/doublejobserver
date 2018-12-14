using System;
using System.Collections.Generic;
using System.Text;
using DoubleJobServer.Core.Interfaces;
using System.Threading.Tasks;

namespace DoubleJobServer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _myContext;

        public UnitOfWork(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<bool> SaveAsync()
        {
            return await _myContext.SaveChangesAsync() > 0;
        }
    }
}
