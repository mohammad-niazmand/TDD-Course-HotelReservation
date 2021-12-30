using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.Domain;

public class RoomReservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
}