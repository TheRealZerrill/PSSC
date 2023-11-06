namespace SergiuCosCumparaturi.Domain.Operations
{
    public static class ShoppingCartOperationsBase
    {

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

        public static Try<ShoppingCartResult.IShoppingCartResult> CalculateTotalPrice(ShoppingCart cart, Func<ProductCode, Task<decimal>> getProductPrice)
        {
            return async () =>
            {
                decimal totalPrice = 0;

                foreach (var item in cart.Items)
                {
                    var productPrice = await getProductPrice(item.ProductCode);
                    totalPrice += productPrice * item.Quantity.Value;
                }

                return ShoppingCartResult.ShoppingCartValid(cart) as ShoppingCartResult.IShoppingCartResult;
            };
        }
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

        public static TryAsync<ShoppingCartResult.IShoppingCartResult> PlaceOrder(
            ShoppingCart cart,
            Func<ProductCode, Task<ProductAvailability>> checkProductAvailability)
        {
            return async () =>
            {
                var validationResult = await ValidateCartItems(cart, checkProductAvailability);

                return validationResult.Match(
                    Right: validCart =>
                    {

                        return ShoppingCartResult.OrderPlaced("Order placed successfully!") as ShoppingCartResult.IShoppingCartResult;
                    },
                    Left: error => ShoppingCartResult.OrderFailed(error.ToString()) as ShoppingCartResult.IShoppingCartResult
                );
            };
        }
        // ...codul anterior...

        public static Try<ShoppingCartResult.IShoppingCartResult> ValidateShippingAddress(ShoppingCart cart, Address shippingAddress)
        {
            return () =>
            {
                if (IsValidAddress(shippingAddress))
                {
                    return ShoppingCartResult.ShoppingCartValid(cart) as ShoppingCartResult.IShoppingCartResult;
                }
                else
                {
                    return new ShoppingCartResult.InvalidAddress("Invalid shipping address") as ShoppingCartResult.IShoppingCartResult;
                }
            };
        }

        private static bool IsValidAddress(Address address)
        {
            // Implementați logica pentru validarea adresei aici
            // În acest exemplu, presupunem că orice adresă este validă
            return true;
        }

        private static async Task<Either<string, ShoppingCart>> ValidateCartItems(
            ShoppingCart cart,
            Func<ProductCode, Task<ProductAvailability>> checkProductAvailability)
        {

            return Right(cart);
        }
    }
}