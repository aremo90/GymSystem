using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Repositroies.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        // GetById
        TEntity? GetByID(int id);
        // GetAll
        IEnumerable<TEntity> GetAll(Func<TEntity , bool> ? condition = null);
        // Insert
        int Add(TEntity entity);
        // Update
        int Update(TEntity entity);
        // Delete
        int Delete(int id);
        void Delete(Membership member);
    }
}
