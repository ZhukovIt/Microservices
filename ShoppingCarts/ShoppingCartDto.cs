using ShoppingCartLogic.ShoppingCarts;

namespace Api.ShoppingCarts
{
    public class ShoppingCartDto
    {
        public long Id { get; set; }

        public int UserId { get; set; }

        public List<ShoppingCartItemDto> ShoppingCartItems { get; set; }

        public ShoppingCartDto(ShoppingCart shoppingCart)
        {
            Id = shoppingCart.Id;
            UserId = shoppingCart.UserId;
            ShoppingCartItems = shoppingCart.Items.Select(x => new ShoppingCartItemDto()
            {
                Id = x.Id,
                ProductCatalogId = x.ProductCatalogId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription.Value.HasValue ? x.ProductDescription.Value.Value : null,
                Money = new MoneyDto()
                {
                    Amount = x.Money.Amount,
                    Currency = x.Money.Currency
                }
            }).ToList();
        }
    }
}
