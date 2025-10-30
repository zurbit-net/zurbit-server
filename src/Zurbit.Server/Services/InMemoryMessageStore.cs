using System.Collections.Concurrent;
using Zurbit.Server.Models;

namespace Zurbit.Server.Services;

public class InMemoryMessageStore : IMessageStore
{
    private readonly ConcurrentDictionary<string, List<Message>> _messages = new();

    public Task AddMessage(string roomName, Message message)
    {
        var roomMessages = _messages.GetOrAdd(roomName, _ => new List<Message>());
        lock (roomMessages)
        {
            roomMessages.Add(message);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Message>> GetMessagesForRoom(string roomName)
    {
        var roomMessages = _messages.GetOrAdd(roomName, _ => new List<Message>());
        List<Message> messagesSnapshot;
        lock (roomMessages)
        {
            messagesSnapshot = new List<Message>(roomMessages);
        }
        return Task.FromResult<IEnumerable<Message>>(messagesSnapshot);
    }
}
