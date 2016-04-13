using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace Challenging_Challenges.Hubs
{
    [Authorize]
    public class NotificationHub: Hub
    {
        public readonly static ConnectionMapping<string> Connections =
                    new ConnectionMapping<string>();

        public void SendAchievementMessage(string user, string achievementId)
        {
            foreach (var connectionId in Connections.GetConnections(user))
            {
                Clients.Client(connectionId).showAchievement(achievementId);
            }
        }

        public override Task OnConnected()
        {
            string user = Context.User.Identity.GetUserId();

            Connections.Add(user, Context.ConnectionId);
            Groups.Add(Context.ConnectionId, user);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string user = Context.User.Identity.GetUserId();

            Connections.Remove(user, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string user = Context.User.Identity.GetUserId();

            if (!Connections.GetConnections(user).Contains(Context.ConnectionId))
            {
                Connections.Add(user, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }

    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count => _connections.Count;

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

    }

}
