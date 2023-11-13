using Api.OtherMicroservicesClients;
using Api.Utils;
using CSharpFunctionalExtensions;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartLogic.Common;
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
        private readonly ShoppingCartItemStore _ShoppingCartItemStore;
        private readonly ProductCatalogClient _ProductCatalog;
        private readonly EventStore _EventStore;
        //--------------------------------------------------------------------------------------
        public ShoppingCartController(
            UnitOfWork _UnitOfWork,
            ProductCatalogClient _ProductCatalog,
            ShoppingCartStore _ShoppingCartStore,
            ShoppingCartItemStore _ShoppingCartItemStore,
            EventStore _EventStore) : base(_UnitOfWork)
        {
            this._ShoppingCartStore = _ShoppingCartStore;
            this._ShoppingCartItemStore = _ShoppingCartItemStore;
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
        [Route("{userid:int}")]
        public IActionResult AddNewShoppingCartByUserId(int userid)
        {
            Result<ForeignKeyId> userIdOrError = ForeignKeyId.Create(userid);
            if (userIdOrError.IsFailure)
                return Error(userIdOrError.Error);

            Maybe<ShoppingCart> shoppingCartOrError = _ShoppingCartStore.GetByUserId(userid);
            if (shoppingCartOrError.HasValue)
                return Error($"Корзина покупок для пользователя с Id = {userid} уже существует!");

            ShoppingCart shoppingCart = new ShoppingCart(userIdOrError.Value);
            _ShoppingCartStore.Add(shoppingCart);

            ShoppingCartDto dto = new ShoppingCartDto(shoppingCart);

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
            _ShoppingCartItemStore.Add(shoppingCart.Items);
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
