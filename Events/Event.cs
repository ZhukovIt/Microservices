using Microservices.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Events
{
    public sealed class Event : Entity
    {
        public DateTimeOffset OccuredAt { get; set; }
        //------------------------------------------------------------
        public string Name { get; set; }
        //------------------------------------------------------------
        public string Content { get; set; }
        //------------------------------------------------------------
        public Event() : base()
        {

        }
        //------------------------------------------------------------
    }
}
