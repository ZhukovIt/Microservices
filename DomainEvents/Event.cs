using Microservices.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DomainEvents
{
    public sealed class Event : Entity
    {
        private DateTimeOffset m_OccuredAt;
        private string m_Name;
        private object m_Content;
        //------------------------------------------------------------
        public DateTimeOffset OccuredAt
        {
            get { return m_OccuredAt; }
        }
        //------------------------------------------------------------
        public string Name
        {
            get { return m_Name; }
        }
        //------------------------------------------------------------
        public object Content
        {
            get { return m_Content; }
        }
        //------------------------------------------------------------
        public Event(long _SequenceNumber, DateTimeOffset _OccuredAt, string _Name, object _Content) : base(_SequenceNumber)
        {
            m_OccuredAt = _OccuredAt;
            m_Name = _Name;
            m_Content = _Content;
        }
        //------------------------------------------------------------
    }
}
