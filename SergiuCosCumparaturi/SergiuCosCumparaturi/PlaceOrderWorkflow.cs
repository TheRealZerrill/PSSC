using SergiuCosCumparaturi.Domain.Operations;


namespace SergiuCosCumparaturi.Domain
{
    public class PlaceOrderWorkflow
    {
        public async Task<ShoppingCartResult.IShoppingCartResult> PlaceOrderAsync(
            ShoppingCart cart,
            Address shippingAddress,
            Func<ProductCode, Task<ProductAvailability>> checkProductAvailability,
            Func<ProductCode, Task<decimal>> getProductPrice)
        {
            return await ShoppingCartOperations.ValidateShippingAddress(cart, shippingAddress)
                .BindAsync(validCart => ShoppingCartOperations.CalculateTotalPrice(validCart, getProductPrice))
                .BindAsync(validCart => ShoppingCartOperations.PlaceOrder(validCart, checkProductAvailability))
                .Match(
                    Right: validCart => ShoppingCartResult.OrderPlaced("Order placed successfully!") as ShoppingCartResult.IShoppingCartResult,
                    Left: error => ShoppingCartResult.OrderFailed(error.ToString()) as ShoppingCartResult.IShoppingCartResult
                );
        }
    }
}
