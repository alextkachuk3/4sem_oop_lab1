using System.Data.Entity;

namespace Server
{
    class AppContext : DbContext
    {
        /// <summary>
        /// The class responsible for the relationship of the program with the database
        /// </summary>
        private static AppContext DataBase;

        /// <summary>
        /// Object for working with threads
        /// </summary>
        private static object syncRoot = new System.Object();

        /// <summary>
        /// Singleton database accessor
        /// </summary>
        /// <returns></returns>
        public static AppContext getDataBase()
        {
            if (DataBase == null)
            {
                lock (syncRoot)
                {
                    if (DataBase == null)
                        DataBase = new AppContext();
                }
            }
            return DataBase;
        }

        /// <summary>
        /// Users database table
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Notes database table
        /// </summary>
        public DbSet<Note> Notes { get; set; }

        /// <summary>
        /// Constructor for connecting this class with SQLite
        /// </summary>
        protected AppContext() : base("DefaultConnection") { }
    }
}
