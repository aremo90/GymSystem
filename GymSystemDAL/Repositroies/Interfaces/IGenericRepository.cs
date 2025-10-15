using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        // Insert
        void Add(TEntity entity);
        // Update
        void Update(TEntity entity);
        // Delete
        void Delete(TEntity entity);
        // GetAll
        IEnumerable<TEntity> GetAll(Func<TEntity , bool> ? condition = null);
        // GetById
        TEntity? GetByID(int id);
    }
}
