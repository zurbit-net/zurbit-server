using Microsoft.AspNetCore.SignalR;
using Zurbit.Server.Models;
using Zurbit.Server.Services;

namespace Zurbit.Server.Hubs;

public class MeetingRoomHub : Hub
{
    private readonly IMessageStore _messageStore;
    private readonly IMeetingRoomService _meetingRoomService;

    public MeetingRoomHub(IMessageStore messageStore, IMeetingRoomService meetingRoomService)
    {
        _messageStore = messageStore;
        _meetingRoomService = meetingRoomService;
    }

    public async Task JoinRoom(string roomName, string userName)
    {
        _meetingRoomService.AddUserConnection(Context.ConnectionId, userName, roomName);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        var messages = await _messageStore.GetMessagesForRoom(roomName);
        await Clients.Caller.SendAsync("ReceiveHistory", messages);

        var systemMessage = new Message("System", $"{userName} has joined the room.", DateTime.UtcNow);
        await Clients.Group(roomName).SendAsync("ReceiveMessage", systemMessage.UserName, systemMessage.Content, systemMessage.Timestamp);
    }

    public async Task LeaveRoom(string roomName, string userName)
    {
        _meetingRoomService.RemoveUserConnection(Context.ConnectionId);
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
        var user = _meetingRoomService.GetUserByConnectionId(Context.ConnectionId);
        if (user.HasValue)
        {
            await LeaveRoom(user.Value.RoomName, user.Value.UserName);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
