using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightManager.Data
{
    public interface IDbContext
    {
        IQueryable<T> Set<T>() where T : class;
    }
}
