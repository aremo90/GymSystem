using GymSystemDAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name is Required !")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required !")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is Required !")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number must be a valid Egyption Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 9000, ErrorMessage = "BuildingNumber must be between 1 and 300.")]
        public int BuildingNumber { get; set; }



        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 30 characters.")]
        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces.")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "At Leaset one Specialty must be assigned !")]
        public Specialites Specialties { get; set; }
    }
}
