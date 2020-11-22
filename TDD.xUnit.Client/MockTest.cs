using FlightManager.Data;
using FlightManager.DataModels;
using FlightManager.Services;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace TDD.xUnit.net.Client
{
    public class MockTest
    {
        [Fact]
        public void CanMock()
        {
            //Arrange
            var data = new[] { new Flight { Id = "1", Origin = new City{ Name = "China"} }, new Flight { Id = "2", Origin = new City { Name = "India" } } }.AsQueryable();
            var mock = new Mock<IDbContext>();
            mock.Setup(x => x.Set<Flight>()).Returns(data);
            //Act
            var context = mock.Object;
            var flights = context.Set<Flight>();
            //Assert
            Assert.Equal(data, flights);
        }
    }
}
