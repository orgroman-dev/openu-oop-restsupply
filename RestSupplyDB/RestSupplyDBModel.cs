namespace RestSupplyDB
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RestSupplyDB.Models.AppUser;
    using RestSupplyDB.Models.Customer;
    using RestSupplyDB.Models.Ingredient;
    using RestSupplyDB.Models.Kitchen;
    using RestSupplyDB.Models.Menu;
    using RestSupplyDB.Models.Supplier;

    public partial class RestSupplyDBModel : IdentityDbContext<AppUser, AppRole, string, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public RestSupplyDBModel()
            : base("name=RestSupplyDBModel")
        {
        }
        public static RestSupplyDBModel Create()
        {
            return new RestSupplyDBModel();
        }

        public virtual DbSet<Ingredients> IngredientsSet { get; set; }
        public virtual DbSet<KitchenIngredients> KitchenIngredientsSet { get; set; }
        public virtual DbSet<Kitchens> KitchensSet { get; set; }
        public virtual DbSet<MenuIngredients> MenuIngredientsSet { get; set; }
        public virtual DbSet<MenuItems> MenuItemsSet { get; set; }
        public virtual DbSet<IngredientListOrders> IngredientListOrdersSet { get; set; }
        public virtual DbSet<IngredientOrders> IngredientOrdersSet { get; set; }
        public virtual DbSet<CustomerOrders> CustomerOrdersSet { get; set; }
        public virtual DbSet<CustomerDetailOrders> CustomerDetailOrdersSet { get; set; }
        public virtual DbSet<Suppliers> SuppliersSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().ToTable("dbo.Users");
            modelBuilder.Entity<AppRole>().ToTable("dbo.Roles");
            modelBuilder.Entity<AppUserClaim>().ToTable("dbo.UserClaims");
            modelBuilder.Entity<AppUserLogin>().ToTable("dbo.UserLogins");
            modelBuilder.Entity<AppUserRole>().ToTable("dbo.UserRoles");

            modelBuilder.Entity<AppUser>().Property(r => r.Id);
            modelBuilder.Entity<AppUserClaim>().Property(r => r.Id);
            modelBuilder.Entity<AppRole>().Property(r => r.Id);

            modelBuilder.Entity<Ingredients>()
                .HasMany(e => e.KitchenIngredients)
                .WithRequired(e => e.IngredientsSet)
                .HasForeignKey(e => e.IngredientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ingredients>()
                .HasMany(e => e.MenuIngredients)
                .WithOptional(e => e.IngredientsSet)
                .HasForeignKey(e => e.IngredientId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Ingredients>()
                .HasMany(e => e.IngredientListOrders)
                .WithRequired(e => e.IngredientsSet)
                .HasForeignKey(e => e.IngredientId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Kitchens>()
                .HasMany(e => e.KitchenIngredientsSet)
                .WithRequired(e => e.KitchensSet)
                .HasForeignKey(e => e.KitchenId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Kitchens>()
                .HasMany(e => e.CustomerOrdersSet)
                .WithRequired(e => e.Kitchens)
                .HasForeignKey(e => e.KitchenId);

            modelBuilder.Entity<MenuItems>()
                .HasMany(e => e.MenuIngredientsSet)
                .WithOptional(e => e.MenuItemsSet)
                .HasForeignKey(e => e.MenuItemId).WillCascadeOnDelete(false);

            modelBuilder.Entity<MenuItems>()
                .HasMany(e => e.CustomerDetailOrdersSet)
                .WithRequired(e => e.MenuItems)
                .HasForeignKey(e => e.MenuItemId).WillCascadeOnDelete(false);
            
            modelBuilder.Entity<IngredientOrders>()
                .HasMany(e => e.IngredientListOrdersSet)
                .WithRequired(e => e.OrdersSet)
                .HasForeignKey(e => e.OrderId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Suppliers>()
                .HasMany(e => e.IngredientsSet)
                .WithMany(e => e.Suppliers)
                .Map(si => 
                {
                    si.MapLeftKey("SupplierRefId");
                    si.MapRightKey("IngredientRefId");
                    si.ToTable("SuppliersIngredients");
                });
                

            modelBuilder.Entity<Suppliers>()
                .HasMany(e => e.IngredientOrdersSet)
                .WithRequired(e => e.SuppliersSet)
                .HasForeignKey(e => e.SupplierId).WillCascadeOnDelete(false);
        }
    }
}
