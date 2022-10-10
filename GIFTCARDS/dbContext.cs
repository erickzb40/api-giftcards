using GiftCards.entity;
using Microsoft.EntityFrameworkCore;

namespace GiftCards
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {
        }

        public DbSet<empresa> Empresa { get; set; }
        public DbSet<usuario> Usuario { get; set; }
        public DbSet<giftcardcab> cardcabs { get; set; }
        public DbSet<giftcarddet> carddets { get; set; }
        public DbSet<local> local { get; set; }
        public DbSet<estado> estado { get; set; }

    }
}

