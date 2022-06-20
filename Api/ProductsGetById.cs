using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class ProductsGetById
    {
        private readonly IProductData productData;

        public ProductsGetById(IProductData productData)
        {
            this.productData = productData;
        }

        [FunctionName("ProductsGetById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{productId:int}")] HttpRequest req,
            int productId,
            ILogger log)
        {
            var response = await productData.GetProductById(productId);

            return new OkObjectResult(response);
        }
    }
}
