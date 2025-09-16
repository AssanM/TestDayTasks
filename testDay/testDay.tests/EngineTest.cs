using Microsoft.AspNetCore.Mvc;
using testDay.api.Controllers;
using testDay.domain.Interfaces;
using Moq;
using testDay.domain.ValueObjects;
namespace testDay.tests
{
    public class EngineTest
    {
        private readonly EngineController _controller;
        private readonly Mock<IEngineLayer> _mockEngine;
        public EngineTest()
        {
            _mockEngine = new Mock<IEngineLayer>();
            _controller = new EngineController(_mockEngine.Object); 
        }
        [Fact]
        public async Task GetTile_ReturnsCorrectType()
        {
            _mockEngine.Setup(l => l.IsInBounds(1, 1)).Returns(true);
            _mockEngine.Setup(l => l.GetTileAsync(1, 1)).ReturnsAsync(EngineType.Mountain);

            var result = await _controller.GetTile(1, 1);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(EngineType.Mountain, ok.Value);

        }

        [Fact]
        public async Task GetTile_ReturnsBadRequest_WhenOutOfBounds()
        {
            _mockEngine.Setup(l => l.IsInBounds(100, 100)).Returns(false);

            var result = await _controller.GetTile(100, 100);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SetTile_ReturnsOk_WhenInBounds()
        {
            _mockEngine.Setup(l => l.IsInBounds(2, 2)).Returns(true);
            _mockEngine.Setup(l => l.SetTileAsync(2, 2, EngineType.Plain)).Returns(Task.CompletedTask);

            var result = await _controller.SetTile(2, 2, EngineType.Plain);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SetTile_ReturnsBadRequest_WhenOutOfBounds()
        {
            _mockEngine.Setup(l => l.IsInBounds(-1, -1)).Returns(false);

            var result = await _controller.SetTile(-1, -1, EngineType.Plain);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CanPlaceObject_ReturnsTrue_WhenAreaIsValid()
        {
            _mockEngine.Setup(l => l.IsInBounds(0, 0)).Returns(true);
            _mockEngine.Setup(l => l.IsInBounds(2, 2)).Returns(true);
            _mockEngine.Setup(l => l.CanPlaceObjectInArea(0, 0, 2, 2)).ReturnsAsync(true);

            var result = await _controller.CanPlaceObject(0, 0, 2, 2);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)ok.Value!);
        }

        [Fact]
        public async Task CanPlaceObject_ReturnsBadRequest_WhenAreaOutOfBounds()
        {
            _mockEngine.Setup(l => l.IsInBounds(0, 0)).Returns(true);
            _mockEngine.Setup(l => l.IsInBounds(9999, 9999)).Returns(false);

            var result = await _controller.CanPlaceObject(0, 0, 9999, 9999);
            Assert.IsType<BadRequestObjectResult>(result);
        }



    }
}
