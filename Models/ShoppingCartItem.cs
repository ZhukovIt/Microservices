using Microservices.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public sealed class ShoppingCartItem : Entity
    {
        private readonly int m_ProductId;
        private readonly string m_ProductName;
        private readonly string m_ProductDescription;
        private readonly Money m_Price;
        //---------------------------------------------------------------
        public int ProductId
        {
            get { return m_ProductId; }
        }
        //---------------------------------------------------------------
        public string ProductName
        {
            get { return m_ProductName; }
        }
        //---------------------------------------------------------------
        public string ProductDescription
        {
            get { return m_ProductDescription; }
        }
        //---------------------------------------------------------------
        public Money Price
        {
            get { return m_Price; }
        }
        //---------------------------------------------------------------
        public ShoppingCartItem(int _ProductId, string _ProductName, string _ProductDescription, Money _Price)
        {
            m_ProductId = _ProductId;
            m_ProductName = _ProductName;
            m_ProductDescription = _ProductDescription;
            m_Price = _Price;
        }
        //---------------------------------------------------------------
    }
}
