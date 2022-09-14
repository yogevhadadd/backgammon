using Microsoft.EntityFrameworkCore;
using TalkBackAccessControll.Date.Models;

namespace TalkBackAccessControll.Date.DB
{
    public partial class TalkBackDbContext : DbContext
    {
        public TalkBackDbContext()
        {
        }

        public TalkBackDbContext(DbContextOptions<TalkBackDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("TalkBckConnectionString");
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-C23F11M;Initial Catalog=Project;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
