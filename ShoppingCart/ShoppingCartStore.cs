using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microservices.ShoppingCart;
using ShoppingCart.Abstractions;
using ShoppingCart.Models;

namespace ShoppingCart.ShoppingCart
{
    public sealed class ShoppingCartStore : IShoppingCartStore
    {
        private readonly ApplicationDBContext m_Context;
        //---------------------------------------------------------------
        public ShoppingCartStore(ApplicationDBContext _Context)
        {
            m_Context = _Context;
        }
        //---------------------------------------------------------------
        public ShoppingCartContent Get(int _UserId)
        {
            if (!m_Database.ContainsKey(_UserId))
                m_Database[_UserId] = new ShoppingCartContent(_UserId);
            return m_Database[_UserId];
        }
        //---------------------------------------------------------------
        public void Save(ShoppingCartContent _ShoppingCart)
        {
            // Данная реализация имеет смысл, если использовать реальную БД
        }
        //---------------------------------------------------------------
    }
}
