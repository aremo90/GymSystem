using AutoMapper;
using GymSystemBLL.ViewModels.SeesionsViewModel;
using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //CreateMap<Session, SessionViewModel>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.trainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.FreeSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<UpdateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
        }
    }
}
