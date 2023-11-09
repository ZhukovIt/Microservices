using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microservices.ShoppingCart;
using Newtonsoft.Json;
using ShoppingCart.Abstractions;

namespace ShoppingCart.Events
{
    public sealed class EventStore : IEventStore
    {
        private readonly ApplicationDBContext m_Context;
        //----------------------------------------------------------
        public EventStore(ApplicationDBContext _Context)
        {
            m_Context  = _Context;
        }
        //----------------------------------------------------------
        public void Raise(string _EventName, object _EventContent)
        {
            Event newEvent = new Event()
            {
                OccuredAt = DateTimeOffset.UtcNow,
                Name = _EventName,
                Content = JsonConvert.SerializeObject(_EventContent)
            };
            m_Context.Events.Add(newEvent);
            m_Context.SaveChanges();
        }
        //----------------------------------------------------------
        public IReadOnlyList<Event> GetEvents(long _Start, long _End)
        {
            return m_Context.Events
                .Where(e => e.Id >= _Start && e.Id <= _End)
                .OrderBy(e => e.Id)
                .ToList();
        }
        //----------------------------------------------------------
    }
}
