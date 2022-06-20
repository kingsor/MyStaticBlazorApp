using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class ProductsGet
    {
        private readonly IProductData productData;

        public ProductsGet(IProductData productData)
        {
            this.productData = productData;
        }
        
        [FunctionName("ProductsGet")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequest req,
            ILogger log)
        {
            var response = await productData.GetProducts();

            return new OkObjectResult(response);
        }
    }
}
