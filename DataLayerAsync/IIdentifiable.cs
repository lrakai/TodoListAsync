using System;

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
