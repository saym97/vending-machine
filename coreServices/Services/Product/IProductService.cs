using coreServices.DTOs;
using coreServices.DTOs.Product;
using coreServices.DTOs.Product.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.Services.Product
{
    public interface IProductService
    {
        ProductDTO AddProduct(ProductDTO product);
        IEnumerable<ProductDTO> GetAllAvailableProducts();
        ProductDTO UpdatePrice(UpdateProductDTO productDto);
        ProductDTO UpdateProductName(UpdateProductDTO productDto);
        GenericResponse DeleteProduct(Guid productId);
        ProductDTO UpdateQuantity(UpdateProductDTO productDto);
        DTOs.Product.Out.BuyProductDTO BuyProduct(Guid userId, DTOs.Product.In.BuyProductDTO product);
    }
}
