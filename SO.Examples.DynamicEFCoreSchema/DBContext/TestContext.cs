using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SO.Examples.DynamicEFCoreSchema.Contracts;

namespace SO.Examples.DynamicEFCoreSchema
{
    public partial class TestContext : DbContext
    {
        private ISchemaProvider SchemaProvider { get; }
        
        public TestContext()
        {
        }

        public TestContext(DbContextOptions<TestContext> options, ISchemaProvider schemaProvider)
            : base(options)
        {
            this.SchemaProvider = schemaProvider;
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Order> Order { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(local);Database=Test;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(this.SchemaProvider?.GetSchema() ?? "dbo");
            
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerName).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
