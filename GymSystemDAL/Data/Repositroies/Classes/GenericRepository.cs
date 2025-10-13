using GymSystemDAL.Data.Context;
using GymSystemDAL.Data.Repositroies.Interfaces;
using GymSystemDAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Repositroies.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymSystemDbContext _dbContext;

        public GenericRepository(GymSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            if (entity == null) return 0;
            _dbContext.Set<TEntity>().Remove(entity);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition == null)
                return _dbContext.Set<TEntity>().ToList();
            else
                return _dbContext.Set<TEntity>().Where(condition).ToList();
        }

        public TEntity? GetByID(int id) => _dbContext.Set<TEntity>().Find(id);

        public int Update(TEntity entity)
        {
            var existingEntity = _dbContext.Set<TEntity>().Find(entity.Id);
            if (existingEntity == null) return 0;
            _dbContext.Set<TEntity>().Update(entity);
            return _dbContext.SaveChanges();
        }
    }
}
