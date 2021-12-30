using Hotel.Core.Interfaces;
using Hotel.Core.Models;
using Hotel.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hotel.Core.Services;

public class RoomReservationServiceTests
{
    private readonly List<RoomDto> _availableRooms;
    private readonly RoomReservationRequest _request;
    private readonly Mock<IRoomReservationRepository> _roomReservationRepositoryMock;
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly IRoomReservationService _roomReservationService;

    public RoomReservationServiceTests()
    {
        _request = new()
        {
            FirstName = "Mohammad",
            LastName = "Niazmand",
            Email = "MohammadNiazmand@gmail.com",
            Date = new DateTime(2021, 1, 28)
        };
        _availableRooms = new() { new RoomDto { Id = 7 } };
        _roomReservationRepositoryMock = new();
        _roomRepositoryMock = new();
        _roomRepositoryMock.Setup(x => x.GetAvailableRooms(_request.Date))
          .Returns(_availableRooms);

        _roomReservationService = new RoomReservationService(
          _roomReservationRepositoryMock.Object, _roomRepositoryMock.Object);
    }

    [Fact]
    public void Reserve_ShouldReturnRoomReservationResultWithRequestValues()
    {
        // Act
        RoomReservationResult result = _roomReservationService.Reserve(_request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_request.FirstName, result.FirstName);
        Assert.Equal(_request.LastName, result.LastName);
        Assert.Equal(_request.Email, result.Email);
        Assert.Equal(_request.Date, result.Date);
    }

    [Fact]
    public void Reserve_ShouldThrowExceptionIfRequestIsNull()
    {
        //Act
        var exception = Assert.Throws<ArgumentNullException>(() => _roomReservationService.Reserve(null));

        //Assert
        Assert.Equal("request", exception.ParamName);
    }

    [Fact]
    public void Reserve_ShouldSaveRoomReservation()
    {
        //Arrange 
        RoomReservationDto savedRoomReservationDto = null;
        _roomReservationRepositoryMock.Setup(x => x.Save(It.IsAny<RoomReservationDto>()))
          .Callback<RoomReservationDto>(roomReservation =>
          {
              savedRoomReservationDto = roomReservation;
          });

        //Act
        _roomReservationService.Reserve(_request);

        //Assert
        _roomReservationRepositoryMock.Verify(x => x.Save(It.IsAny<RoomReservationDto>()), Times.Once);

        Assert.NotNull(savedRoomReservationDto);
        Assert.Equal(_request.FirstName, savedRoomReservationDto.FirstName);
        Assert.Equal(_request.LastName, savedRoomReservationDto.LastName);
        Assert.Equal(_request.Email, savedRoomReservationDto.Email);
        Assert.Equal(_request.Date, savedRoomReservationDto.Date);
        Assert.Equal(_availableRooms.First().Id, savedRoomReservationDto.RoomId);
    }

    [Fact]
    public void Reserve_ShouldNotSaveRoomReservationIfNoRoomIsAvailable()
    {
        //Arrange 
        _availableRooms.Clear();

        //Act
        _roomReservationService.Reserve(_request);

        //Assert
        _roomReservationRepositoryMock.Verify(x => x.Save(It.IsAny<RoomReservationDto>()), Times.Never);
    }

    [Theory]
    [InlineData(ReservationResultCode.Success, true)]
    [InlineData(ReservationResultCode.NoRoomAvailable, false)]
    public void Reserve_ShouldReturnExpectedResultCode(ReservationResultCode expectedResultCode, bool isRoomAvailable)
    {
        //Arrange
        if (!isRoomAvailable) _availableRooms.Clear();

        //Act
        var result = _roomReservationService.Reserve(_request);

        //Assert
        Assert.Equal(expectedResultCode, result.Code);
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(null, false)]
    public void Reserve_ShouldReturnExpectedRoomReservationId(int? expectedRoomReservationId, bool isRoomAvailable)
    {
        //Arrange
        if (!isRoomAvailable)
            _availableRooms.Clear();
        else
            _roomReservationRepositoryMock.Setup(x => x.Save(It.IsAny<RoomReservationDto>()))
              .Callback<RoomReservationDto>(roomReservation =>
              {
                  roomReservation.Id = expectedRoomReservationId.Value;
              });

        //Act
        var result = _roomReservationService.Reserve(_request);

        //Assert
        Assert.Equal(expectedRoomReservationId, result.RoomReservationId);
    }
}
