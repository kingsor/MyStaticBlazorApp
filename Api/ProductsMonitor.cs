using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace Api
{
    public class ProductsMonitor
    {
        private readonly IProductData productData;

        public ProductsMonitor(IProductData productData)
        {
            this.productData = productData;
        }

        [FunctionName("ProductsMonitor")]
        public void Run([TimerTrigger("0 30 */1 * * *")] TimerInfo timerInfo, ILogger logger)
        {
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var productsReport = productData.GetProductsReport().Result;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(productsReport, options);

            //logger.LogInformation(json);
            productData.SendNotifyEmail(json);
        }
    }
}
