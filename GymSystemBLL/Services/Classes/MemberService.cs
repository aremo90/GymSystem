using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemDAL.Models;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        #region DB Connect

        private readonly IUnitOfWork _unitOfWork;
        //Conection to db
        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
        #region CRUD Methods

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            #region First Way
            //var members = _memberRepo.GetAll() ?? [];

            //var MemberViewModels = new List<MemberViewModel>();

            //foreach (var member in members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.Phone,
            //        Gender = member.Gender.ToString(),
            //    };
            //    MemberViewModels.Add(memberViewModel);
            //}
            //return MemberViewModels;
            #endregion
            #region Second Way

            var members = _unitOfWork.GetRepository<Member>().GetAll() ?? new List<Member>();

            var MemberViewModels = members.Select(member => new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
            });
            return MemberViewModels;
            #endregion
        }

        public bool CreateMember(CreateMemberViewModel createdMember)
        {
            try
            {
                // Check if phone or email are unique
                if (IsEmailExists(createdMember.Email) || IsPhoneExists(createdMember.Phone))
                    return false;

                var member = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    Gender = createdMember.Gender,
                    DateOfBirth = createdMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createdMember.BuildingNumber,
                        Street = createdMember.Street,
                        City = createdMember.City,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createdMember.HealthViewModel.Height,
                        Weight = createdMember.HealthViewModel.Weight,
                        BloodType = createdMember.HealthViewModel.BloodType,
                        Note = createdMember.HealthViewModel.Note,
                    }
                };
                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public MemberViewModel? GetMemberDeatails(int id)
        {
            // Create Plan Repo
            var member = _unitOfWork.GetRepository<Member>().GetByID(id);
            if (member == null) return null;

            var ViewModel = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBitrh = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address?.BuildingNumber} - {member.Address?.Street} - {member.Address?.City}"
            };
            var membership = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();
            if (membership is not null)
            {
                ViewModel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEndDate = membership.EndDate.ToShortDateString();

                // get plan name from plan repo
                var plan = _unitOfWork.GetRepository<Plan>().GetByID(membership.PlanId);
                ViewModel.PlanName = plan?.Name;
            }
            return ViewModel;

        }

        public HealthViewModel? GetMemberHealthRecord(int memberId)
        {
            var healthRecord = _unitOfWork.GetRepository<HealthRecord>().GetByID(memberId);
            if (healthRecord == null) return null;

            return new HealthViewModel()
            {
                BloodType = healthRecord.BloodType,
                Height = healthRecord.Height,
                Weight = healthRecord.Weight,
                Note = healthRecord.Note,
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetByID(memberId);
            if (member == null) return null;
            return new MemberToUpdateViewModel()
            {
                Email = member.Email,
                Phone = member.Phone,
                Name = member.Name,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
            };
        }

        public bool UpdateMember(int memberId, MemberToUpdateViewModel updatedMember)
        {
            try
            {
                var MemberRepo = _unitOfWork.GetRepository<Member>();
                //if (IsEmailExists(updatedMember.Email) || IsPhoneExists(updatedMember.Phone))
                //    return false;

                var emailExists = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Email == updatedMember.Email && X.Id != memberId);

                var phoneExists = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Phone == updatedMember.Phone && X.Id != memberId);

                if (emailExists.Any() || phoneExists.Any()) return false;

                var member = MemberRepo.GetByID(memberId);
                if (member is null) return false;


                member.Email = updatedMember.Email;
                member.Phone = updatedMember.Phone;
                member.Address.BuildingNumber = updatedMember.BuildingNumber;
                member.Address.Street = updatedMember.Street;
                member.Address.City = updatedMember.City;
                member.UpdatedAt = DateTime.Now;
                MemberRepo.Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteMember(int memberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var MmeberSessionRepo = _unitOfWork.GetRepository<MemberSession>();
            var MemberShipRepo = _unitOfWork.GetRepository<Membership>();

            var Member = MemberRepo.GetByID(memberId);
            if (Member is null) return false;

            // Check if the member has any active memberships
            var HasActiveMemberSeesions = MmeberSessionRepo.GetAll(X => X.MemberId == memberId && X.Session.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSeesions) return false;

            // Remove
            var Memberships = MemberShipRepo.GetAll(X => X.MemberId == memberId);
            try
            {
                if (Memberships.Any())
                {
                    foreach (var Membership in Memberships)
                    {
                        //_MemberShipRepo.Delete(member.MemberId);
                        MemberShipRepo.Delete(Membership);
                    }
                }
                MemberRepo.Delete(Member);

                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region Helper

        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExists(string Phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == Phone).Any();
        }

        #endregion



    }
}
