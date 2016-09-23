using System;

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
