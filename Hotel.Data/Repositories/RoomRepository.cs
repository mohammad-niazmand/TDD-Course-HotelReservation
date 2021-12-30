using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Hotel.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotel.DataLayer.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly HotelContext _context;

    public RoomRepository(HotelContext context)
    {
        _context = context;
    }

    public IEnumerable<RoomDto> GetAll()
    {
        return _context
               .Room
               .Select(a => new RoomDto { Description = a.Name, Id = a.Id })
               .ToList();
    }

    public IEnumerable<RoomDto> GetAvailableRooms(DateTime date)
    {
        var reservedRoomIds = _context.RoomReservation.
          Where(x => x.Date == date)
          .Select(b => b.RoomId)
          .ToList();

        return _context.Room
          .Where(x => !reservedRoomIds.Contains(x.Id))
          .Select(a => new RoomDto { Description = a.Name, Id = a.Id })
          .ToList();
    }
}
