using Hotel.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data;

public class HotelContext : DbContext
{
    public HotelContext(DbContextOptions<HotelContext> options) : base(options)
    {
    }

    public DbSet<RoomReservation> RoomReservation { get; set; }

    public DbSet<Room> Room { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Room 101" },
            new Room { Id = 2, Name = "Room 102" }
        );
    }
}