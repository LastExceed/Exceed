using System;
using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon {
    class Command {
        public static void Execute(string command, string parameter, Player player) {
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
                            hostility = (byte)Database.Hostility.enemy
                        };
                        xpDummy.Write(player.writer);

                        var kill = new Kill() {
                            killer = player.entityData.guid,
                            victim = 1000,
                            xp = amount
                        };
                        var serverUpdate = new ServerUpdate();
                        serverUpdate.kills.Add(kill);
                        serverUpdate.Write(player.writer, true);
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
                        time.Write(player.writer, true);
                    } catch (Exception) {
                        //invalid syntax
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
