using System;
using Xunit;

namespace Hotel.Web.Pages;

public class ReserveRoomConfirmationModelTests
{
    [Fact]
    public void OnGet_ShouldStoreParameterValuesInProperties()
    {
        // Arrange
        const int roomReservationId = 7;
        const string firstName = "Mohammad";
        var date = new DateTime(2021, 1, 28);

        var reserveRoomConfirmationModel = new ReserveRoomConfirmationModel();

        // Act
        reserveRoomConfirmationModel.OnGet(roomReservationId, firstName, date);

        // Assert
        Assert.Equal(roomReservationId, reserveRoomConfirmationModel.RoomReservationId);
        Assert.Equal(firstName, reserveRoomConfirmationModel.FirstName);
        Assert.Equal(date, reserveRoomConfirmationModel.Date);
    }
}