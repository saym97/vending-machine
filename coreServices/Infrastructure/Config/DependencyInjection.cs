using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreServices.Services.User;
using coreServices.Services.Product;
using coreServices.AutoMapperProfile;
using dbContext.VendingMachine;
using Microsoft.EntityFrameworkCore;

namespace coreServices.Infrastructure.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VendingMachineContext>(options => options.UseSqlServer(configuration.GetConnectionString("DEV_VendingMachineDBConnStr")));
            //Services Interfaces
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();


            services.AddAutoMapper(x => x.AddProfile(new ProductMappingProfile()));
            services.AddAutoMapper(x => x.AddProfile(new UserMappingProfile()));
            return services;
        }
    }
}
