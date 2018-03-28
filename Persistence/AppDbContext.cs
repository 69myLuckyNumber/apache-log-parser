using ApacheLogParser.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ApacheLogParser.Persistence
{
    public class AppDbContext : DbContext
    {   
        public DbSet<Host> Hosts { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<File> Files { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Host>()
                .HasKey(b => b.IPAddressBytes)
                .HasName("PK_IPAddressBytes");
            
            // Host 1 ----- * Request
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Requestor)
                .WithMany(h => h.Requests)
                .HasForeignKey(r => r.RequestorIPAddress);
            
            // Request 1 ---- 1 File
            modelBuilder.Entity<File>()
                .HasOne(f => f.Request)
                .WithOne(r => r.RequestedFile)
                .HasForeignKey<File>(f => f.RequestId);
        }
    }
}