using System.Collections.Concurrent;
using Zurbit.Server.Models;

namespace Zurbit.Server.Services;

public class MeetingRoomService : IMeetingRoomService
{
    private readonly ConcurrentDictionary<string, (string UserName, string RoomName)> _userConnections = new();

    public void AddUserConnection(string connectionId, string userName, string roomName)
    {
        _userConnections.TryAdd(connectionId, (userName, roomName));
    }

    public void RemoveUserConnection(string connectionId)
    {
        _userConnections.TryRemove(connectionId, out _);
    }

    public (string UserName, string RoomName)? GetUserByConnectionId(string connectionId)
    {
        _userConnections.TryGetValue(connectionId, out var usernameAndRoom);
        return usernameAndRoom;
    }

}
