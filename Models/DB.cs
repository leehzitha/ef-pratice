using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
public  class Config
{
    public static async Task<ExampleDbContext> InitiateAsync()
    {
        var connBuilder = new SqlConnectionStringBuilder
        {
            DataSource = "localhost",
            InitialCatalog = "StoreDB",
            IntegratedSecurity = true,
            TrustServerCertificate = true
        };
        var stringConnection = connBuilder.ToString();

        var optsBuilder = new DbContextOptionsBuilder();
        optsBuilder.UseSqlServer(stringConnection);
        var options = optsBuilder.Options;

        var db = new ExampleDbContext(options);
        await db.Database.EnsureCreatedAsync();
        return db;
    }

}

public class ExampleDbContext(DbContextOptions opts) : DbContext(opts)
{
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<ProductItem> Items => Set<ProductItem>();
    public DbSet<UserData> Users => Set<UserData>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<UserData>();
        model.Entity<ProductItem>();

        model.Entity<Sale>()
            .HasOne(s => s.User)
            .WithMany(u => u.Sales)
            .HasForeignKey(s => s.UserDataID)
            .OnDelete(DeleteBehavior.Cascade);

        model.Entity<Sale>()
            .HasOne(s => s.Item)
            .WithMany(p => p.Sales)
            .HasForeignKey(s => s.ProductID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


public class Sale
{
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int UserDataID { get; set; }
    public DateTime BuyDate { get; set; }
    public UserData User { get; set; }
    public ProductItem Item { get; set; }
}

public class ProductItem
{
    public int ID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ICollection<Sale> Sales { get; set; } = [];
}

public class UserData
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string Pass { get; set; }
    public bool IsAdm { get; set; }
    public ICollection<Sale> Sales { get; set; } = [];
}