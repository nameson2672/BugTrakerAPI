using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Web.Http;
using BugTrakerAPI.DatabaseTableModel;
using BugTrakerAPI.Model;
using BugTrakerAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BugTrakerAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TeamAdminAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _db;
        private readonly string _modelName;


        public TeamAdminAttribute(string modelName)
        {
            _modelName = modelName;
        }
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var dbContext = context.HttpContext
            .RequestServices
            .GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            var model = context.ActionArguments[_modelName] as AddUserToTeam;
            var correntUser = context.HttpContext.User.Identities.ToList()[0].Claims.First().Value;
            var teamAdminObject = new TeamAdmin() { teamId = model.teamId, userId = correntUser };
            var isCurrentUserIsATeamAdmin = dbContext.TeamAdmins.Find(teamAdminObject.teamId, teamAdminObject.userId);
            if (isCurrentUserIsATeamAdmin == null)
            {
               
                var ers = "You are not an admin of the team";
                var json = new
                {
                    success = false,
                    data = "null",
                    errors = new List<string> { ers }
                };
                // we can write our own custom response content here
                context.Result = new JsonResult(json);
                context.HttpContext.Response.StatusCode = 401;
            }
        }
    }
}