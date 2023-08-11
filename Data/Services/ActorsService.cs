using ECommerceWebApplication.Data.Base;
using ECommerceWebApplication.Models;

namespace ECommerceWebApplication.Data.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }
    }
}