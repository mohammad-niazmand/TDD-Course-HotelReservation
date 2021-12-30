using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.Core.Domain;
using Hotel.Core.Models;
using Hotel.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Hotel.DataLayer.Repositories;

public class RoomRepositoryTests
{
    [Fact]
    public void GetAvailableRooms_ShouldReturnTheAvailableRooms()
    {
        // Arrange
        var date = new DateTime(2021, 1, 25);

        var options = new DbContextOptionsBuilder<HotelContext>()
            .UseInMemoryDatabase(databaseName: "ShouldReturnTheAvailableRooms")
            .Options;

        using (var context = new HotelContext(options))
        {
            context.Room.Add(new() { Id = 1 ,Name="RoomA"});
            context.Room.Add(new() { Id = 2 ,Name="RoomB"});
            context.Room.Add(new() { Id = 3 ,Name="RoomC"});

            context.RoomReservation.Add(new() { RoomId = 1,FirstName="Sara"  ,LastName="Boman" ,Email="Sara@Gmail.Com" , Date = date });
            context.RoomReservation.Add(new() { RoomId = 2,FirstName="Jesica",LastName="Toie",Email="Jesica@Gmail.Com" , Date = date.AddDays(1) });

            context.SaveChanges();
        }

        using (var context = new HotelContext(options))
        {
            var repository = new RoomRepository(context);

            // Act
            var rooms = repository.GetAvailableRooms(date);

            // Assert
            Assert.Equal(2, rooms.Count());
            Assert.Contains(rooms, d => d.Id == 2);
            Assert.Contains(rooms, d => d.Id == 3);
            Assert.DoesNotContain(rooms, d => d.Id == 1);
        }
    }

    [Fact]
    public void GetAll_ShouldReturnAllRooms()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<HotelContext>()
            .UseInMemoryDatabase(databaseName: "ShouldGetAll")
            .Options;

        var storedList = new List<Room>
        {
            new (),
            new (),
            new ()
        };

        using (var context = new HotelContext(options))
        {
            foreach (var room in storedList)
            {
                context.Add(room);
                context.SaveChanges();
            }
        }

        // Act
        List<RoomDto> actualList;
        using (var context = new HotelContext(options))
        {
            var repository = new RoomRepository(context);
            actualList = repository.GetAll().ToList();
        }

        // Assert
        Assert.Equal(storedList.Count(), actualList.Count());
    }
}