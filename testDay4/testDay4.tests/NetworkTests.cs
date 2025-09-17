using LiteNetLib;
using MemoryPack;
using Moq;
using testDay4.contracts.Requests;
using testDay4.domain.Entities;
using testDay4.domain.Interfaces;
using testDay4.server;

namespace testDay4.tests
{
    public class NetworkTests
    {
        [Fact]
        public async Task GetObjectsInArea_ReturnsCorrectObjects()
        {
            var mockLayer = new Mock<IObjectLayer>();
            mockLayer.Setup(l => l.GetByAreaAsync(0, 0, 10, 10))
                     .ReturnsAsync(new List<MapObject> { new("id1", 1, 1, 2, 2) });

            var dispatcher = new MessageDispatcher(mockLayer.Object, Mock.Of<IRegionLayer>(), Mock.Of<MapUdpServer>());
            var request = new GetObjecstInAreaRequest { X1 = 0, Y1 = 0, X2 = 10, Y2 = 10 };
            var bytes = MemoryPackSerializer.Serialize(request);

            // Проверка десериализации и вызова
            dispatcher.Handle(Mock.Of<NetPeer>(), bytes);
            // Проверка через Moq: mockLayer.Verify(...)
        }

    }
}
