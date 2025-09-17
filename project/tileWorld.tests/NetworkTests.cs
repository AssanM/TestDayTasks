using LiteNetLib;
using MemoryPack;
using Moq;
using tileWorld.domain.Entities;
using tileWorld.domain.Interfaces;
using tileWorld.infrastructure.Network.Contracts;
using tileWorld.infrastructure.Network.Dispatcher;
using tileWorld.infrastructure.Network.Server;

namespace tileWorld.tests;

public class NetworkTests
{
    [Fact]
    public async Task GetObjectsInArea_ReturnsCorrectObjects()
    {
        var mockLayer = new Mock<IObjectLayer>();
        mockLayer.Setup(l => l.GetByAreaAsync(0, 0, 10, 10))
                 .ReturnsAsync(new List<MapObject> { new("id1", 1, 1, 2, 2) });

        var dispatcher = new MessageDispatcher(mockLayer.Object, Mock.Of<IRegionLayer>(), Mock.Of<MapUdpServer>());
        var request = new GetObjectsInAreaRequest { X1 = 0, Y1 = 0, X2 = 10, Y2 = 10 };
        var bytes = MemoryPackSerializer.Serialize(request);

        dispatcher.Handle(Mock.Of<NetPeer>(), bytes);
    }
}
