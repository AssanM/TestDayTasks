using LiteNetLib;
using MemoryPack;
using System.Net;
using System.Net.Sockets;
using tileWorld.infrastructure.Network.Dispatcher;

namespace tileWorld.infrastructure.Network.Server;

public class MapUdpServer : INetEventListener
{
    private readonly MessageDispatcher _dispatcher;
    private readonly NetManager _server;
    private readonly List<NetPeer> _clients = new();

    public MapUdpServer(MessageDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _server = new NetManager(this)
        {
            AutoRecycle = true,
            UpdateTime = 15,
            DisconnectTimeout = 5000,
            UnconnectedMessagesEnabled = false
        };
    }

    public void Start(int port)
    {
        _server.Start(port);
        Task.Run(() => Loop());
    }
    private async Task Loop()
    {
        while (true)
        {
            _server.PollEvents();
            await Task.Delay(15);
        }
    }
    public void OnConnectionRequest(ConnectionRequest request) =>
        request.AcceptIfKey("map");

    public void OnPeerConnected(NetPeer peer) => _clients.Add(peer);
    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo info) => _clients.Remove(peer);

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod method)
    {
        var data = reader.GetRemainingBytes();
        _dispatcher.Handle(peer, data);
        reader.Recycle();
    }

    public void Broadcast<T>(T message) where T : class
    {
        var bytes = MemoryPackSerializer.Serialize(message);
        foreach (var peer in _clients)
            peer.Send(bytes, DeliveryMethod.ReliableOrdered);
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        throw new NotImplementedException();
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        throw new NotImplementedException();
    }
}
