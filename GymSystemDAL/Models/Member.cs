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
    }
}
