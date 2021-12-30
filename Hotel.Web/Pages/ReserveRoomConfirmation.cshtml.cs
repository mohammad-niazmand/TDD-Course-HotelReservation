using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Hotel.Web.Pages;

public class ReserveRoomConfirmationModel : PageModel
{
    public int RoomReservationId { get; set; }

    public string FirstName { get; set; }

    public DateTime Date { get; set; }

    public void OnGet(int roomReservationId, string firstName, DateTime date)
    {
        RoomReservationId = roomReservationId;
        FirstName = firstName;
        Date = date;
    }
}
