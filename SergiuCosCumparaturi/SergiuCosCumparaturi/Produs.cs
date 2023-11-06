using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SergiuCosCumparaturi.Domain.Operations
{
    public record ProductCode(string Value);
    public record Quantity(int Value);
    public record Address(string Street, string City, string ZipCode);

    public class ShoppingCartItem
    {
        public ProductCode ProductCode { get; }
        public Quantity Quantity { get; }

        public ShoppingCartItem(ProductCode productCode, Quantity quantity)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }

    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public void AddItem(string productCode, ShoppingCartItem item)
        {
            Items.Add(item);
        }
    }

    public record ProductAvailability(ProductCode ProductCode, int AvailableQuantity)
    {
        public static ProductAvailability InStock { get; internal set; }
    }

    [AsChoice]
    public static partial class ShoppingCartResult
    {
        public interface IShoppingCartResult
        {
            bool Match(Func<object, object> Right, Func<object, string> Left);
        }

        public record ProductNotFound(string Message) : IShoppingCartResult;
        public record InsufficientStock(string Message) : IShoppingCartResult;
        public record InvalidAddress(string Message) : IShoppingCartResult;
        public record ShoppingCartValid(ShoppingCart ShoppingCart) : IShoppingCartResult;
        public record ShoppingCartInvalid(string Message) : IShoppingCartResult;
    }
}
