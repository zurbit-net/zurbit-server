using Zurbit.Server.Models;

namespace Zurbit.Server.Services;

public interface IMeetingRoomService
{
    void AddUserConnection(string connectionId, string userName, string roomName);
    void RemoveUserConnection(string connectionId);
    (string UserName, string RoomName)? GetUserByConnectionId(string connectionId);
}
