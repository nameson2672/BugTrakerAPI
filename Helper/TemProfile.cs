using AutoMapper;
using BugTrakerAPI.DatabaseTableModel;
using BugTrakerAPI.Model;
using BugTrakerAPI.Model.ReturnModel;
using BugTrakerAPI.ViewModel;

namespace BugTrakerAPI.Helper
{
    public class TemProfile : Profile
    {
        public TemProfile()
        {
            CreateMap<TeamViewModel, Team>();
            CreateMap<Team, TeamResponseData>();
            CreateMap<UserInfoModel, TeamAdminRes>().ForMember(dest =>
            dest.userId,
            opt => opt.MapFrom(src => src.Id));
        }

    }
}