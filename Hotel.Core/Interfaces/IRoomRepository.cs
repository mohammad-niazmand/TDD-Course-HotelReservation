using Hotel.Core.Models;
using System;
using System.Collections.Generic;

namespace Hotel.Core.Interfaces;

public interface IRoomRepository
{
    IEnumerable<RoomDto> GetAll();
    IEnumerable<RoomDto> GetAvailableRooms(DateTime date);
}
