using System.Collections.Generic;
using Hotel.Core.Models;

namespace Hotel.Core.Interfaces;

public interface IRoomReservationRepository
{
    void Save(RoomReservationDto deskReservationDto);
    IEnumerable<RoomReservationDto> GetAll();
}