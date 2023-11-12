namespace Api.ShoppingCarts
{
    public class ShoppingCartItemDto
    {
        public long Id { get; set; }

        public MoneyDto Money { get; set; }

        public long ProductCatalogId { get; set; }

        public string ProductName { get; set; }

        public string? ProductDescription { get; set; }
    }
}
