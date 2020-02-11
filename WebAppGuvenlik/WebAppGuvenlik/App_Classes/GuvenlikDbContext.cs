using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppGuvenlik.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebAppGuvenlik
{
    public class GuvenlikDbContext: DbContext
    {
        public GuvenlikDbContext():base("GuvenlikDb")
        {
            Database.SetInitializer<GuvenlikDbContext>(null);
        }
        
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCategory> UserCategory { get; set; }
        public virtual DbSet<Visitor> Visitor { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        }
}