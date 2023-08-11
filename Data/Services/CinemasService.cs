using ECommerceWebApplication.Data.Base;
using ECommerceWebApplication.Models;

namespace ECommerceWebApplication.Data.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext context) : base(context)
        {
        }
    }
}