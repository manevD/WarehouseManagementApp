using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Models;

namespace WarehouseManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =========================================================
        // DbSets
        // =========================================================

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<WarehouseLocation> WarehouseLocations { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockMovement> StockMovements { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        // =========================================================
        // Model Configuration
        // =========================================================

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // =====================================================
            // CATEGORY
            // =====================================================

            builder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);
            });


            // =====================================================
            // PRODUCT
            // =====================================================

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.SKU)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.SKU)
                    .IsUnique();

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.Barcode)
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Barcode)
                    .IsUnique()
                    .HasFilter("[Barcode] IS NOT NULL");

                entity.Property(x => x.PurchasePrice)
                    .HasPrecision(18, 2);

                entity.Property(x => x.SalePrice)
                    .HasPrecision(18, 2);

                entity.Property(x => x.MinimumStock)
                    .HasPrecision(18, 3);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");


                // Product → Category
                entity.HasOne(x => x.Category)
      .WithMany(x => x.Products)
      .HasForeignKey(x => x.CategoryId)
      .OnDelete(DeleteBehavior.Restrict);
            });


            // =====================================================
            // WAREHOUSE
            // =====================================================

            builder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Address)
                    .HasMaxLength(200);

                entity.Property(x => x.City)
                    .HasMaxLength(100);

                entity.Property(x => x.PostalCode)
                    .HasMaxLength(20);

                entity.Property(x => x.Country)
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });


            // =====================================================
            // WAREHOUSE LOCATION
            // =====================================================

            builder.Entity<WarehouseLocation>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Name)
                    .HasMaxLength(200);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);


                // Warehouse → Locations
                entity.HasOne(x => x.Warehouse)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Cascade);


                // Same location code should be unique
                // inside one warehouse.
                entity.HasIndex(x => new
                {
                    x.WarehouseId,
                    x.Code
                })
                .IsUnique();
            });


            // =====================================================
            // STOCK
            // =====================================================

            builder.Entity<Stock>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Quantity)
                    .HasPrecision(18, 3);

                entity.Property(x => x.ReservedQuantity)
                    .HasPrecision(18, 3);

                entity.Property(x => x.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");


                // Stock → Product
                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);


                // Stock → WarehouseLocation
                entity.HasOne(x => x.WarehouseLocation)
                    .WithMany(x => x.Stocks)
                    .HasForeignKey(x => x.WarehouseLocationId)
                    .OnDelete(DeleteBehavior.Restrict);


                // One product can exist only once
                // in one warehouse location.
                entity.HasIndex(x => new
                {
                    x.ProductId,
                    x.WarehouseLocationId
                })
                .IsUnique();
            });

            builder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(x => x.Id);


                entity.Property(x => x.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(50);


                entity.HasIndex(x => x.OrderNumber)
                    .IsUnique();


                entity.Property(x => x.ReferenceNumber)
                    .HasMaxLength(100);


                entity.Property(x => x.Note)
                    .HasMaxLength(500);


                entity.Property(x => x.Total)
                    .HasPrecision(18, 2);


                entity.Property(x => x.Status)
                    .IsRequired();


                entity.Property(x => x.OrderDate)
                    .HasDefaultValueSql("GETUTCDATE()");


                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");


                // PurchaseOrder → Supplier
                entity.HasOne(x => x.Supplier)
                    .WithMany()
                    .HasForeignKey(x => x.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);


                // PurchaseOrder → Items
                entity.HasMany(x => x.Items)
                    .WithOne(x => x.PurchaseOrder)
                    .HasForeignKey(x => x.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =========================================================
            // PURCHASE ORDER ITEM
            // =========================================================

            builder.Entity<PurchaseOrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);


                entity.Property(x => x.Quantity)
                    .HasPrecision(18, 3);


                entity.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2);


                entity.Property(x => x.DeliveredQuantity)
                    .HasPrecision(18, 3);


                // PurchaseOrderItem → Product
                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);


                // PurchaseOrderItem → PurchaseOrder
                entity.HasOne(x => x.PurchaseOrder)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // =====================================================
            // STOCK MOVEMENT
            // =====================================================

            builder.Entity<StockMovement>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.PurchaseOrder)
                        .WithMany()
                        .HasForeignKey(x => x.PurchaseOrderId)
                        .OnDelete(DeleteBehavior.SetNull);
                         entity.HasOne(x => x.Supplier)
                        .WithMany()
                        .HasForeignKey(x => x.SupplierId)
                        .OnDelete(DeleteBehavior.SetNull);
                entity.Property(x => x.Quantity)
                    .HasPrecision(18, 3);

                entity.Property(x => x.Note)
                    .HasMaxLength(500);

                entity.Property(x => x.UserId)
                    .HasMaxLength(450);

                entity.Property(x => x.Type)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");


                // -------------------------------------------------
                // StockMovement → Product
                // -------------------------------------------------

                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);


                // -------------------------------------------------
                // StockMovement → From Location
                // -------------------------------------------------

                entity.HasOne(x => x.FromWarehouseLocation)
                    .WithMany()
                    .HasForeignKey(x => x.FromWarehouseLocationId)
                    .OnDelete(DeleteBehavior.Restrict);


                // -------------------------------------------------
                // StockMovement → To Location
                // -------------------------------------------------

                entity.HasOne(x => x.ToWarehouseLocation)
                    .WithMany()
                    .HasForeignKey(x => x.ToWarehouseLocationId)
                    .OnDelete(DeleteBehavior.Restrict);


                // -------------------------------------------------
                // StockMovement → ApplicationUser
                // -------------------------------------------------

                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // =========================================================
            // INVENTORY
            // =========================================================

            builder.Entity<Inventory>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Status)
                    .IsRequired();

                entity.Property(x => x.Note)
                    .HasMaxLength(500);

                entity.Property(x => x.UserId)
                    .HasMaxLength(450);

                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");


                // Inventory → Warehouse
                entity.HasOne(x => x.Warehouse)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict);


                // Inventory → Items
                entity.HasMany(x => x.Items)
                    .WithOne(x => x.Inventory)
                    .HasForeignKey(x => x.InventoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =========================================================
            // INVENTORY ITEM
            // =========================================================

            builder.Entity<InventoryItem>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.SystemQuantity)
                    .HasPrecision(18, 3);

                entity.Property(x => x.CountedQuantity)
                    .HasPrecision(18, 3);

                entity.Property(x => x.Difference)
                    .HasPrecision(18, 3);


                // InventoryItem → Product
                entity.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);


                // InventoryItem → WarehouseLocation
                entity.HasOne(x => x.WarehouseLocation)
                    .WithMany()
                    .HasForeignKey(x => x.WarehouseLocationId)
                    .OnDelete(DeleteBehavior.Restrict);


                // InventoryItem → Inventory
                entity.HasOne(x => x.Inventory)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.InventoryId)
                    .OnDelete(DeleteBehavior.Cascade);


                // Same product/location only once in one inventory
                entity.HasIndex(x => new
                {
                    x.InventoryId,
                    x.ProductId,
                    x.WarehouseLocationId
                })
                .IsUnique();
            });
        }
    }
}