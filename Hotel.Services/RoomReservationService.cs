using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Hotel.Core.Services;


namespace Hotel.Services;

public class RoomReservationService : IRoomReservationService
{
    private readonly IRoomReservationRepository _roomReservationRepository;
    private readonly IRoomRepository _roomRepository;

    public RoomReservationService(IRoomReservationRepository roomReservationRepository, IRoomRepository roomRepository)
    {
        _roomReservationRepository = roomReservationRepository;
        _roomRepository = roomRepository;
    }

    //Notice : In real scenario you should use an object mapper like AutoMapper instead of mapping manually.
    private static T Create<T>(RoomReservationRequest request) where T : RoomReservationBase, new()
    {
        return new T
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Date = request.Date
        };
    }

    public RoomReservationResult Reserve(RoomReservationRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var result = Create<RoomReservationResult>(request);

        var availableRooms = _roomRepository.GetAvailableRooms(request.Date);
        if (availableRooms.FirstOrDefault() is { } availableRoom)
        {
            var roomReservation = Create<RoomReservationDto>(request);
            roomReservation.RoomId = availableRoom.Id;

            _roomReservationRepository.Save(roomReservation);

            result.RoomReservationId = roomReservation.Id;
            result.Code = ReservationResultCode.Success;
        }
        else
        {
            result.Code = ReservationResultCode.NoRoomAvailable;
        }

        return result;
    }
}
