using Moq;
using Moq.Language.Flow;
using System.Threading.Tasks;

namespace BikeTracker.XTests.Helpers
{
    internal static class TaskResultHelper
    {
        public static IReturnsResult<TMock> ReturnsEmptyTask<TMock>(this ISetup<TMock, Task> setup)
            where TMock : class
        {
            return setup.Returns(Task.FromResult<object>(null));
        }

        public static IReturnsResult<TMock> ReturnsNullTask<TMock, TModel>(this ISetup<TMock, Task<TModel>> setup)
            where TMock : class
            where TModel : class
        {
            return setup.ReturnsAsync(() => null);
        }
    }
}