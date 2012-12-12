using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;
using Coze.Core.ContentProviders;
using MS.Internal.Xml.XPath;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Security.Application;

namespace Coze.Core.Hubs
{
    public class CozeHub : Hub
    {
        private static readonly ConcurrentDictionary<string, CozeUser> _users =
            new ConcurrentDictionary<string, CozeUser>(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<string, HashSet<string>> _userRooms =
            new ConcurrentDictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<string, CozeRoom> _rooms =
            new ConcurrentDictionary<string, CozeRoom>(StringComparer.OrdinalIgnoreCase);

        private static readonly List<IContentProvider> _contentProviders = new List<IContentProvider>()
            {
                new ImageContentProvider(),
                new YouTubeContentProvider(),
                new CollegeHumorContentProvider()
            };
        public IEnumerable<CozeUser> GetUsers()
        {
            string room = Clients.Caller.room;
            if (string.IsNullOrEmpty(room))
            {
                return Enumerable.Empty<CozeUser>();
            }

            return _rooms[room].Users.Select(u => _users[u]);
        }

        private T TryGetDicValue<T>(IDictionary<string, T> dictionary, string key) where T : class
        {
            if (!dictionary.ContainsKey(key))
            {
                return null;
            }
            return dictionary[key];
        }

        public bool Join()
        {
            var userIdCookie = TryGetDicValue(Context.RequestCookies, "userid");
            if (userIdCookie == null)
            {
                return false;
            }

            CozeUser user = _users.Values.FirstOrDefault(u => u.Id == userIdCookie.Value);

            if (user != null)
            {
                user.ConnectionId = Context.ConnectionId;

                Clients.Caller.id = user.Id;
                Clients.Caller.name = user.Name;
                Clients.Caller.hash = user.Hash;

                HashSet<string> rooms;
                if (_userRooms.TryGetValue(user.Name, out rooms))
                {
                    foreach (var room in rooms)
                    {
                        Clients.Group(room).leave(user);
                        CozeRoom cozeRoom = _rooms[room];
                        cozeRoom.Users.Remove(user.Name);
                    }
                }

                _userRooms[user.Name] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                Clients.Caller.addUser(user);

                return true;
            }

            return false;
        }

        public void Send(string content)
        {
            content = Sanitizer.GetSafeHtmlFragment(content);

            if (!TryHandleCommand(content))
            {
                string roomName = Clients.Caller.room;
                string name = Clients.Caller.name;

                EnsureUserAndRoom();

                HashSet<string> links;
                var messageText = Transform(content, out links);
                var chatMessage = new CozeMessage(name, messageText);

                _rooms[roomName].Messages.Add(chatMessage);

                Clients.Group(roomName).addMessage(chatMessage.Id, chatMessage.User, chatMessage.Text);

                if (links.Any())
                {
                    // REVIEW: is this safe to do? We're holding on to this instance 
                    // when this should really be a fire and forget.
                    var contentTasks = links.Select(ExtractContent).ToArray();
                    Task.Factory.ContinueWhenAll(contentTasks, tasks =>
                        {
                            foreach (var task in tasks)
                            {
                                if (task.IsFaulted)
                                {
                                    Trace.TraceError(task.Exception.GetBaseException().Message);
                                    continue;
                                }

                                if (String.IsNullOrEmpty(task.Result))
                                {
                                    continue;
                                }

                                // Try to get content from each url we're resolved in the query
                                string extractedContent = "<p>" + task.Result + "</p>";

                                // If we did get something, update the message and notify all clients
                                chatMessage.Text += extractedContent;

                                Clients.Group(roomName).addMessageContent(chatMessage.Id, extractedContent);
                            }
                        });
                }
            }
        }

        private bool TryHandleCommand(string message)
        {
            string room = Clients.Caller.room;
            string name = Clients.Caller.name;

            message = message.Trim();
            if (message.StartsWith("/"))
            {
                string[] parts = message.Substring(1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string commandName = parts[0];

                if (commandName.Equals("nick", StringComparison.OrdinalIgnoreCase))
                {
                    string newUserName = string.Join(" ", parts.Skip(1));
                    if (string.IsNullOrEmpty(newUserName))
                    {
                        throw new InvalidOperationException("用户名不能为空!");
                    }
                    if (newUserName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("新名字与原来相同...");
                    }

                    if (!_users.ContainsKey(newUserName))
                    {
                        if (string.IsNullOrEmpty(name) || !_users.ContainsKey(name))
                        {
                            AddUser(newUserName);
                        }
                        else
                        {
                            var oldUser = _users[name];
                            var newUser = new CozeUser()
                                {
                                    Name = newUserName,
                                    Hash = GetMD5Hash(newUserName),
                                    Id = oldUser.Id,                                   
                                    ConnectionId = oldUser.ConnectionId
                                };

                            _users[newUserName] = newUser;
                            _userRooms[newUserName] = new HashSet<string>(_userRooms[name]);

                            if (_userRooms[name].Any())
                            {
                                foreach (var r in _userRooms[name])
                                {
                                    _rooms[r].Users.Remove(name);
                                    _rooms[r].Users.Add(newUserName);
                                    Clients.Group(r).changeUserName(oldUser, newUser);
                                }
                            }

                            HashSet<string> ignoredRoom;
                            CozeUser ignoredUser;
                            _userRooms.TryRemove(name, out ignoredRoom);
                            _users.TryRemove(name, out ignoredUser);

                            Clients.Caller.hash = newUser.Hash;
                            Clients.Caller.name = newUser.Name;
                            Clients.Caller.iconame = newUser.IcoName;
                            Clients.Caller.changeUserName(oldUser, newUser);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(String.Format("用户名 '{0}' 已被使用!", newUserName));
                    }

                    return true;
                }
                else
                {
                    EnsureUser();
                    if (commandName.Equals("rooms", StringComparison.OrdinalIgnoreCase))
                    {
                        var rooms = _rooms.Select(r => new
                            {
                                Name = r.Key,
                                Count = r.Value.Users.Count
                            });

                        Clients.Caller.showRooms(rooms);

                        return true;
                    }
                    else if (commandName.Equals("join", StringComparison.OrdinalIgnoreCase))
                    {
                        if (parts.Length == 1)
                        {
                            throw new InvalidOperationException("请选择房间");
                        }

                        string newRoom = parts[1];
                        CozeRoom cozeRoom;
                        if (!_rooms.TryGetValue(newRoom, out cozeRoom))
                        {
                            cozeRoom = new CozeRoom();
                            _rooms.TryAdd(newRoom, cozeRoom);
                        }

                        if (!string.IsNullOrEmpty(room))
                        {
                            _userRooms[name].Remove(room);
                            _rooms[room].Users.Remove(name);

                            Clients.Group(room).leave(_users[name]);
                            Groups.Remove(Context.ConnectionId, room);
                        }

                        _userRooms[name].Add(newRoom);
                        if (!cozeRoom.Users.Add(name))
                        {
                            throw new InvalidOperationException("你已经在此房间了!");
                        }

                        Clients.Group(newRoom).addUser(_users[name]);

                        Clients.Caller.room = newRoom;
                        Groups.Add(Context.ConnectionId, newRoom);
                        Clients.Caller.refreshRoom(newRoom);

                        return true;
                    }
                    else if (commandName.Equals("msg", StringComparison.OrdinalIgnoreCase))
                    {
                        if (_users.Count == 1)
                        {
                            throw new InvalidOperationException("这里就只有你一个人");
                        }

                        if (parts.Length < 2)
                        {
                            throw new InvalidOperationException("你要给谁发私人消息?");
                        }

                        string to = parts[1];
                        if (to.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidOperationException("不能给自己发私人消息!");
                        }

                        if (!_users.ContainsKey(to))
                        {
                            throw new InvalidOperationException(String.Format("不能打到此人 '{0}'.", to));
                        }

                        string messageText = String.Join(" ", parts.Skip(2)).Trim();

                        if (String.IsNullOrEmpty(messageText))
                        {
                            throw new InvalidOperationException(String.Format("你想跟 '{0}' 说什么.", to));
                        }

                        string recipientId = _users[to].ConnectionId;
                        // Send a message to the sender and the sendee                        
                        Clients.Group(recipientId).sendPrivateMessage(name, to, messageText);
                        Clients.Caller.sendPrivateMessage(name, to, messageText);

                        return true;
                    }
                    else
                    {
                        EnsureUserAndRoom();
                        if (commandName.Equals("me", StringComparison.OrdinalIgnoreCase))
                        {
                            if (parts.Length == 1)
                            {
                                throw new InvalidProgramException("你要干什么?");
                            }
                            var content = String.Join(" ", parts.Skip(1));

                            Clients.Group(room).sendMeMessage(name, content);
                            return true;
                        }
                        else if (commandName.Equals("leave", StringComparison.OrdinalIgnoreCase))
                        {
                            CozeRoom chatRoom;
                            if (_rooms.TryGetValue(room, out chatRoom))
                            {
                                chatRoom.Users.Remove(name);
                                _userRooms[name].Remove(room);

                                Clients.Group(room).leave(_users[name]);
                            }

                            Groups.Remove(Context.ConnectionId, room);

                            Clients.Caller.room = null;

                            return true;
                        }

                        throw new InvalidOperationException(String.Format("'{0}' 错误的命令.", parts[0]));
                    }
                }
            }
            return false;
        }

        private CozeUser AddUser(string newUserName)
        {
            var user = new CozeUser(newUserName, GetMD5Hash(newUserName)) { ConnectionId = Context.ConnectionId };

            _users[newUserName] = user;
            _userRooms[newUserName] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            Clients.Caller.name = user.Name;
            Clients.Caller.hash = user.Hash;
            Clients.Caller.id = user.Id;
            Clients.Caller.iconame = user.IcoName;
            Clients.Caller.addUser(user);

            return user;
        }

        private string GetMD5Hash(string name)
        {
            return string.Join("",
                               MD5.Create().ComputeHash(Encoding.Default.GetBytes(name)).Select(b => b.ToString("x2")));
        }

        private void EnsureUser()
        {
            string name = Clients.Caller.name;
            if (string.IsNullOrEmpty(name) || !_users.ContainsKey(name))
            {
                throw new InvalidOperationException("你还没有用户名. 请使用 '/nick nickname' 创建一个.");
            }
        }

        private void EnsureUserAndRoom()
        {
            EnsureUser();

            //todo: restore when groups work
            string room = Clients.Caller.room;
            string name = Clients.Caller.name;

            if (string.IsNullOrEmpty(room) || !_rooms.ContainsKey(room))
            {
                throw new InvalidOperationException("请使用 '/join roomname' 加入一个房间.");
            }

            HashSet<string> rooms;
            if (!_userRooms.TryGetValue(name, out rooms) || !rooms.Contains(room))
            {
                throw new InvalidOperationException(String.Format("你不在房间 '{0}'. 请使用 '/join {0}' 加入.", room));
            }
        }

        private string Transform(string message, out HashSet<string> extractedUrls)
        {
            const string urlPattern =
                @"((https?|ftp)://|www\.)[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";

            var urls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            message = Regex.Replace(message, urlPattern, m =>
                {
                    string httpPortion = String.Empty;
                    if (!m.Value.Contains("://"))
                    {
                        httpPortion = "http://";
                    }

                    string url = httpPortion + m.Value;

                    urls.Add(url);

                    return String.Format(CultureInfo.InvariantCulture,
                                         "<a rel=\"nofollow external\" target=\"_blank\" href=\"{0}\" title=\"{1}\">{1}</a>",
                                         url, m.Value);
                });

            extractedUrls = urls;
            return message;
        }

        private Task<string> ExtractContent(string url)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            var requestTask = Task.Factory.FromAsync((cb, state) => request.BeginGetResponse(cb, state), ar => request.EndGetResponse(ar), null);
            return requestTask.ContinueWith(task => ExtractContent((HttpWebResponse)task.Result));
        }

        private string ExtractContent(HttpWebResponse response)
        {
            return _contentProviders.Select(c => c.GetContent(response))
                                    .FirstOrDefault(content => content != null);
        }

        [Serializable]
        public class CozeMessage
        {
            public string Id { get; private set; }
            public string User { get; set; }
            public string Text { get; set; }

            public CozeMessage(string user, string text)
            {
                User = user;
                Text = text;
                Id = Guid.NewGuid().ToString("d");
            }
        }

        [Serializable]
        public class CozeUser
        {
            public string ConnectionId { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Hash { get; set; }
            public string IcoName { get; set; }
            public CozeUser()
            {
                this.IcoName = GetIcoName();
            }

            public CozeUser(string name, string hash)
            {
                this.Name = name;
                this.Hash = hash;
                this.Id = Guid.NewGuid().ToString("d");
                this.IcoName = GetIcoName();
            }
            private string GetIcoName()
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                path = Path.Combine(path, "Content/images/faces");

                var files = Directory.GetFiles(path, "*.ico");
                return Path.GetFileName(files.RandomItem());
            }


        }

        public class CozeRoom
        {
            public List<CozeMessage> Messages { get; set; }
            public HashSet<string> Users { get; set; }

            public CozeRoom()
            {
                Messages = new List<CozeMessage>();
                Users = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
        }
    }
    public static class Helper
    {
        private static readonly Random random = new Random();
        public static T RandomItem<T>(this IList<T> list)
        {
            if (list != null)
            {
                return list[random.Next(list.Count)];
            }
            return default(T);
        }
    }
}
