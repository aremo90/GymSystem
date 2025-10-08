using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Models
{
    internal class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }



        #region 1 : M Between Session and Category
        //fk
        public int CategoryId { get; set; }

        public Category SessionCategory { get; set; }

        #endregion

        #region 1 : M Between Session and Trainer
        //fk
        public int TrainerId { get; set; }

        public Trainer SessionTrainer { get; set; }

        #endregion

        #region M : M between Member and Session

        public ICollection<MemberSession> SessionMembers { get; set; }

        #endregion
    }
}
