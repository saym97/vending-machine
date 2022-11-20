using coreServices.DTOs.Product;
using dbContext.VendingMachine.Entities;

namespace coreServices.AutoMapperProfile
{
    public class ProductMappingProfile:AutoMapper.Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Products, ProductDTO>();
            CreateMap<ProductDTO,Products>();
        }
    }
}
