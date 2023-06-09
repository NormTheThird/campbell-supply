using System.Runtime.Serialization;
using CampbellSupply.Common.Models;
using System.Collections.Generic;
using System.Data;

namespace CampbellSupply.Common.RequestAndResponses
{
    public class GetProductsRequest : BaseRequest { }

    public class GetProductsResponse : BaseResponse
    {
        public GetProductsResponse()
        {
            this.Products = new List<ProductModel>();
        }

        [DataMember(IsRequired = true)]
        public List<ProductModel> Products { get; set; }
    }


    public class UpdateProductRequest : BaseRequest
    {
        public UpdateProductRequest()
        {
            this.Product = new UploadModel();
        }

        [DataMember(IsRequired = true)]
        public UploadModel Product { get; set; }
    }

    public class UpdateProductResponse : BaseResponse { }
}