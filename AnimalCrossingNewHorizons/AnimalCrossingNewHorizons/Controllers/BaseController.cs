using System.Reflection;
using System.Web.Mvc;

namespace AnimalCrossingNewHorizons.Controllers
{
    public class BaseController : Controller
    {
        protected class ButtonAttribute : ActionMethodSelectorAttribute
        {
            public string ButtonName { get; set; }
            public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
            {
                return controllerContext.Controller.ValueProvider.GetValue(ButtonName) != null;
            }
        }
    }
}