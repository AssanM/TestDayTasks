using LiteNetLib;
using MemoryPack;
using testDay4.contracts.Events;
using testDay4.contracts.Requests;
using testDay4.contracts.Response;
using testDay4.domain.Entities;
using testDay4.domain.Interfaces;
namespace testDay4.server;
public class MessageDispatcher
{
    private readonly IObjectLayer _objectLayer;
    private readonly IRegionLayer _regionLayer;
    private readonly MapUdpServer _server;
    public MessageDispatcher(IObjectLayer objectLayer, IRegionLayer regionLayer, MapUdpServer server)
    {
        _objectLayer = objectLayer;
        _regionLayer = regionLayer;
        _server = server;

        _objectLayer.ObjectAdded += (_, obj) =>
            _server.Broadcast(new ObjectAdded { Object = ToDto(obj) });

        _objectLayer.ObjectUpdated += (_, args) =>
            _server.Broadcast(new ObjectUpdated { NewObject = ToDto(args.NewObject) });

        _objectLayer.ObjectDeleted += (_, id) =>
            _server.Broadcast(new ObjectDeleted { ObjectId = id });
    }

    public async void Handle(NetPeer peer, byte[] data)
    {
        var type = DetectType(data); // можно использовать префикс или тип в заголовке

        switch (type)
        {
            case "GetObjectsInAreaRequest":
                var req = MemoryPackSerializer.Deserialize<GetObjecstInAreaRequest>(data);
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

    private MapObjectDto ToDto(MapObject obj) => new()
    {
        Id = obj.Id,
        X = obj.X,
        Y = obj.Y,
        Width = obj.Width,
        Height = obj.Height
    };

    private string DetectType(byte[] data)
    {
        // Простейший способ: первые байты — тип сообщения
        // В проде лучше использовать MemoryPackUnion или заголовок
        return "GetObjectsInAreaRequest"; // заглушка
    }

}
