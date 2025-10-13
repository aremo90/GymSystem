using GymSystemBLL.ViewModels;
using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel createdMember);

        MemberViewModel? GetMemberDeatails(int id);

        // Get Health record
        HealthViewModel? GetMemberHealthRecord(int memberId);

        // GetMemberId to update View
        MemberToUpdateViewModel? GetMemberToUpdate(int memberId);

        // apply update
        bool UpdateMember(int memberId, MemberToUpdateViewModel updatedMember);

        // Delete Member
        bool DeleteMember(int memberId);
    }
}
