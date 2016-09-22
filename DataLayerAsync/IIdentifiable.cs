using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Contract
{
    /// <summary>
    /// Provides and Id System.Guid.
    /// </summary>
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
