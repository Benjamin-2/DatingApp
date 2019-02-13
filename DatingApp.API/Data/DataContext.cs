using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) : base (option) {}

        // property name Values for when create table in sql & 
        // under DbSet<type> is models name set
        public DbSet<Values> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}