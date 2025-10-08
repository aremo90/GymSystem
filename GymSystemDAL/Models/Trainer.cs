using GymSystemDAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Models
{
    internal class Trainer : GymUser
    {
        // CreatedAd will be inherited from BaseEntity and it name will be altered to HireDate
        public Specialites Specialites { get; set; }

    }
}
