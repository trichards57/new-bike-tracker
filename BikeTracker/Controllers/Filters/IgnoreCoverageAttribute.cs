using System;

namespace BikeTracker.Controllers.Filters
{
    [IgnoreCoverage, AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class IgnoreCoverageAttribute : Attribute
    {
    }
}