using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.TrainerViewModels;
using GymSystemDAL.Models;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();

                if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone)) return false;
                var Trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Specialties = createTrainer.Specialties,
                    Gender = createTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        City = createTrainer.City,
                        Street = createTrainer.Street,
                    }
                };


                Repo.Add(Trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || Trainers.Any()) return [];

            return Trainers.Select(X => new TrainerViewModel()
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Specialties = X.Specialties
            });
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetByID(trainerId);

            if (Trainer is null) return null;

            return new TrainerViewModel
            {
                Email = Trainer.Email,
                Name = Trainer.Name,
                Phone = Trainer.Phone,
                Specialties = Trainer.Specialties
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetByID(trainerId);
            if (Trainer is null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name, // Display
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Street = Trainer.Address.Street,
                BuildingNumber = Trainer.Address.BuildingNumber,
                City = Trainer.Address.City,
                Specialties = Trainer.Specialties
            };
        }

        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetByID(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetByID(trainerId);

            if (TrainerToUpdate is null || IsEmailExist(updatedTrainer.Email) || IsPhoneExist(updatedTrainer.Phone)) return false;

            TrainerToUpdate.Email = updatedTrainer.Email;
            TrainerToUpdate.Phone = updatedTrainer.Phone;
            TrainerToUpdate.Address.BuildingNumber = updatedTrainer.BuildingNumber;
            TrainerToUpdate.Address.Street = updatedTrainer.Street;
            TrainerToUpdate.Address.City = updatedTrainer.City;
            TrainerToUpdate.Specialties = updatedTrainer.Specialties;
            TrainerToUpdate.UpdatedAt = DateTime.Now;

            Repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;

        }




        #region Helper Methods

        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Phone == phone).Any();
        }
        private bool HasActiveSessions(int id)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(S => S.TrainerId == id && S.StartDate > DateTime.Now).Any();
            return activeSessions;
        }

        #endregion
    }


}
