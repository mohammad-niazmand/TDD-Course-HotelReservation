using System;

namespace Hotel.Core.Models;

public class RoomReservationBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
}