namespace SergiuCosCumparaturi.Domain.Operations
{
    public static class ShoppingCartOperations
    {

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

        private static bool IsValidAddress(Address address)
        {
       
            return true;
        }
    }
}
