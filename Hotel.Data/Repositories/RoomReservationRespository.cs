using System.Collections.Generic;
using System.Linq;
using Hotel.Core.Domain;
using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Hotel.Data;

namespace Hotel.DataLayer.Repositories;

public class RoomReservationRepository : IRoomReservationRepository
{
    private readonly HotelContext _context;

    public RoomReservationRepository(HotelContext context)
    {
        _context = context;
    }

    public IEnumerable<RoomReservationDto> GetAll()
    {
        //Notice : In real scenario you should use an object mapper like AutoMapper instead of mapping manually.
        return _context
            .RoomReservation
            .OrderBy(x => x.Date)
            .Select(a => new RoomReservationDto
            {
                Id = a.Id,
                RoomId = a.RoomId,
                Date = a.Date,
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName
            })
            .ToList();
    }

    public void Save(RoomReservationDto roomReservationDto)
    {
        var newRoomReservation = new RoomReservation
        {
            RoomId=roomReservationDto.RoomId,
            Date = roomReservationDto.Date,
            Email = roomReservationDto.Email,
            FirstName = roomReservationDto.FirstName,
            LastName = roomReservationDto.LastName
        };
        _context.RoomReservation.Add(newRoomReservation);
        _context.SaveChanges();
    }
}
