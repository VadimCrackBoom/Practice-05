using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using DeliveryClub.Entities;

namespace DeliveryClub;

public class DeliveryDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ShiftAssignment> ShiftAssignments { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }

    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Delivery.db")
                .LogTo(message => Debug.WriteLine(message));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка отношений и ограничений

        // User - ShiftAssignment (one-to-many)
        modelBuilder.Entity<ShiftAssignment>()
            .HasOne(sa => sa.User)
            .WithMany(u => u.ShiftAssignments)
            .HasForeignKey(sa => sa.UserId);

        // Shift - ShiftAssignment (one-to-many)
        modelBuilder.Entity<ShiftAssignment>()
            .HasOne(sa => sa.Shift)
            .WithMany(s => s.ShiftAssignments)
            .HasForeignKey(sa => sa.ShiftId);

        // Client - Order (one-to-many)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientId);

        // User (Manager) - Order (one-to-many)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Manager)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.ManagerId);

        // Order - OrderItem (one-to-many)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        // Product - OrderItem (one-to-many)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);

        // Уникальные индексы
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        // Настройка каскадного удаления (при необходимости)
        // По умолчанию EF Core устанавливает каскадное удаление для обязательных отношений
        // Можно явно указать поведение при удалении:
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .OnDelete(DeleteBehavior.Cascade);
    }
}