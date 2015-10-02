using System.Web.Mvc;

namespace BikeTracker
{
    /// <summary>
    /// Setup class to configure the main routing filters
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filter collection to add to.</param>
        /// <remarks>
        /// Registers the Error Attribute handling.
        /// </remarks>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}