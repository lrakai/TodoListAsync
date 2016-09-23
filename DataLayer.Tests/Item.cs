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

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Item item = obj as Item;
            if ((Object)item == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Id == item.Id) && (DateTime == item.DateTime);
        }

        public bool Equals(Item item)
        {
            // If parameter is null return false:
            if ((object)item == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Id == item.Id) && (DateTime == item.DateTime);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
