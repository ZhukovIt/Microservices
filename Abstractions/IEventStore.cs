using ShoppingCart.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Abstractions
{
    public interface IEventStore
    {
        void Raise(string _EventName, object _EventContent);

        IReadOnlyList<Event> GetEvents(long _Start, long _End);
    }
}
