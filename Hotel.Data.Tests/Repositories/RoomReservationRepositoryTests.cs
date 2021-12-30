using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hotel.Core.Models;
using Hotel.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Hotel.DataLayer.Repositories;

public class RoomReservationRepositoryTests
{
    [Fact]
    public void Save_ShouldSaveTheRoomReservation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<HotelContext>()
            .UseInMemoryDatabase(databaseName: "ShouldSaveTheRoomReservation")
            .Options;

        var roomReservation = new RoomReservationDto
        {
            FirstName = "Mohammad",
            LastName = "Niazmand",
            Date = new DateTime(2021, 1, 25),
            Email = "MohammadNiazmand@gmail.com",
            RoomId = 1
        };

        // Act
        using (HotelContext context = new(options))
        {
            var repository = new RoomReservationRepository(context);
            repository.Save(roomReservation);
        }

        // Assert
        using (HotelContext context = new(options))
        {
            var reservations = context.RoomReservation.ToList();
            var storedRoomReservation = Assert.Single(reservations);

            Assert.Equal(roomReservation.FirstName, storedRoomReservation.FirstName);
            Assert.Equal(roomReservation.LastName, storedRoomReservation.LastName);
            Assert.Equal(roomReservation.Email, storedRoomReservation.Email);
            Assert.Equal(roomReservation.RoomId, storedRoomReservation.RoomId);
            Assert.Equal(roomReservation.Date, storedRoomReservation.Date);
        }
    }

    [Fact]
    public void GetAll_ShouldGetAllOrderedByDate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<HotelContext>()
            .UseInMemoryDatabase(databaseName: "ShouldReturnAllOrderedByDate")
            .Options;

        var storedList = new List<RoomReservationDto>
        {
            CreateRoomReservation(1,new (2021, 1, 27)),
            CreateRoomReservation(2,new (2021, 1, 25)),
            CreateRoomReservation(3,new (2021, 1, 29))
        };

        var expectedList = storedList.OrderBy(x => x.Date).ToList();

        using (var context = new HotelContext(options))
        {
            foreach (var roomReservation in storedList)
            {
                context.RoomReservation.Add(new()
                {
                    Id = roomReservation.Id,
                    FirstName = roomReservation.FirstName,
                    LastName = roomReservation.LastName,
                    Email = roomReservation.Email,
                    Date = roomReservation.Date,
                    RoomId = roomReservation.RoomId
                });
            }
            
            context.SaveChanges();
        }

        // Act
        List<RoomReservationDto> actualList;
        using (var context = new HotelContext(options))
        {
            var repository = new RoomReservationRepository(context);
            actualList = repository.GetAll().ToList();
        }

        // Assert
        Assert.Equal(expectedList, actualList, new RoomReservationEqualityComparer());
    }

    private class RoomReservationEqualityComparer : IEqualityComparer<RoomReservationDto>
    {
        public bool Equals([AllowNull] RoomReservationDto x, [AllowNull] RoomReservationDto y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] RoomReservationDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    private RoomReservationDto CreateRoomReservation(int id, DateTime dateTime)
    {
        return new RoomReservationDto
        {
            Id = id,
            FirstName = "Mohammad",
            LastName = "Niazmand",
            Date = dateTime,
            Email = "MohammadNiazmand@gmail.com",
            RoomId = 1
        };
    }
}