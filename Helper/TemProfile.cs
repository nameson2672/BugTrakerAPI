using AutoMapper;
using BugTrakerAPI.DatabaseTableModel;
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
        }

    }
}