namespace SergiuCosCumparaturi.Domain.Operations
{
    public static class ShoppingCartOperations
    {
        public static Option<ProductCode> ParseProductCode(string code)
        {
            if (!string.IsNullOrEmpty(code) && code.StartsWith("P"))
            {
                return Some(new ProductCode(code));
            }
            else
            {
                return None;
            }
        }

        public static TryAsync<ShoppingCartResult.IShoppingCartResult> AddItemToCart(
            ProductCode productCode, Quantity quantity, ShoppingCart cart, Func<ProductCode, Task<ProductAvailability>> checkProductAvailability)
        {
            return async () =>
            {
                var availability = await checkProductAvailability(productCode);
                return availability.Match(
                    Some: a =>
                    {
                        if (a.AvailableQuantity >= quantity.Value)
                        {
                            var newItem = new ShoppingCartItem(productCode, quantity);
                            cart.AddItem(newItem);
                            return ShoppingCartResult.ShoppingCartValid(cart) as ShoppingCartResult.IShoppingCartResult;
                        }
                        else
                        {
                            return new ShoppingCartResult.InsufficientStock($"Insufficient stock for product {productCode.Value}. Available quantity: {a.AvailableQuantity}") as ShoppingCartResult.IShoppingCartResult;
                        }
                    },
                    None: () => new ShoppingCartResult.ProductNotFound($"Product {productCode.Value} not found") as ShoppingCartResult.IShoppingCartResult
                );
            };
        }
    }
}
