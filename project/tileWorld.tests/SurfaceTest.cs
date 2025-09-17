using tileWorld.api.Controllers;
using tileWorld.domain.Interfaces;
using tileWorld.domain.ValueObjects;
using Moq;
using Microsoft.AspNetCore.Mvc;
namespace tileWorld.tests
{
    public class SurfaceTest
    {
        private readonly SurfaceController _controller;
        private readonly Mock<ISurfaceLayer> _mockSurface;
        public SurfaceTest()
        {
            _mockSurface = new Mock<ISurfaceLayer>();
            _controller = new SurfaceController(_mockSurface.Object);
        }
        [Fact]
        public async Task GetTile_ReturnsCorrectType()
        {
            _mockSurface.Setup(l => l.IsInBounds(1, 1)).Returns(true);
            _mockSurface.Setup(l => l.GetTileAsync(1, 1)).ReturnsAsync(SurfaceType.Mountain);

            var result = await _controller.GetTile(1, 1);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(SurfaceType.Mountain, ok.Value);

        }

        [Fact]
        public async Task GetTile_ReturnsBadRequest_WhenOutOfBounds()
        {
            _mockSurface.Setup(l => l.IsInBounds(100, 100)).Returns(false);

            var result = await _controller.GetTile(100, 100);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SetTile_ReturnsOk_WhenInBounds()
        {
            _mockSurface.Setup(l => l.IsInBounds(2, 2)).Returns(true);
            _mockSurface.Setup(l => l.SetTileAsync(2, 2, SurfaceType.Plain)).Returns(Task.CompletedTask);

            var result = await _controller.SetTile(2, 2, SurfaceType.Plain);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SetTile_ReturnsBadRequest_WhenOutOfBounds()
        {
            _mockSurface.Setup(l => l.IsInBounds(-1, -1)).Returns(false);

            var result = await _controller.SetTile(-1, -1, SurfaceType.Plain);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CanPlaceObject_ReturnsTrue_WhenAreaIsValid()
        {
            _mockSurface.Setup(l => l.IsInBounds(0, 0)).Returns(true);
            _mockSurface.Setup(l => l.IsInBounds(2, 2)).Returns(true);
            _mockSurface.Setup(l => l.CanPlaceObjectInArea(0, 0, 2, 2)).ReturnsAsync(true);

            var result = await _controller.CanPlaceObject(0, 0, 2, 2);
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)ok.Value!);
        }

        [Fact]
        public async Task CanPlaceObject_ReturnsBadRequest_WhenAreaOutOfBounds()
        {
            _mockSurface.Setup(l => l.IsInBounds(0, 0)).Returns(true);
            _mockSurface.Setup(l => l.IsInBounds(9999, 9999)).Returns(false);

            var result = await _controller.CanPlaceObject(0, 0, 9999, 9999);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
