using Microsoft.EntityFrameworkCore;
using SchoolManangement.DataAccess.Data;
using SchoolManangement.DataAccess.Repository.Abstract;
using SchoolManangement.DataAccess.Repository.Generic;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SchoolManangement.DataAccess.UnitOfWorks.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolManangementDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories = new();
        public UnitOfWork(SchoolManangementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (_repositories.ContainsKey(type))
                return (IRepository<T>)_repositories[type];

            var repo = new Repository<T>(_dbContext);
            _repositories[type] = repo;
            return repo;
        }

        public void SetEntityState<T>(T entity, EntityState state) where T : class
        {
            _dbContext.Entry(entity).State = state;
        }
    }
}
