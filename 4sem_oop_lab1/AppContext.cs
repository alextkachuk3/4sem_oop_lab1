using System.Data.Entity;

namespace _4sem_oop_lab1
{
    class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public AppContext() : base("DefaultConnection") { }
    }
}
