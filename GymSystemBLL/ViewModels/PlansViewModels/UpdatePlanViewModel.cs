using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels.PlansViewModels
{
    public class UpdatePlanViewModel
    {
        public string PlanName { get; set; } = null!;


        [Required(ErrorMessage = "Description is required")]
        [StringLength(200,MinimumLength =5, ErrorMessage = "Description must be between 5:200 Chars !")]
        public string Description { get; set; } = null!;



        [Required(ErrorMessage = "DurationDays is required")]
        [Range(1, 365, ErrorMessage = "Days must be between 1 and 365 days")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public decimal Price { get; set; }
    }
}
