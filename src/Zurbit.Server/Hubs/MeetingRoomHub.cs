using Microsoft.AspNetCore.SignalR;
using Zurbit.Server.Models;
using Zurbit.Server.Services;

namespace Zurbit.Server.Hubs;

public class MeetingRoomHub : Hub
{
    private readonly IMessageStore _messageStore;

    public MeetingRoomHub(IMessageStore messageStore)
    {
        _messageStore = messageStore;
    }

    public async Task JoinRoom(string roomName, string userName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        var messages = await _messageStore.GetMessagesForRoom(roomName);

        await Clients.Caller.SendAsync("ReceiveHistory", messages);

        var systemMessage = new Message("System", $"{userName} has joined the room.", DateTime.UtcNow);
        await Clients.Group(roomName).SendAsync("ReceiveMessage", systemMessage.UserName, systemMessage.Content, systemMessage.Timestamp);
    }

    public async Task LeaveRoom(string roomName, string userName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

        var systemMessage = new Message("System", $"{userName} has left the room.", DateTime.UtcNow);
        await Clients.OthersInGroup(roomName).SendAsync("ReceiveMessage", systemMessage.UserName, systemMessage.Content, systemMessage.Timestamp);
    }

    public async Task SendMessageToGroup(string roomName, string userName, string messageContent)
    {
        var newMessage = new Message(userName, messageContent, DateTime.UtcNow);

        await _messageStore.AddMessage(roomName, newMessage);

        await Clients.Group(roomName).SendAsync("ReceiveMessage", newMessage.UserName, newMessage.Content, newMessage.Timestamp);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // we would need a way to know which room(s) the user was in.
        // var userConnection = _userTracker.GetUser(Context.ConnectionId);
        // if (userConnection!= null)
        // {
        //     await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.Room);
        //     await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", "System", $"{userConnection.UserName} has left the room.");
        // }

        await base.OnDisconnectedAsync(exception);
    }
}
