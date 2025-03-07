using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using ProductAPI.Protos;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
   
        private readonly ProductService.ProductServiceClient _productServiceClient;

        // Injecting the gRPC client via constructor
        public CustomerController(ProductService.ProductServiceClient productServiceClient)
        {
            _productServiceClient = productServiceClient;
        }

        // Get products for a specific customer
        [HttpGet("GetMyProducts/{customerId}")]
        public async Task<IActionResult> GetMyProducts(int customerId)
        {
            var request = new CustomerRequest { CustomerId = customerId };
            var productList = await _productServiceClient.GetProductsByCustomerIdAsync(request);

            var products = productList.Products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Price
            }).ToList();

            return Ok(products);
        }
    }
}
