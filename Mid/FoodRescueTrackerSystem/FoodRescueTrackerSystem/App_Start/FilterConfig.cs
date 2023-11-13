using FoodRescueTrackerSystem.Validators;
using System.Web;
using System.Web.Mvc;

namespace FoodRescueTrackerSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
