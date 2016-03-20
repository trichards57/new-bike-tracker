using System;

namespace BikeTracker.Controllers.Filters
{
    [IgnoreCoverage, AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    sealed class IgnoreCoverageAttribute : Attribute
    {
    }
}