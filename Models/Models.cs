using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiVcode.Models;
[Table("Admin")]
public class Admin
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = default!;
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = default!;
    [Required]
    [StringLength(20)]
    public string Password { get; set; } = default!;
}

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = default!;
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = default!;
    [Required]
    [StringLength(20)]
    public string Password { get; set; } = default!;

}

[Table("Product")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default!;
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = default!;
    [Required]
    public float Price { get; set; } = default!;
    [Required]
    public int Stock { get; set; } = default!;
};


[Table("BuyCart")]
public class BuyCart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default;
    [Required]
    public int User_id { get; set; } = default!;
    [Required]
    public int Products_id { get; set; } = default!;
    [Required]
    public int Amount { get; set; } = 1;
    [Required]
    public int TotalPrice { get; set; } = default!;

}
[Table("BuyOrder")]
public class BuyOrder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = default;
    [Required]
    public int User_id { get; set; } = default!;
    [Required]
    public int Products_id { get; set; } = default!;
    [Required]
    public int Amount { get; set; } = 1;
    [Required]
    public int TotalPrice { get; set; } = default!;

}


