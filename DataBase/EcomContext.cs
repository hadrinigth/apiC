using apiVcode.Models;
using Microsoft.EntityFrameworkCore;


public class ECommContext : DbContext
{
#nullable disable
  public ECommContext(DbContextOptions<ECommContext> options) : base(options)
  {
  }
  // *  setando tables

  public DbSet<Admin> Admins { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<BuyCart> BuyCarts { get; set; }
  public DbSet<BuyOrder> BuyOrders { get; set; }

}

