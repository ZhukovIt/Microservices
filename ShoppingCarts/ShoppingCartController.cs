using Api.OtherMicroservicesClients;
using Api.Utils;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartLogic.Events;
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
                return NotFound($"Корзина покупок для пользователя с Id = {userid} отсутствует!");

            ShoppingCartDto dto = new ShoppingCartDto(shoppingCartOrError.Value);

            return Ok(dto);
        }
        //--------------------------------------------------------------------------------------
        [HttpPost]
        [Route("{userid:int}/items")]
        public async Task<IActionResult> AddNewProductsInUserShoppingCart(int userid, [FromBody] AddNewProductsDto items)
        {
            Maybe<ShoppingCart> shoppingCartOrError = _ShoppingCartStore.GetByUserId(userid);
            if (shoppingCartOrError.HasNoValue)
                return NotFound($"Корзина покупок для пользователя с Id = {userid} отсутствует!");
            ShoppingCart shoppingCart = shoppingCartOrError.Value;

            IEnumerable<ShoppingCartItem> shoppingCartItems = await _ProductCatalog
                .GetShoppingCartItems(items.ProductCatalogIds)
                .ConfigureAwait(false);
            shoppingCart.AddItems(shoppingCartItems, _EventStore);
            _ShoppingCartStore.Add(shoppingCart);

            ShoppingCartDto dto = new ShoppingCartDto(shoppingCart);

            return Ok(dto);
        }
        //--------------------------------------------------------------------------------------
        [HttpDelete]
        [Route("{userid:int}/items")]
        public IActionResult DeleteProductsInUserShoppingCart(int userid, [FromBody] DeleteProductsDto items)
        {
            Maybe<ShoppingCart> shoppingCartOrError = _ShoppingCartStore.GetByUserId(userid);
            if (shoppingCartOrError.HasNoValue)
                return NotFound($"Корзина покупок для пользователя с Id = {userid} отсутствует!");
            ShoppingCart shoppingCart = shoppingCartOrError.Value;

            shoppingCart.RemoveItems(items.ProductCatalogIds, _EventStore);
            _ShoppingCartStore.Add(shoppingCart);

            ShoppingCartDto dto = new ShoppingCartDto(shoppingCart);

            return Ok(dto);
        }
        //--------------------------------------------------------------------------------------
    }
}
