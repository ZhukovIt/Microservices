using ShoppingCart.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public sealed class ShoppingCartContent
    {
        private int m_UserId;
        private HashSet<ShoppingCartItem> m_Items;
        //--------------------------------------------------------------------------
        public ShoppingCartContent(int _UserId)
        {
            m_Items = new HashSet<ShoppingCartItem>();
            m_UserId = _UserId;
        }
        //--------------------------------------------------------------------------
        public void AddItems(IEnumerable<ShoppingCartItem> _ShoppingCartItems, IEventStore _EventStore)
        {
            foreach (ShoppingCartItem item in _ShoppingCartItems)
            {
                if (m_Items.Add(item))
                    _EventStore.Raise(
                        "ShoppingCartItemAdded",
                        new { m_UserId, item });
            }
        }
        //--------------------------------------------------------------------------
        public void RemoveItems(IEnumerable<int> _ProductCatalogIds, IEventStore _EventStore)
        {
            foreach (int productCatalogId in _ProductCatalogIds)
            {
                ShoppingCartItem currentItem = m_Items.FirstOrDefault(sci => sci.ProductId == productCatalogId);
                if (currentItem != null && m_Items.Remove(currentItem))
                    _EventStore.Raise(
                        "ShoppingCartItemDelete",
                        new { m_UserId, currentItem });
            }
        }
        //--------------------------------------------------------------------------
    }
}
