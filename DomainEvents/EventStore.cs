using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShoppingCart.Abstractions;

namespace ShoppingCart.DomainEvents
{
    public sealed class EventStore : IEventStore
    {
        private long m_CurrentSequenceNumber;
        private readonly IList<Event> m_Database;
        //----------------------------------------------------------
        public EventStore()
        {
            m_CurrentSequenceNumber = 0;
            m_Database = new List<Event>();
        }
        //----------------------------------------------------------
        public void Raise(string _EventName, object _EventContent)
        {
            long seqNumber = Interlocked.Increment(ref m_CurrentSequenceNumber);
            m_Database.Add(new Event(seqNumber, DateTimeOffset.UtcNow, _EventName, _EventContent));
        }
        //----------------------------------------------------------
        public IReadOnlyList<Event> GetEvents(long _Start, long _End)
        {
            return m_Database
                .Where(e => e.Id >= _Start && e.Id <= _End)
                .OrderBy(e => e.Id)
                .ToList();
        }
        //----------------------------------------------------------
    }
}
