using GymSystemDAL.Data.Context;
using GymSystemDAL.Data.Repositroies.Interfaces;
using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Repositroies.Classes
{
    internal class MemberRepository : IMemberRepository
    {
        // connection to the database
        private readonly GymSystemDbContext _dbContext = new GymSystemDbContext();
        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var member = _dbContext.Members.Find(id);
            //if (member != null)
            //{
            //    _dbContext.Members.Remove(member);
            //    return _dbContext.SaveChanges();
            //}
            //return 0;
            if (member is null) return 0;
            _dbContext.Members.Remove(member);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Member> GetAll()  => _dbContext.Members.ToList();

        public Member? GetById(int id) => _dbContext.Members.Find(id);

        public int Update(Member member)
        {
            var existingMember = _dbContext.Members.Find(member.Id);
            if (existingMember is null) return 0;
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
    }
}
