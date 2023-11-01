using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Abstractions;

namespace ShoppingCart.DomainEvents
{
    [ApiController]
    [Route("api/events")]
    public class DomainEventsController : Controller
    {
        private IEventStore m_EventStore;
        //------------------------------------------------------------------------------------------
        public DomainEventsController(IEventStore _EventStore)
        {
            m_EventStore = _EventStore;
        }
        //------------------------------------------------------------------------------------------
        [HttpGet("")]
        public IReadOnlyList<Event> GetAllEventsInTheRange([FromQuery] long start, [FromQuery] long end)
        {
            return m_EventStore.GetEvents(start, end);
        }
        //------------------------------------------------------------------------------------------
    }
}
