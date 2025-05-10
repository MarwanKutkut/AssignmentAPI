using Microsoft.AspNetCore.Mvc;

namespace AssignmentAPI.Controllers
{
    public abstract class APIControllerBase : ControllerBase
    {
        public int CurrentUserId => int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("userId"))?.Value ?? "0");
    }
}
