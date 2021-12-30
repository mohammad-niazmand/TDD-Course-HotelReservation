using Hotel.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Collections.Generic;
using Hotel.Core.Models;
using Xunit;

namespace Hotel.Web.Pages;

public class ReserveRoomModelTests
{
    private Mock<IRoomReservationService> _roomReservationServiceMock;
    private ReserveRoomModel _reserveRoomModel;
    private RoomReservationResult _roomReservationResult;

    public ReserveRoomModelTests()
    {
        _roomReservationServiceMock = new();

        _reserveRoomModel = new(_roomReservationServiceMock.Object)
        {
            RoomReservationRequest = new()
        };

        _roomReservationResult = new()
        {
            Code = ReservationResultCode.Success
        };

        _roomReservationServiceMock.Setup(x => x.Reserve(_reserveRoomModel.RoomReservationRequest))
            .Returns(_roomReservationResult);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void OnPost_ShouldCallReserveMethodOfRoomReservationServiceIfModelIsValid(
        int expectedReserveRoomCalls, bool isModelValid)
    {
        // Arrange
        if (!isModelValid)
        {
            _reserveRoomModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");
        }

        // Act
        _reserveRoomModel.OnPost();

        // Assert
        _roomReservationServiceMock.Verify(x => x.Reserve(_reserveRoomModel.RoomReservationRequest),
            Times.Exactly(expectedReserveRoomCalls));
    }

    [Fact]
    public void OnPost_ShouldAddModelErrorIfNoRoomIsAvailable()
    {
        // Arrange
        _roomReservationResult.Code = ReservationResultCode.NoRoomAvailable;

        // Act
        _reserveRoomModel.OnPost();

        // Assert
        var modelStateEntry = Assert.Contains("RoomReservationRequest.Date", _reserveRoomModel.ModelState);
        var modelError = Assert.Single(modelStateEntry.Errors);
        Assert.Equal("No Room available for selected date", modelError.ErrorMessage);
    }

    [Fact]
    public void OnPost_ShouldNotAddModelErrorIfRoomIsAvailable()
    {
        // Arrange
        _roomReservationResult.Code = ReservationResultCode.Success;

        // Act
        _reserveRoomModel.OnPost();

        // Assert
        Assert.DoesNotContain("RoomReservationRequest.Date", _reserveRoomModel.ModelState);
    }

    [Theory]
    [InlineData(typeof(PageResult), false, null)]
    [InlineData(typeof(PageResult), true, ReservationResultCode.NoRoomAvailable)]
    [InlineData(typeof(RedirectToPageResult), true, ReservationResultCode.Success)]
    public void OnPost_ShouldReturnExpectedActionResult(Type expectedActionResultType,
        bool isModelValid, ReservationResultCode? roomReservationResultCode)
    {
        // Arrange
        if (!isModelValid)
        {
            _reserveRoomModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");
        }

        if (roomReservationResultCode.HasValue)
        {
            _roomReservationResult.Code = roomReservationResultCode.Value;
        }

        // Act
        IActionResult actionResult = _reserveRoomModel.OnPost();

        // Assert
        Assert.IsType(expectedActionResultType, actionResult);
    }

    [Fact]
    public void OnPost_ShouldRedirectToReserveRoomConfirmationPage()
    {
        // Arrange
        _roomReservationResult.Code = ReservationResultCode.Success;
        _roomReservationResult.RoomReservationId = 7;
        _roomReservationResult.FirstName = "Mohammad";
        _roomReservationResult.Date = new DateTime(2021, 1, 28);

        // Act
        IActionResult actionResult = _reserveRoomModel.OnPost();

        // Assert
        var redirectToPageResult = Assert.IsType<RedirectToPageResult>(actionResult);
        Assert.Equal("ReserveRoomConfirmation", redirectToPageResult.PageName);

        IDictionary<string, object> routeValues = redirectToPageResult.RouteValues;
        Assert.Equal(3, routeValues.Count);

        var roomReservationId = Assert.Contains("RoomReservationId", routeValues);
        Assert.Equal(_roomReservationResult.RoomReservationId, roomReservationId);

        var firstName = Assert.Contains("FirstName", routeValues);
        Assert.Equal(_roomReservationResult.FirstName, firstName);

        var date = Assert.Contains("Date", routeValues);
        Assert.Equal(_roomReservationResult.Date, date);
    }
}