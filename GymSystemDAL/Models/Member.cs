using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Models
{
    internal class Member : GymUser
    {
        // CreatedAd will be inherited from BaseEntity and it name will be altered to JoinDate
        public string? Photo { get; set; }


        #region one : one relationship between Member and HelthRecord

        public HealthRecord HealthRecord { get; set; } // One

        #endregion

        #region M : M between Member and Plan

        public ICollection<Membership> Memberships { get; set; }

        #endregion

        #region M : M between Member and Session

        public ICollection<MemberSession> MemberSessions { get; set; }

        #endregion
    }
}
