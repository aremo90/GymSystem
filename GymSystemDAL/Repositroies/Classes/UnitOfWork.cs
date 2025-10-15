using GymSystemDAL.Data.Context;
using GymSystemDAL.Repositroies.Classes;
using GymSystemDAL.Repositroies.Interfaces;
using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositroies.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type , object> _repositiores = new Dictionary<Type , object>();
        private readonly GymSystemDbContext _dbContext;

        public UnitOfWork(GymSystemDbContext dbContext , ISessionRepoository sessionRepoository)
        {
            _dbContext = dbContext;
            SessionRepoository = sessionRepoository;
        }

        public ISessionRepoository SessionRepoository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);
            if (_repositiores.TryGetValue(EntityType, out var Repo))
                return (IGenericRepository<TEntity>)Repo;
            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _repositiores[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
