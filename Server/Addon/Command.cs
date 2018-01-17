﻿using System;
using Resources;
using Resources.Packet;
using Resources.Datagram;
using Resources.Utilities;

namespace Server.Addon {
    class Command {
        public static void TCP(string command, string parameter, Player player) {
            switch (command) {
                case "spawn":
                    break;

                case "reload_world":
                    break;

                case "xp":
                    try {
                        int amount = Convert.ToInt32(parameter);

                        var xpDummy = new EntityUpdate() {
                            guid = 1000,
                            hostility = Hostility.enemy
                        };
                        xpDummy.Write(player.writer);

                        var kill = new ServerUpdate.Kill() {
                            killer = player.guid,
                            victim = 1000,
                            xp = amount
                        };
                        var serverUpdate = new ServerUpdate();
                        serverUpdate.kills.Add(kill);
                        serverUpdate.Write(player.writer);
                        break;
                    } catch (Exception) {
                        //invalid syntax
                    }
                    break;
                case "time":
                    try {
                        int index = parameter.IndexOf(":");
                        int hour = Convert.ToInt32(parameter.Substring(0, index));
                        int minute = Convert.ToInt32(parameter.Substring(index + 1));

                        var time = new Time() {
                            time = (hour * 60 + minute) * 60000
                        };
                        time.Write(player.writer);
                    } catch (Exception) {
                        //invalid syntax
                    }
                    break;

                default:
                    break;
            }
        }
        public static void Server(string text, Player source, ServerUDP server) {
            string parameter = string.Empty;
            string command = text.Substring(1);
            if (command.Contains(" ")) {
                int spaceIndex = command.IndexOf(" ");
                parameter = command.Substring(spaceIndex + 1);
                command = command.Substring(0, spaceIndex);
            }

            switch (command.ToLower()) {
                case "spawn":
                    var entityUpdate = new EntityUpdate() {
                        guid = source.guid,
                        position = new LongVector() {
                            x = 543093329157,
                            y = 546862296355,
                            z = 14423162
                        }
                    };
                    server.SendUDP(entityUpdate.Data, source);
                    break;

                case "load":
                    server.worldUpdate.Write(source.writer);
                    break;

                case "ban":
                    ushort guid = 0;
                    if (!source.admin || !ushort.TryParse(parameter, out guid)) {
                        break;
                    }
                    break;

                case "time":
                    try {
                        int index = parameter.IndexOf(":");
                        int hour = Convert.ToInt32(parameter.Substring(0, index));
                        int minute = Convert.ToInt32(parameter.Substring(index + 1));

                        var inGameTime = new InGameTime() {
                            Time = (hour * 60 + minute) * 60000
                        };
                        server.SendUDP(inGameTime.data, source);
                    } catch (Exception) {
                        //invalid syntax
                    }
                    break;
                case "set":
                    break;

                default:
                    break;
            }
        }
    }
}
