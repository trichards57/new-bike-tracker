using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeTracker.Core.Services.Interfaces
{
    public interface IImeiService
    {
        Task Delete(int id);

        Task<IEnumerable<IMEI>>
    }
}
