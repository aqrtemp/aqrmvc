using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AQRWebApp.Model.Entity;

namespace AQRWebApp.Model
{
    public class AQRWebAppDbContext: DbContext
    {
        public AQRWebAppDbContext()
            : base("AQRConnection")
        {
            Database.SetInitializer(new MyInitializer());
        }

        public AQRWebAppDbContext(string connectionString)
            : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppInfo> AppInfos { set; get; }
        public DbSet<Client> Clients { set; get; }
        public DbSet<Colour> Colours { set; get; }
        public DbSet<Delivery> Deliveries { set; get; }
        public DbSet<FileInfo> FileInfos { set; get; }
        public DbSet<Partner> Partners { set; get; }
        public DbSet<PartnerStore> PartnerStores { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<Receive> Receives { set; get; }
        public DbSet<ShipmentId> ShipmentIds { set; get; }
        public DbSet<ShipmentRegister> ShipmentRegisters { set; get; }
        public DbSet<Size> Sizes { set; get; }
        public DbSet<SizeGroup> SizeGroups { set; get; }
        public DbSet<Style> Styles { set; get; }
        public DbSet<ClientPartner> ClientPartners { set; get; }
        public DbSet<Person> Persons { set; get; }
        public DbSet<PersonAccount> PersonAccounts { set; get; }
        public DbSet<ChucDanh> ChucDanhs { set; get; }
        public DbSet<Department> Departments { set; get; }
    }
    
}
