using Microsoft.AspNetCore.SignalR;
using Zurbit.Server.Services;

namespace Zurbit.Server.Hubs;

public class RoomChatHub : Hub
{
    private readonly IMessageStore _messageStore;

    public RoomChatHub(IMessageStore messageStore)
    {
        _messageStore = messageStore;
    }

    // add methods here
}
