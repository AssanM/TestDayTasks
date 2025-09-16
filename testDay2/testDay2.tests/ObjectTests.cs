using Microsoft.AspNetCore.Mvc;
using Moq;
using testDay2.api.Controllers;
using testDay2.application.Services;
using testDay2.domain.Entities;

namespace testDay2.tests
{
    public class ObjectTests
    {
        private readonly Mock<ObjectService> _mockService;
        private readonly ObjectController _controller;
        public ObjectTests()
        {
            _mockService = new Mock<ObjectService>(MockBehavior.Strict , null!);
            _controller = new ObjectController(_mockService.Object);
        }
        [Fact]
        public async Task Add_ReturnsOk()
        {
            var obj = new MapObject("obj-1", 10, 10, 2, 2);
            _mockService.Setup(s => s.AddObjectAsync(obj)).Returns(Task.CompletedTask);

            var result = await _controller.Add(obj);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsObject_WhenFound()
        {
            var obj = new MapObject("obj-2", 5, 5, 1, 1);
            _mockService.Setup(s => s.GetByIdAsync("obj-2")).ReturnsAsync(obj);

            var result = await _controller.Get("obj-2");
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(obj, ok.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetByIdAsync("missing")).ReturnsAsync((MapObject?)null);

            var result = await _controller.Get("missing");
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ReturnsOk()
        {
            _mockService.Setup(s => s.RemoveAsync("obj-3")).Returns(Task.CompletedTask);

            var result = await _controller.Delete("obj-3");
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetByPoint_ReturnsObjects()
        {
            var list = new List<MapObject> { new("obj-4", 1, 1, 1, 1) };
            _mockService.Setup(s => s.GetByPointAsync(1, 1)).ReturnsAsync(list);

            var result = await _controller.GetByPoint(1, 1);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, ok.Value);
        }

        [Fact]
        public async Task GetByArea_ReturnsObjects()
        {
            var list = new List<MapObject> { new("obj-5", 2, 2, 2, 2) };
            _mockService.Setup(s => s.GetByAreaAsync(0, 0, 5, 5)).ReturnsAsync(list);

            var result = await _controller.GetByArea(0, 0, 5, 5);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, ok.Value);
        }

    }
}
