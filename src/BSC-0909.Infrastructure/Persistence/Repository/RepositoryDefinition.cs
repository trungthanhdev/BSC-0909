using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSC_0909.Domain.Interfaces;
using CTCore.DynamicQuery.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BSC_0909.Infrastructure.Persistence.Repository
{
    public class RepositoryDefinition<TEntity> : IRepositoryDefinition<TEntity> where TEntity : class, IEntityClass
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly AppDbContext _dbContext;
        public RepositoryDefinition(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddList(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetById(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveList(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}