using apiC.Models;
using Microsoft.EntityFrameworkCore;

namespace apiC.DataBase
{
  public class EcomContext : DbContext
  {
#nullable disable
    public EcomContext(DbContextOptions<EcomContext> options) : base(options)
    {
    }
    // *  setando tables

    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<BuyCart> BuyCarts { get; set; }
    public DbSet<BuyOrder> BuyOrders { get; set; }

  }
}