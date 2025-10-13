using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemDAL.Data.Repositroies.Interfaces;
using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        #region Fields
        private readonly IGenericRepository<Member> _memberRepo;
        private readonly IGenericRepository<Membership> _MemberShipRepo;
        private readonly IPlanRepoository _PlanRepo;
        private readonly IGenericRepository<HealthRecord> _HealthRecordRepo;
        private readonly IGenericRepository<MemberSession> _MemberSessionRepo;
        #endregion

        public MemberService
            (IGenericRepository<Member> memberRepo,
            IGenericRepository<Membership> memberShipRepo,
            IPlanRepoository planRepo,
            IGenericRepository<HealthRecord> healthRecordRepo,
            IGenericRepository<MemberSession> memberSessionRepo)
        {
            _memberRepo = memberRepo;
            _MemberShipRepo = memberShipRepo;
            _PlanRepo = planRepo;
            _HealthRecordRepo = healthRecordRepo;
            _MemberSessionRepo = memberSessionRepo;
        }



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

            var members = _memberRepo.GetAll() ?? new List<Member>();

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
                return _memberRepo.Add(member) > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public MemberViewModel? GetMemberDeatails(int id)
        {
            // Create Plan Repo
            var member = _memberRepo.GetByID(id);
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
            var membership = _MemberShipRepo.GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();
            if (membership is not null)
            {
                ViewModel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEndDate = membership.EndDate.ToShortDateString();

                // get plan name from plan repo
                var plan = _PlanRepo.GetPlanById(membership.PlanId);
                ViewModel.PlanName = plan?.Name;
            }
            return ViewModel;

        }

        public HealthViewModel? GetMemberHealthRecord(int memberId)
        {
            var healthRecord = _HealthRecordRepo.GetByID(memberId);
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
            var member = _memberRepo.GetByID(memberId);
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
                if (IsEmailExists(updatedMember.Email) || IsPhoneExists(updatedMember.Phone))
                    return false;

                var member = _memberRepo.GetByID(memberId);
                if (member is null) return false;


                member.Email = updatedMember.Email;
                member.Phone = updatedMember.Phone;
                member.Address.BuildingNumber = updatedMember.BuildingNumber;
                member.Address.Street = updatedMember.Street;
                member.Address.City = updatedMember.City;
                member.UpdatedAt = DateTime.Now;
                return _memberRepo.Update(member) > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteMember(int memberId)
        {
            var Member = _memberRepo.GetByID(memberId);
            if (Member is null) return false;

            // Check if the member has any active memberships
            var HasActiveMemberSeesions = _MemberSessionRepo.GetAll(X => X.MemberId == memberId && X.Session.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSeesions) return false;

            // Remove
            var Membership = _MemberShipRepo.GetAll(X => X.MemberId == memberId);
            try
            {
                if (Membership.Any())
                {
                    foreach (var member in Membership)
                    {
                        //_MemberShipRepo.Delete(member.MemberId);
                        _MemberShipRepo.Delete(member);
                    }
                }
                return _memberRepo.Delete(memberId) > 0;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Helper

        private bool IsEmailExists(string email)
        {
            return _memberRepo.GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExists(string Phone)
        {
            return _memberRepo.GetAll(m => m.Phone == Phone).Any();
        }




        #endregion

    }
}
