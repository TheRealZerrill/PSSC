namespace SergiuCosCumparaturi.Domain.Operations
{
    public static class ShoppingCartOperations
    {

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

        private static async Task<Either<string, ShoppingCart>> ValidateCartItems(
            ShoppingCart cart,
            Func<ProductCode, Task<ProductAvailability>> checkProductAvailability)
        {
   
            return Right(cart);
        }
    }
}
