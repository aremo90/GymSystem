using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Models
{
    internal class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;



        #region 1 : M Between Session and Category

        public ICollection<Session> Sessions { get; set; } // Many

        #endregion
    }
}
