using FlightManager.Services.ServiceModels;
using FlightManager.Services;
using Moq;

namespace TDD.xUnit.net.Client
{
    public class TestServices
    {
        //[Fact]
        public void ProperlyCreatesReservation() //UnderDevelopment
        {
            //Arrange
            var data = new ReservationServiceModel { Id = "1", FirstName = "Pesho" };
            var reservationServiceMock = new Mock<ReservationService>();
            reservationServiceMock.Setup(r => r.Create(data));
           
            //Act
            var service = reservationServiceMock.Object;
            //var reservations = service.Create(data);

            //Assert
            reservationServiceMock.Verify(r => r.Exists(data.Id));
            //Assert.Equal(data, service.GetById(data.Id));
        }
    }
}
