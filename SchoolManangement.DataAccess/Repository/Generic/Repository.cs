using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SchoolManangement.DataAccess.Data;
using SchoolManangement.DataAccess.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.DataAccess.Repository.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SchoolManangementDbContext _context;
        private readonly DbSet<T> _dbSet; // orm olarak efcore u kullanıyoruz . Bu sebeple dbset efcoredaki her bir tabloyu temsil eder
        public Repository(SchoolManangementDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);

        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;

        }
        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;

        }
        // liste halinde query işlemlerimizde paginition desteği sağladım . Aynı zamanda bazı durumlarda include gerekebileceği için include parametresi ekledim.
        //Liste halinde veri döneceği için efcore un tarcking özelliğini default olarak kapalı tuttum.
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter,
            bool tracked = false,
            int pageSize = 100,
            int pageNumber = 1,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            var query = CreateQuery(filter, include, tracked);

            if (pageSize > 0)
            {
                if (pageSize > 100)
                    pageSize = 100;

                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, bool tracked = true, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = CreateQuery(filter, include, tracked);
            return await query.FirstOrDefaultAsync();
        }


        public IQueryable<T> CreateQuery(Expression<Func<T, bool>>? predicate = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }
    }
}
