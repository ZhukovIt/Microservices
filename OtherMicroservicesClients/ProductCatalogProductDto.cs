using Api.ShoppingCarts;

namespace Api.OtherMicroservicesClients
{
    public class ProductCatalogProductDto
    {
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public MoneyDto Money { get; set; }
    }
}
