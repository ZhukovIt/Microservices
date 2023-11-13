using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;
using ShoppingCartLogic.ShoppingCarts;
using CSharpFunctionalExtensions;
using FluentNHibernate.Conventions.Helpers;
using ShoppingCartLogic.Common;

namespace Api.OtherMicroservicesClients
{
    public class ProductCatalogClient
    {
        private readonly HttpClient client;
        private static readonly string ProductCatalogueBaseUrl = @"https://git.io/JeHiE";
        private static readonly string GetProductPathTemplate = "?productIds=[{0}]";

        public ProductCatalogClient(HttpClient client)
        {
            client.BaseAddress = new Uri(ProductCatalogueBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client = client;
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
        {
            string productsResource = string.Format(GetProductPathTemplate, string.Join(",", productCatalogIds));

            using HttpResponseMessage response = await client.GetAsync(productsResource);
            response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            List<ProductCatalogProductDto> productsDto = await JsonSerializer.DeserializeAsync<List<ProductCatalogProductDto>>(
                stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            return productsDto.Select(p => new ShoppingCartItem(
                (ForeignKeyId)(int)p.ProductId,
                (ProductName)p.ProductName,
                (ProductDescription)p.ProductDescription,
                Money.Create(p.Price.Amount, p.Price.Currency).Value));
        }
    }
}

