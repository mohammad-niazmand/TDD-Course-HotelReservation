using Hotel.Core.Models;

namespace Hotel.Core.Services;

public interface IRoomReservationService
{
    RoomReservationResult Reserve(RoomReservationRequest request);
}
