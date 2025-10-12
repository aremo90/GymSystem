using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Repositroies.Interfaces
{
    internal interface IMemberRepository
    {
        //Get all Members
        IEnumerable<Member> GetAll();

        // Get Member by ID
        Member? GetById(int id);

        // Add a new Member
        int Add(Member member);

        // delete a Member
        int Delete(int id);

        // Update a Member
        int Update(Member member);
    }
}
