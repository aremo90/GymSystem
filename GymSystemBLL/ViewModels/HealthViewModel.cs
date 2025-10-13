using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels
{
    public class HealthViewModel
    {
        [Required(ErrorMessage = "Height is Required !")]
        [Range(1, 300, ErrorMessage = "Height must be Greater than 0")]
        public decimal Height { get; set; }


        [Required(ErrorMessage = "Weight is Required !")]
        [Range(1, 500, ErrorMessage = "Weight must be Greater than 0 and less than 500")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "BloodType is Required !")]
        [StringLength(3, ErrorMessage = "BloodType must be 3 Chars or Less")]
        public string BloodType { get; set; }

        public string Note { get; set; }
    }
}
