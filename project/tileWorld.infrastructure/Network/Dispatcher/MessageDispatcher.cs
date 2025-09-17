using LiteNetLib;
using MemoryPack;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using tileWorld.domain.Entities;
using tileWorld.domain.Interfaces;
using tileWorld.infrastructure.Network.Contracts;
using tileWorld.infrastructure.Network.Server;

namespace tileWorld.infrastructure.Network.Dispatcher;

public class MessageDispatcher
{
    private readonly IObjectLayer _objectLayer;
    private readonly IRegionLayer _regionLayer;
    private readonly IServiceProvider _provider;

    public MessageDispatcher(IObjectLayer objectLayer, IRegionLayer regionLayer, IServiceProvider provider)
    {
        _objectLayer = objectLayer;
        _regionLayer = regionLayer;
        _provider = provider;
        _objectLayer.ObjectAdded += (_, obj) =>
            Broadcast(new ObjectAdded { Object = ToDto(obj) });

        _objectLayer.ObjectUpdated += (_, args) =>
            Broadcast(new ObjectUpdated { Object = ToDto(args.NewObject) });

        _objectLayer.ObjectDeleted += (_, id) =>
            Broadcast(new ObjectDeleted { ObjectId = id });
    }
    private MapObjectDto ToDto(MapObject obj) => new()
    {
        Id = obj.Id,
        X = obj.X,
        Y = obj.Y,
        Width = obj.Width,
        Height = obj.Height
    };

    private void Broadcast<T>(T message) where T: class => Server.Broadcast(message);
    private MapUdpServer Server => _provider.GetRequiredService<MapUdpServer>();
    public async void Handle(NetPeer peer, byte[] data)
    {
        var type = DetectType(data); // можно использовать префикс или тип в заголовке

        switch (type)
        {
            case "GetObjectsInAreaRequest":
                var req = MemoryPackSerializer.Deserialize<GetObjectsInAreaRequest>(data);
                var objects = await _objectLayer.GetByAreaAsync(req.X1, req.Y1, req.X2, req.Y2);
                var response = new GetObjectsInAreaResponse
                {
                    Objects = objects.Select(ToDto).ToList()
                };
                peer.Send(MemoryPackSerializer.Serialize(response), DeliveryMethod.ReliableOrdered);
                break;

            case "GetRegionsInAreaRequest":
                var regReq = MemoryPackSerializer.Deserialize<GetRegionsInAreaRequest>(data);
                var regions = await _regionLayer.GetRegionsInAreaAsync(regReq.X1, regReq.Y1, regReq.X2, regReq.Y2);
                var regRes = new GetRegionsInAreaResponse
                {
                    Regions = regions.Select(r => new RegionDto { Id = r.Id, Name = r.Name }).ToList()
                };
                peer.Send(MemoryPackSerializer.Serialize(regRes), DeliveryMethod.ReliableOrdered);
                break;
        }
    }
    private string DetectType(byte[] data)
    {
        // Простейший способ: первые байты — тип сообщения
        // В проде лучше использовать MemoryPackUnion или заголовок
        return "GetObjectsInAreaRequest"; // заглушка
    }
}
