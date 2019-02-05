using Resources;
using Resources.Datagram;
using Resources.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Extensions {
    public static class ChatCommands {
        public static void Init() {
            ServerCore.ChatMessageReceived += ParseAsCommand;
        }

        private static void ParseAsCommand(string message, Player source) {
            if (!message.StartsWith("/")) {
                return;
            }
            var parameters = message.Substring(1).Split(" ");
            var command = parameters[0].ToLower();
            switch (command) {
                case "kick":
                case "btfo":
                case "ban":
                    #region ban
                    if (source.entity.name != "BLACKROCK") {
                        source.Notify("no permission");
                        break;
                    }
                    if (parameters.Length == 1) {
                        source.Notify($"usage example: /{parameters[0]} blackrock");
                        break;
                    }
                    var target = ServerCore.players.FirstOrDefault(x => x.entity.name.Contains(parameters[1]));
                    if (target == null) {
                        source.Notify("invalid target");
                        break;
                    };
                    var reason = "no reason specified";
                    if (parameters.Length > 2) {
                        reason = parameters[2];
                    }
                    if (command == "kick") {
                        ServerCore.Kick(target, reason);
                        break;
                    }
                    target.writer.Write((byte)ServerPacketID.BTFO);
                    target.writer.Write(reason);
                    if (command == "ban") {
                        ServerCore.userDatabase.BanUser(target.entity.name, (int)target.IP.Address, target.MAC, reason);
                    }
                    ServerCore.RemovePlayerEntity(target, false);
                    break;
                #endregion
                case "bleeding":

                    break;
                case "time":
                    #region time
                    if (parameters.Length == 1) {
                        source.Notify("usage example: /time 12:00");
                        break;
                    }
                    var clock = parameters[1].Split(":");
                    if (clock.Length < 2 ||
                    !int.TryParse(clock[0], out int hour) ||
                    !int.TryParse(clock[1], out int minute)) {
                        source.Notify("invalid syntax");
                        break;
                    }
                    var inGameTime = new InGameTime() {
                        Milliseconds = (hour * 60 + minute) * 60000,
                    };
                    ServerCore.SendUDP(inGameTime.data, source);
                    break;
                #endregion
                default:
                    source.Notify($"unknown command '{parameters[0]}'");
                    break;
            }
        }
    }
}
