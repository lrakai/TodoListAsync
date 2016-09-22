using DataLayer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Tests
{
    public class Item : IIdentifiable, IDateTimeProvider
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
    }
}
