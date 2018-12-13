namespace RestSupplyDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RestSupplyDB.Models;

    public partial class RestSupplyDBModel : IdentityDbContext<UsersSet, RoleSet, string, UserLoginSet, UserRoleSet, UserClaimSet>
    {
        public RestSupplyDBModel()
            : base("name=RestSupplyDBModel")
        {
        }
        public static RestSupplyDBModel Create()
        {
            return new RestSupplyDBModel();
        }

        public virtual DbSet<IngredientsSet> IngredientsSet { get; set; }
        public virtual DbSet<KitchenIngredientsSet> KitchenIngredientsSet { get; set; }
        public virtual DbSet<KitchensSet> KitchensSet { get; set; }
        public virtual DbSet<MenuIngredientsSet> MenuIngredientsSet { get; set; }
        public virtual DbSet<MenuItemsSet> MenuItemsSet { get; set; }
        public virtual DbSet<IngredientListOrdersSet> IngredientListOrdersSet { get; set; }
        public virtual DbSet<IngredientOrdersSet> IngredientOrdersSet { get; set; }
        public virtual DbSet<CustomerOrdersSet> CustomerOrdersSet { get; set; }
        public virtual DbSet<CustomerDetailOrdersSet> CustomerDetailOrdersSet { get; set; }
        public virtual DbSet<SuppliersSet> SuppliersSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsersSet>().ToTable("dbo.Users");
            modelBuilder.Entity<RoleSet>().ToTable("dbo.Roles");
            modelBuilder.Entity<UserClaimSet>().ToTable("UserClaims");
            modelBuilder.Entity<UserLoginSet>().ToTable("UserLogins");
            modelBuilder.Entity<UserRoleSet>().ToTable("UserRoles");

            modelBuilder.Entity<UsersSet>().Property(r => r.Id);
            modelBuilder.Entity<UserClaimSet>().Property(r => r.Id);
            modelBuilder.Entity<RoleSet>().Property(r => r.Id);

            modelBuilder.Entity<IngredientsSet>()
                .HasMany(e => e.KitchenIngredientsSet)
                .WithRequired(e => e.IngredientsSet)
                .HasForeignKey(e => e.IngredientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IngredientsSet>()
                .HasMany(e => e.MenuIngredientsSet)
                .WithOptional(e => e.IngredientsSet)
                .HasForeignKey(e => e.IngredientId).WillCascadeOnDelete(false);

            modelBuilder.Entity<IngredientsSet>()
                .HasMany(e => e.IngredientListOrdersSet)
                .WithRequired(e => e.IngredientsSet)
                .HasForeignKey(e => e.IngredientId).WillCascadeOnDelete(false);

            modelBuilder.Entity<KitchensSet>()
                .HasMany(e => e.KitchenIngredientsSet)
                .WithRequired(e => e.KitchensSet)
                .HasForeignKey(e => e.KitchenId).WillCascadeOnDelete(false);

            modelBuilder.Entity<KitchensSet>()
                .HasMany(e => e.CustomerOrdersSet)
                .WithRequired(e => e.KitchensSet)
                .HasForeignKey(e => e.KitchenId);

            modelBuilder.Entity<MenuItemsSet>()
                .HasMany(e => e.MenuIngredientsSet)
                .WithOptional(e => e.MenuItemsSet)
                .HasForeignKey(e => e.MenuItemId).WillCascadeOnDelete(false);

            modelBuilder.Entity<MenuItemsSet>()
                .HasMany(e => e.CustomerDetailOrdersSet)
                .WithRequired(e => e.MenuItemsSet)
                .HasForeignKey(e => e.MenuItemId).WillCascadeOnDelete(false);
            
            modelBuilder.Entity<IngredientOrdersSet>()
                .HasMany(e => e.IngredientListOrdersSet)
                .WithRequired(e => e.OrdersSet)
                .HasForeignKey(e => e.OrderId).WillCascadeOnDelete(false);

            modelBuilder.Entity<SuppliersSet>()
                .HasMany(e => e.IngredientsSet)
                .WithRequired(e => e.SuppliersSet)
                .HasForeignKey(e => e.SupplierId).WillCascadeOnDelete(false);

            modelBuilder.Entity<SuppliersSet>()
                .HasMany(e => e.IngredientOrdersSet)
                .WithRequired(e => e.SuppliersSet)
                .HasForeignKey(e => e.SupplierId).WillCascadeOnDelete(false);
        }
    }
}
