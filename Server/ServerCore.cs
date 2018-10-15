using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Server.Database;

using Microsoft.EntityFrameworkCore;

using Resources;
using Resources.Datagram;
using Resources.Packet;
using Resources.Utilities;
using Server.Plugins;
using Server.Extensions;
namespace Server {
    public static partial class ServerCore {
        public static UdpClient udpClient;
        public static TcpListener tcpListener;
        public static List<Player> players = new List<Player>();
        public static Dictionary<ushort, EntityUpdate> dynamicEntities = new Dictionary<ushort, EntityUpdate>();
        public static UserDatabase userDatabase;
        public static void Start(int port) {
            Log.PrintLn("server starting...");
            userDatabase = new UserDatabase();
            userDatabase.Database.Migrate(); //Ensure database exists
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            new Thread(new ThreadStart(ListenUDP)).Start();
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            new Thread(new ThreadStart(ListenTCP)).Start();

            Extensions.Extensions.Init();
            PluginsCore.Init();
            Log.PrintLn("loading completed");
        }

        private static void ListenTCP() {
            var player = new Player(tcpListener.AcceptTcpClient());
            new Thread(new ThreadStart(ListenTCP)).Start();
            Log.PrintLn(player.IP + " connected", ConsoleColor.Blue);
            try {
                while (true) ProcessPacket(player.reader.ReadByte(), player);
            }
            catch (IOException) {
                if (player.entity != null) {
                    RemovePlayerEntity(player, false);
                }
                players.Remove(player);
                Log.PrintLn(player.IP + " disconnected", ConsoleColor.Red);
            }
        }
        private static void ListenUDP() {
            IPEndPoint source = null;
            while (true) {
                byte[] datagram = udpClient.Receive(ref source);
                var player = players.FirstOrDefault(x => (x.RemoteEndPoint).Equals(source));
                if (player != null && player.entity != null) {
                    try {
                        ProcessDatagram(datagram, player);
                    }
                    catch (IndexOutOfRangeException) {
                        Kick(player, "invalid data received");
                    }
                }
            }
        }

        public static void SendUDP(byte[] data, Player target) {
            udpClient.Send(data, data.Length, target.RemoteEndPoint);
        }
        public static void BroadcastUDP(byte[] data, Player toSkip = null) {
            foreach (var player in players.ToList()) {
                if (player != toSkip) {
                    SendUDP(data, player);
                }
            }
        }

        private static void ProcessPacket(byte packetID, Player source) {
            switch ((ServerPacketID)packetID) {
                case ServerPacketID.VersionCheck:
                    #region VersionCheck
                    source.writer.Write((byte)ServerPacketID.VersionCheck);
                    if (source.reader.ReadInt32() != Config.bridgeVersion) {
                        source.writer.Write(false);
                        //close connection
                        break;
                    }
                    source.writer.Write(true);
                    players.Add(source);
                    foreach (EntityUpdate entity in dynamicEntities.Values) {
                        SendUDP(entity.CreateDatagram(), source);
                    }
                    break;
                #endregion
                case ServerPacketID.Login://login
                    #region login
                    string username = source.reader.ReadString();
                    string password = source.reader.ReadString();
                    source.MAC = source.reader.ReadString();

                    AuthResponse authResponse;
                    if (!players.Contains(source)) {
                        //musnt login without checking bridge version first
                        authResponse = AuthResponse.Unverified;
                    }
                    else if (source.entity != null) {
                        //already logged in into another account
                        authResponse = AuthResponse.UserAlreadyLoggedIn;
                    }
                    else if (players.FirstOrDefault(x => x.entity?.name == username) != null) {
                        //another user is already logged into this account
                        authResponse = AuthResponse.AccountAlreadyActive;
                    }
                    else {
                        authResponse = userDatabase.AuthUser(username, password, (int)source.IP.Address, source.MAC);
                    }
                    source.writer.Write((byte)ServerPacketID.Login);
                    source.writer.Write((byte)authResponse);
                    if (authResponse != AuthResponse.Success) break;

                    source.entity = new EntityUpdate() {
                        guid = AssignGuid(),
                        name = username,
                    };
                    source.writer.Write((ushort)source.entity.guid);
                    source.writer.Write(Config.mapseed);

                    dynamicEntities.Add((ushort)source.entity.guid, source.entity);

                    Log.PrintLn(source.IP + " logged in as " + username, ConsoleColor.Green);
                    break;
                #endregion
                case ServerPacketID.Logout:
                    #region logout
                    if (source.entity == null) break;//not logged in
                    RemovePlayerEntity(source, false);
                    Log.PrintLn(source.IP + " logged out", ConsoleColor.Yellow);
                    break;
                #endregion
                case ServerPacketID.Register:
                    #region Register
                    username = source.reader.ReadString();
                    var email = source.reader.ReadString();
                    password = source.reader.ReadString();

                    RegisterResponse registerResponse;
                    if (!Tools.alphaNumericRegex.IsMatch(username) || !Tools.validEmailRegex.IsMatch(email)) {
                        registerResponse = RegisterResponse.InvalidInput;
                    }
                    else {
                        registerResponse = userDatabase.RegisterUser(username, email, password);
                    }
                    source.writer.Write((byte)ServerPacketID.Register);
                    source.writer.Write((byte)registerResponse);
                    if (registerResponse == RegisterResponse.Success) {
                        Log.PrintLn(source.IP + " registered as " + username, ConsoleColor.Cyan);
                    }
                    break;
                #endregion
                default:
                    Log.PrintLn($"unknown packetID {packetID} received from {source.IP}", ConsoleColor.Magenta);
                    source.tcpClient.Close();
                    break;
            }
        }
        private static void ProcessDatagram(byte[] datagram, Player source) {
            switch ((DatagramID)datagram[0]) {
                case DatagramID.DynamicUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(datagram);
                    EntityUpdated?.Invoke(entityUpdate, source);
                    entityUpdate.Merge(source.entity);
                    BroadcastUDP(entityUpdate.CreateDatagram(), source);
                    break;
                #endregion
                case DatagramID.Attack:
                    #region attack
                    var attack = new Attack(datagram);
                    EntityAttacked?.Invoke(attack, source);
                    source.lastTarget = attack.Target;
                    var target = players.FirstOrDefault(p => p.entity?.guid == attack.Target);
                    if (target != null) SendUDP(attack.data, target);
                    break;
                #endregion
                case DatagramID.Projectile:
                    #region Projectile
                    var projectile = new Projectile(datagram);
                    ProjectileCreated?.Invoke(projectile, source);
                    BroadcastUDP(projectile.data, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.Proc:
                    #region proc
                    var proc = new Proc(datagram);
                    PassiveProcced?.Invoke(proc, source);
                    BroadcastUDP(proc.data, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.Chat:
                    #region chat
                    var chat = new Chat(datagram);
                    ChatMessageReceived?.Invoke(chat, source);
                    break;
                #endregion
                case DatagramID.Interaction:
                    #region interaction
                    var interaction = new Interaction(datagram);
                    EntityInteracted?.Invoke(interaction, source);
                    BroadcastUDP(interaction.data, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.RemoveDynamicEntity:
                    #region removeDynamicEntity
                    var remove = new RemoveDynamicEntity(datagram);
                    EntityRemoved?.Invoke(remove, source);
                    RemovePlayerEntity(source, true);
                    break;
                #endregion
                case DatagramID.SpecialMove:
                    #region specialMove
                    var specialMove = new SpecialMove(datagram);
                    SpecialMoveUsed?.Invoke(specialMove, source);
                    break;
                #endregion
                case DatagramID.HolePunch:
                    break;
                default:
                    Log.PrintLn($"unknown DatagramID {datagram[0]} received from {source.IP}", ConsoleColor.Magenta);
                    Kick(source, "invalid data received");
                    break;
            }
        }

        public static void RemovePlayerEntity(Player player, bool createNewEntity) {
            var rde = new RemoveDynamicEntity() {
                Guid = (ushort)player.entity.guid,
            };
            BroadcastUDP(rde.data, player);
            if (player.tomb != null) {
                rde.Guid = (ushort)player.tomb;
                BroadcastUDP(rde.data);
                player.tomb = null;
            }
            if (createNewEntity) {
                player.entity = new EntityUpdate() {
                    guid = player.entity.guid
                };
            }
            else {
                dynamicEntities.Remove((ushort)player.entity.guid);
                player.entity = null;
            }
        }
        public static void Kick(Player target, string reason) {
            Notify(target, "you got kicked: " + reason);
            target.writer.Write((byte)ServerPacketID.Kick);
            RemovePlayerEntity(target, true);
        }

        public static ushort AssignGuid() {
            ushort newGuid = 1;
            while (dynamicEntities.ContainsKey(newGuid)) newGuid++;
            return newGuid;
        }

        public static void Notify(Player target, string message) {
            var chat = new Chat() {
                Sender = 0,
                Text = message,
            };
            SendUDP(chat.data, target);
        }
    }
}
