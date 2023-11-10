using Microsoft.AspNetCore.Mvc;
using ShoppingCartLogic.Events;

namespace Api.Events
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : Controller
    {
        private EventStore m_EventStore;
        //------------------------------------------------------------------------------------------
        public EventsController(EventStore _EventStore)
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
