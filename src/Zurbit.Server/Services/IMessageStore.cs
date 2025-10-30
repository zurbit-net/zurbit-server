using Zurbit.Server.Models;

namespace Zurbit.Server.Services;

public interface IMessageStore
{
    Task AddMessage(string roomName, Message message);
    Task<IEnumerable<Message>> GetMessagesForRoom(string roomName);
}
