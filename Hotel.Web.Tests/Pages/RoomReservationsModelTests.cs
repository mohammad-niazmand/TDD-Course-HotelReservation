using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Moq;
using Xunit;

namespace Hotel.Web.Pages;

public class RoomReservationsModelTests
{
    [Fact]
    public void OnGet_ShouldGetAllRoomReservations()
    {
        // Arrange
        var roomReservations = new RoomReservationDto[]
        {
            new (),
            new (),
            new (),
        };

        var roomReservationRepositoryMock = new Mock<IRoomReservationRepository>();
        roomReservationRepositoryMock.Setup(x => x.GetAll())
            .Returns(roomReservations);

        var roomReservationsModel = new RoomReservationsModel(roomReservationRepositoryMock.Object);

        // Act
        roomReservationsModel.OnGet();

        // Assert
        Assert.Equal(roomReservations, roomReservationsModel.RoomReservations);
    }
}