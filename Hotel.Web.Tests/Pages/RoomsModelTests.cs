using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Moq;
using Xunit;

namespace Hotel.Web.Pages;

public class RoomsModelTests
{
    [Fact]
    public void OnGet_ShouldGetAllRooms()
    {
        // Arrange
        var rooms = new RoomDto[]
        {
            new (),
            new (),
            new (),
        };

        var roomRepositoryMock = new Mock<IRoomRepository>();
        roomRepositoryMock.Setup(x => x.GetAll())
            .Returns(rooms);

        var roomsModel = new RoomsModel(roomRepositoryMock.Object);

        // Act
        roomsModel.OnGet();

        // Assert
        Assert.Equal(rooms, roomsModel.Rooms);
    }
}