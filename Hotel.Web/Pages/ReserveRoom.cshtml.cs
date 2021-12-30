using Hotel.Core.Models;
using Hotel.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Pages;

public class ReserveRoomModel : PageModel
{
    private readonly IRoomReservationService _roomReservationService;

    public ReserveRoomModel(IRoomReservationService roomReservationService)
    {
        _roomReservationService = roomReservationService;
    }

    [BindProperty]
    public RoomReservationRequest RoomReservationRequest { get; set; }

    public IActionResult OnPost()
    {
        IActionResult actionResult = Page();

        if (!ModelState.IsValid) return actionResult;

        var result = _roomReservationService.Reserve(RoomReservationRequest);
        if (result.Code == ReservationResultCode.Success)
        {
            actionResult = RedirectToPage("ReserveRoomConfirmation", new
            {
                result.RoomReservationId,
                result.FirstName,
                result.Date
            });
        }
        else if (result.Code == ReservationResultCode.NoRoomAvailable)
        {
            ModelState.AddModelError("RoomReservationRequest.Date",
                "No Room available for selected date");
        }

        return actionResult;
    }
}