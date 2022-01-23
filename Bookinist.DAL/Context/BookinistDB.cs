using Bookinist.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.DAL.Context
{
    public class BookinistDB : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Buyer> Buyers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        public DbSet<Deal> Deals { get; set; }


        public BookinistDB(DbContextOptions<BookinistDB> options) : base(options) { }
    }
}
