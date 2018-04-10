using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Web;
using InrappAdmin.DomainModel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InrappAdmin.DataAccess
{
    public class InrappAdminIdentityDbContext : IdentityDbContext<AppUserAdmin>
    {
        public InrappAdminIdentityDbContext() : base("name=IdentityConnection")
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
            Configuration.ProxyCreationEnabled = false;
        }

        public InrappAdminIdentityDbContext(string connString) : base(connString)
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
            Configuration.ProxyCreationEnabled = false;
        }

        public static InrappAdminIdentityDbContext Create()
        {
            return new InrappAdminIdentityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            MapEntities(modelBuilder);
        }

        private void MapEntities(DbModelBuilder modelBuilder)
        {
            //AspNetUsers
            //modelBuilder.Entity<AppUserAdmin>().ToTable("AspNetUsers");
            modelBuilder.Entity<AppUserAdmin>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<AppUserAdmin>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<AppUserAdmin>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<AppUserAdmin>().Property(e => e.AndradAv).HasColumnName("andradav");
        }

        //public DbSet<AppUserAdmin> AppUserAdmin { get; set; }


    }
}