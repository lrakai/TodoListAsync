﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Contract
{
    /// <summary>
    /// Indicates an item cannot be inserted because an item with the same Id already exists.
    /// </summary>
    public class DataLayerAlreadyExistsException : Exception
    {
    }
}