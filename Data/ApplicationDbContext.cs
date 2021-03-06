using GP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GP.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Estate>().Property(c => c.OnDate).HasDefaultValueSql("Getdate()");
            builder.Entity<Contract>().Property(c => c.OnDate).HasDefaultValueSql("Getdate()");
            builder.Entity<Comments>().Property(c => c.OnDate).HasDefaultValueSql("Getdate()");
            builder.Entity<Estate>().Property(c => c.is_active).HasDefaultValueSql("1");
            builder.Entity<Opinion>().Property(c => c.is_active).HasDefaultValueSql("0");

            builder.Entity<Estate>().Property(c => c.publish).HasDefaultValueSql("1");
            builder.Entity<Message>().HasOne(a => a.Sender).WithMany(x => x.Messages)
                .HasForeignKey(x => x.UserId);


            //builder.Entity<likedEstates>().HasKey(p=>new {p.Id ,p.IdUser});
        }

        public DbSet<Estate> TEstates { get; set; }
        public DbSet<Opinion> TOpinion { get; set; }
        public DbSet<Category> TCategory { get; set; }
        public DbSet<State> TState { get; set; }
        public DbSet<City> TCity { get; set; }
        public DbSet<Currency> TCurrency { get; set; }
        public DbSet<Service_Estate> TService_Estate { get; set; }
        public DbSet<Advertisement> TAdvertisement { get; set; }
        public DbSet<Models.Type> Ttype { get; set; }

        public DbSet<Services> TServices { get; set; }
        public DbSet<Comments> TComments { get; set; }

        public DbSet<Replaies> TReplaies { get; set; }


        public DbSet<PhotoEstate> TPhotoEstate { get; set; }
        public DbSet<Contact> TContacts { get; set; }
        public DbSet<InformationGen> TInformatiomGensT { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Contract> TContract { get; set; }

        public DbSet<likedEstates> TlikedEstates { get; set; }
        public DbSet<Notification> Notifications { get; set; }


    }
}
