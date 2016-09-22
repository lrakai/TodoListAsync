using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Contract
{
    /// <summary>
    /// Provides a DateTime property.
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTime DateTime { get; }
    }
}
