using Microsoft.AspNetCore.Mvc;
using Moq;
using testDay.api.Controllers;
using testDay.application.Models;
using testDay.application.Services;
using testDay.domain.Interfaces;
namespace testDay.tests
{
    public class PathTest
    {
        private readonly PathController _controller;
        private readonly Mock<PathFindingQueue> _mockQueue;
        private readonly Mock<IEngineLayer> _mockEngine;
        public PathTest()
        {
            _mockEngine = new Mock<IEngineLayer>();
            _mockQueue = new Mock<PathFindingQueue>();
            _controller = new PathController(_mockQueue.Object,_mockEngine.Object); 
        }
        [Fact]
        public void StartPathfinding_ReturnsTaskId()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            _mockQueue.Setup(q => q.Enqueue(It.IsAny<Func<IProgress<float>, CancellationToken, Task<List<(int, int)>>>>()))
                      .Returns(expectedId);

            // Act
            var result = _controller.StartPathFinding(0, 0, 5, 5);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedId, ok.Value);
        }

        [Fact]
        public void GetStatus_ReturnsStatus_WhenFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var status = new PathFindingTaskStatus
            {
                TaskId = taskId,
                Progress = 0.5f,
                IsCompleted = false,
                Result = new List<(int, int)>()
            };

            _mockQueue.Setup(q => q.GetStatus(taskId)).Returns(status);

            // Act
            var result = _controller.GetStatus(taskId);
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(status, ok.Value);
        }

        [Fact]
        public void GetStatus_ReturnsNotFound_WhenMissing()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mockQueue.Setup(q => q.GetStatus(taskId)).Returns((PathFindingTaskStatus)null);

            // Act
            var result = _controller.GetStatus(taskId);
            Assert.IsType<NotFoundResult>(result.Result);
        }

    }
}
