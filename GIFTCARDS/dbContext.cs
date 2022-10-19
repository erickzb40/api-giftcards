using GiftCards.entity;
using GIFTCARDS.entity;
using Microsoft.EntityFrameworkCore;

namespace GiftCards
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions options) : base(options)
        {
        }

        private readonly string? _connectionString;
        public SampleContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<giftcardcab> cardcabs { get; set; }
        public DbSet<giftcarddet> carddets { get; set; }
        public DbSet<Local> local { get; set; }
        public DbSet<estado> estado { get; set; }
        public DbSet<Usuario_local> Usuario_local { get; set; }
        public DbSet<User_aprob_log> user_aprob_log { get; set; }

    }
}

