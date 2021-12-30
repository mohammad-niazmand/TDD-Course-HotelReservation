using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Hotel.Web.Pages;

public class RoomReservationsModel : PageModel
{
    private readonly IRoomReservationRepository _roomReservationRepository;

    public RoomReservationsModel(IRoomReservationRepository roomReservationRepository)
    {
        _roomReservationRepository = roomReservationRepository;
    }

    public IEnumerable<RoomReservationDto> RoomReservations { get; set; }

    public void OnGet()
    {
        RoomReservations = _roomReservationRepository.GetAll();
    }
}
