using SergiuCosCumparaturi.Domain;
using SergiuCosCumparaturi.Domain.Operations;

namespace SergiuCosCumparaturi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var productCode = "PROD123";
            var cart = new ShoppingCart();
            cart.AddItem(productCode, 2); 

            var shippingAddress = new Address("Strada Exemplu, Nr. 123", "Orasul", "123456");

            var placeOrderWorkflow = new PlaceOrderWorkflow();

            async Task<ProductAvailability> CheckProductAvailability(ProductCode code)
            {
         
                return ProductAvailability.InStock;
            }

            async Task<decimal> GetProductPrice(ProductCode code)
            {
             
                return 100; 
            }

            var result = await placeOrderWorkflow.PlaceOrderAsync(cart, shippingAddress, CheckProductAvailability, GetProductPrice);

            Console.WriteLine(result.Match(
                Right: success => success.Message,
                Left: error => $"Order failed: {error}"
            ));

            Console.ReadLine(); 
        }
    }
}
