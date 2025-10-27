using AutoMapper;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SeesionsViewModel;
using GymSystemDAL.Models;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _Mapper = mapper;
        }

        public IMapper Mapper { get; }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            //var sessions = _unitOfWork.GetRepository<Session>().GetAll();
            var sessions = _unitOfWork.SessionRepoository.GetAllSessionsWithDetails();
            if (!sessions.Any()) return [];
            return sessions.Select(s => new SessionViewModel
            {
                Id = s.Id,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Capacity = s.Capacity,
                trainerName = s.SessionTrainer.Name,
                CategoryName = s.SessionCategory.CategoryName,
                FreeSlots = s.Capacity - _unitOfWork.SessionRepoository.GetCountOfBookedSlots(s.Id)
            });
            
        }

        public SessionViewModel GetSessionById(int id)
        {
            var session = _unitOfWork.SessionRepoository.GetSessionWithTrainerAndCategory(id);
            if (session == null) return null;
            //return new SessionViewModel
            //{
            //    Id = session.Id,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    Caoacity = session.Capacity,
            //    trainerName = session.SessionTrainer.Name,
            //    CategoryName = session.SessionCategory.CategoryName,
            //    FreeSlots = session.Capacity - _unitOfWork.SessionRepoository.GetCountOfBookedSlots(session.Id)
            //};
            var MappedSession = _Mapper.Map<Session , SessionViewModel>(session);
            MappedSession.FreeSlots = MappedSession.Capacity - _unitOfWork.SessionRepoository.GetCountOfBookedSlots(session.Id);

            return MappedSession;

        }
        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                // Check if trainer exist
                // Check if Category Exist
                // Check StartDate < EndDate
                if (!IsTrainerExist(CreatedSession.TrainerId)) return false;
                if (!IsCategoryExist(CreatedSession.CategoryId)) return false;
                if (!IsValidDateRange(CreatedSession.StartDate , CreatedSession.EndDate)) return false;
                if (CreatedSession.Capacity < 0 || CreatedSession.Capacity > 25) return false;


                var SessionEntity = _Mapper.Map<Session>(CreatedSession);
                //var SessionEntity = _Mapper.Map<CreateSessionViewModel, Session>(CreatedSession);

                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public UpdateSessionViewModel GetSessionForUpdate(int id)
        {

            var session = _unitOfWork.SessionRepoository.GetByID(id);

            if (!IsSessionAvailableForUpdate(session!)) return null;

            return _Mapper.Map<UpdateSessionViewModel>(session);
        }

        public bool UpdateSession(UpdateSessionViewModel UpdatedSession, int id)
        {
            try
            {
                var session = _unitOfWork.SessionRepoository.GetByID(id);
                if (!IsSessionAvailableForUpdate(session!)) return false;
                if (!IsTrainerExist(UpdatedSession.TrainerId)) return false;
                if (!IsValidDateRange(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;

                _Mapper.Map(UpdatedSession, session);
                session.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepoository.Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSession(int id)
        {
            try
            {
                var session = _unitOfWork.SessionRepoository.GetByID(id);
                if (!IsSessionAvailableForDelete(session!)) return false;

                _unitOfWork.SessionRepoository.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;

            }
        }
        public IEnumerable<TrainerSelectViewModel> GetTrainerForSessions()
        {
            var trains = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _Mapper.Map<IEnumerable<TrainerSelectViewModel>>(trains);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForSessions()
        { 
            var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _Mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        #region Helper

        private bool IsTrainerExist(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetByID(TrainerId) is not null;
        }
        private bool IsCategoryExist(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetByID(CategoryId) is not null;
        }
        private bool IsValidDateRange(DateTime StartDate , DateTime EndDate)
        {
            return StartDate < EndDate;
        }

        private bool IsSessionAvailableForUpdate(Session session)
        {
            if (session is null) return false;

            if (session.EndDate < DateTime.Now ) return false;

            if (session.StartDate <= DateTime.Now) return false; // onGoing

            var ActiveBooking = _unitOfWork.SessionRepoository.GetCountOfBookedSlots(session.Id) > 0;
            if (ActiveBooking) return false;

            return true;
        }

        private bool IsSessionAvailableForDelete(Session session)
        {
            if (session is null) return false;

            if (session.EndDate < DateTime.Now) return false; // Completed

            if (session.StartDate > DateTime.Now) return false; // UpComing

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false; // onGoing

            var ActiveBooking = _unitOfWork.SessionRepoository.GetCountOfBookedSlots(session.Id) > 0; // Has Bookings
            if (ActiveBooking) return false;

            return true;
        }








        #endregion

    }
}
