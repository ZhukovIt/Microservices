using Api.Utils;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartLogic.Events;
using ShoppingCartLogic.OtherMicroservicesClients;
using ShoppingCartLogic.ShoppingCarts;
using ShoppingCartLogic.Utils;

namespace Api.ShoppingCarts
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : BaseController
    {
        private readonly ShoppingCartStore _ShoppingCartStore;
        private readonly ProductCatalogClient _ProductCatalog;
        private readonly EventStore _EventStore;
        //--------------------------------------------------------------------------------------
        public ShoppingCartController(
            UnitOfWork _UnitOfWork,
            ProductCatalogClient _ProductCatalog,
            ShoppingCartStore _ShoppingCartStore,
            EventStore _EventStore) : base(_UnitOfWork)
        {
            this._ShoppingCartStore = _ShoppingCartStore;
            this._ProductCatalog = _ProductCatalog;
            this._EventStore = _EventStore;
        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult DefaultRoute()
        {
            return Ok("Микросервис ShoppingCart работает!");
        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        [Route("{userid:int}")]
        public IActionResult GetShoppingCartByUserId(int userid)
        {
            Maybe<ShoppingCart> shoppingCartOrError = _ShoppingCartStore.GetByUserId(userid);
            if (shoppingCartOrError.HasNoValue)
                return Error($"Корзина покупок для пользователя с Id = {userid} отсутствует!");

            return Ok(shoppingCartOrError.Value);
        }
        //--------------------------------------------------------------------------------------
        [HttpPost]
        [Route("{userid:int}/items")]
        public async Task<ShoppingCart> AddNewProductsInUserShoppingCart(int userid, [FromBody] int[] _ProductCatalogIds)
        {
            ShoppingCart shoppingCart = _ShoppingCartStore.GetByUserId(userid);
            IEnumerable<ShoppingCartItem> shoppingCartItems = await _ProductCatalog
                .GetShoppingCartItems(_ProductCatalogIds)
                .ConfigureAwait(false);
            shoppingCart.AddItems(shoppingCartItems, _EventStore);
            _ShoppingCartStore.Add(shoppingCart);

            return shoppingCart;
        }
        //--------------------------------------------------------------------------------------
        [HttpDelete]
        [Route("{userid:int}/items")]
        public ShoppingCart DeleteProductsInUserShoppingCart(int userid, [FromBody] int[] _ProductCatalogIds)
        {
            ShoppingCart shoppingCart = _ShoppingCartStore.Get(userid);
            shoppingCart.RemoveItems(_ProductCatalogIds, _EventStore);
            _ShoppingCartStore.Save(shoppingCart);

            return shoppingCart;
        }
        //--------------------------------------------------------------------------------------
    }
}
