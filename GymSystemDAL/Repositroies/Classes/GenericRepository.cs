using GymSystemDAL.Data.Context;
using GymSystemDAL.Models;
using GymSystemDAL.Repositroies.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymSystemDbContext _dbContext;

        public GenericRepository(GymSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            
        }
        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Find(entity); 
        }
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition == null)
                return _dbContext.Set<TEntity>().ToList();
            else
                return _dbContext.Set<TEntity>().Where(condition).ToList();
        }
        public TEntity? GetByID(int id) => _dbContext.Set<TEntity>().Find(id);
        public void Update(TEntity entity)
        {                
          _dbContext.Set<TEntity>().Update(entity);          
        }
    }
}
