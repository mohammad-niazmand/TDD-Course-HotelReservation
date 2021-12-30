using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Hotel.Web.Pages;
public class RoomsModel : PageModel
{
    private readonly IRoomRepository _roomRepository;

    public RoomsModel(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public IEnumerable<RoomDto> Rooms { get; set; }

    public void OnGet()
    {
        Rooms = _roomRepository.GetAll();
    }
}