using System.Data.Entity;

namespace Server
{
    class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
