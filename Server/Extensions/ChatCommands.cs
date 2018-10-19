﻿using Resources;
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
                    break;
                case "btfo":
                    break;
                case "ban":
                    #region ban
                    if (!minimumPermission(source,3)) {
                        ServerCore.Notify(source, "no permission");
                        break;
                    }
                    if (parameters.Length == 1) {
                        ServerCore.Notify(source, string.Format("usage example: /kick blackrock"));
                        break;
                    }
                    var target = ServerCore.players.FirstOrDefault(x => x.entity.name.Contains(parameters[1]));
                    if (target == null) {
                        ServerCore.Notify(source, "invalid target");
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
                        ServerCore.Notify(source, string.Format("usage example: /time 12:00"));
                        break;
                    }
                    var clock = parameters[1].Split(":");
                    if (clock.Length < 2 ||
                    !int.TryParse(clock[0], out int hour) ||
                    !int.TryParse(clock[1], out int minute)) {
                        ServerCore.Notify(source, string.Format("invalid syntax"));
                        break;
                    }
                    var inGameTime = new InGameTime() {
                        Milliseconds = (hour * 60 + minute) * 60000,
                    };
                    ServerCore.SendUDP(inGameTime.data, source);
                    break;
                #endregion
                default:
                    ServerCore.Notify(source, string.Format("unknown command '{0}'", parameters[0]));
                    break;
            }
        }
        private static Boolean minimumPermission(Player source,int roleId)
        {
            if(source.Permission >= roleId)
            {
                return true;
            }
            return false;
        }
    }
}
