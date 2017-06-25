using System;
using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon {
    class Command {
        public static void execute(string command, string parameter, Player player) {
            switch (command) {
                case "spawn":
                    break;

                case "reload_world":
                    break;

                case "xp":
                    try {
                        int amount = Convert.ToInt32(parameter);

                        var xpDummy = new EntityUpdate();
                        xpDummy.guid = 1000;
                        xpDummy.bitfield1 = 0b00000000_00000000_00000000_10000000;
                        xpDummy.hostility = (byte)Database.Hostility.enemy;
                        xpDummy.send(player);

                        var kill = new Kill();
                        kill.killer = player.entityData.guid;
                        kill.victim = 1000;
                        kill.xp = amount;

                        var serverUpdate = new ServerUpdate();
                        serverUpdate.kills.Add(kill);
                        serverUpdate.send(player);
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

                        var time = new Time();
                        time.time = (hour * 60 + minute) * 60000;
                        time.send(player);
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
