namespace Hotel.Core.Models;

public class RoomReservationResult : RoomReservationBase
{
    public ReservationResultCode Code { get; set; }
    public int? RoomReservationId { get; set; }
}