using ECommerceWebApplication.Data.Base;
using ECommerceWebApplication.Models;

namespace ECommerceWebApplication.Data.Services
{
    public class ProducersService : EntityBaseRepository<Producer>, IProducersService
    {
        public ProducersService(AppDbContext context) : base(context)
        {
        }
    }
}