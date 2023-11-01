using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Abstractions;
using ShoppingCart.Models;

namespace ShoppingCart.ShoppingCart
{
    [ApiController]
    [Route("/shoppingcart")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartStore m_ShoppingCartStore;
        private readonly IProductCatalogClient m_ProductCatalog;
        private readonly IEventStore m_EventStore;
        //--------------------------------------------------------------------------------------
        public ShoppingCartController(
            IShoppingCartStore _ShoppingCartStore,
            IProductCatalogClient _ProductCatalog,
            IEventStore _EventStore)
        {
            m_ShoppingCartStore = _ShoppingCartStore;
            m_ProductCatalog = _ProductCatalog;
            m_EventStore = _EventStore;
        }
        //--------------------------------------------------------------------------------------
        [HttpGet("")]
        public string DefaultRoute()
        {
            return "Микросервис ShoppingCart работает!";
        }
        //--------------------------------------------------------------------------------------
        [HttpGet("/{userid:int}")]
        public ShoppingCartContent GetShoppingCartByUserId(int userid)
        {
            return m_ShoppingCartStore.Get(userid);
        }
        //--------------------------------------------------------------------------------------
        [HttpPost("/{userid:int}/items")]
        public async Task<ShoppingCartContent> AddNewProductsInUserShoppingCart(int userid, [FromBody] int[] _ProductCatalogIds)
        {
            ShoppingCartContent shoppingCart = m_ShoppingCartStore.Get(userid);
            IEnumerable<ShoppingCartItem> shoppingCartItems = await m_ProductCatalog
                .GetShoppingCartItems(_ProductCatalogIds)
                .ConfigureAwait(false);
            shoppingCart.AddItems(shoppingCartItems, m_EventStore);
            m_ShoppingCartStore.Save(shoppingCart);

            return shoppingCart;
        }
        //--------------------------------------------------------------------------------------
        [HttpDelete("/{userid:int}/items")]
        public ShoppingCartContent DeleteProductsInUserShoppingCart(int userid, [FromBody] int[] _ProductCatalogIds)
        {
            ShoppingCartContent shoppingCart = m_ShoppingCartStore.Get(userid);
            shoppingCart.RemoveItems(_ProductCatalogIds, m_EventStore);
            m_ShoppingCartStore.Save(shoppingCart);

            return shoppingCart;
        }
        //--------------------------------------------------------------------------------------
    }
}
