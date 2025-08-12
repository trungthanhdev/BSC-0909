using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSC_0909.Domain.Interfaces
{
    public interface IRepositoryDefinition<TEntity> where TEntity : class, IEntityClass
    {
        void Add(TEntity entity);
        void AddList(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveList(IEnumerable<TEntity> entities);
        Task<TEntity?> GetById(string id);
        Task<List<TEntity>> GetAll();
    }
}