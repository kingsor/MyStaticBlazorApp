using System.Collections.Generic;

namespace Data
{
    public class ProductsResponse : BaseResponse
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
