using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Resources;
namespace Server.Plugins.Duel
{
    class Commands
    {
        public static Plugin pluginRef;
        public static void Init(Plugin plugin)
        {
            Server.ChatMessageReceived += ParseAsCommand;
            pluginRef = plugin;
        }
        private static void ParseAsCommand(string message, Player sourceTemp)
        {
            PlayerDuel target;
            PlayerDuel source = DuelCore.players.FirstOrDefault(x => x.entity.guid == sourceTemp.entity.guid);
            DuelSystem duelFinder;
            if (!message.StartsWith("/duel"))
            {
                return;
            }
            var parameters = message.Substring(1 + pluginRef.getName().Length).Split(" ");
            var command = parameters[0].ToLower();
            switch (command)
            {
                case "help":
                    #region help
                    Server.Notify(source, string.Format("/duel start [player2]"));
                    Server.Notify(source, string.Format("/duel accept"));
                    Server.Notify(source, string.Format("/duel refuse"));
                    Server.Notify(source, string.Format("/duel spectate [playerName]"));
                    Server.Notify(source, string.Format("/duel stop"));
                    break;
                #endregion
                case "stop":
                    #region stop
                    Console.WriteLine(DuelCore.duels.Count());
                    duelFinder = DuelCore.duels.LastOrDefault(x => x.player1.entity.name.Contains(source.entity.name) || x.player2.entity.name.Contains(source.entity.name));
                    if (duelFinder != null)
                    {
                        duelFinder.Stop();
                        Console.WriteLine(DuelCore.duels.Count());
                    }
                    else
                    {
                        Server.Notify(source, "[Duel] No duel ongoing");
                    }
                    break;
                #endregion
                case "start":
                    #region start
                    if (parameters.Length == 3)
                    {
                        target = DuelCore.players.FirstOrDefault(x => x.entity.name.Contains(parameters[2]));
                        if (target == null)
                        {
                            Server.Notify(source, "[Duel] invalid target");
                            break;
                        }
                        else if (target == source && source.entity.name != "BLIZZY")
                        {
                            Server.Notify(source, "[Duel] Unfortunatly, you can't duel yourself");
                            break;
                        }
                        else if (source.Duel == true)
                        {
                            Server.Notify(source, "[Duel] You're already involved in a duel");
                            break;
                        }
                        else if (target.Duel == true)
                        {
                            Server.Notify(source, "[Duel] The target is already involved in a duel");
                            break;
                        }
                        Server.Notify(target, string.Format("[Duel] {0} wants to duel you ! /duel accept or /duel refuse", source.entity.name));
                        var duel = new DuelSystem(source, target, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                        DuelCore.duels.Add(duel);
                        Console.WriteLine(String.Format("{0} ask a duel with {1}, Total Duel ongoing : {2}", source.entity.name, target.entity.name, DuelCore.duels.Count()));
                        new Thread(() => duel.RunDuel()).Start();
                    }
                    else
                    {
                        Server.Notify(source, string.Format("Syntax : /duel start [player2]"));
                    }
                    break;
                #endregion
                case "accept":
                    #region accept
                    duelFinder = DuelCore.duels.LastOrDefault(x => x.player2.entity.name.Contains(source.entity.name));
                    if (duelFinder != null && source.Duel == null)
                    {
                        duelFinder.AcceptDuel();
                    }
                    else if (source.Duel == true)
                    {
                        Server.Notify(source, string.Format("[Duel] Duel already ongoing"));
                    }
                    else
                    {
                        Server.Notify(source, string.Format("[Duel] No duel request found"));
                    }
                    break;
                #endregion
                case "refuse":
                    #region refuse
                    duelFinder = DuelCore.duels.LastOrDefault(x => x.player2.entity.name.Contains(source.entity.name));
                    if (duelFinder != null && source.Duel == null)
                    {
                        duelFinder.RefuseDuel();
                    }
                    else if (source.Duel == true)
                    {
                        Server.Notify(source, string.Format("[Duel] Duel already ongoing"));
                    }
                    else
                    {
                        Server.Notify(source, string.Format("[Duel] No duel request found"));
                    }
                    break;
                #endregion
                case "spectate":
                    #region spectate
                    if (parameters.Length == 3)
                    {
                        var name = parameters[2];
                        duelFinder = DuelCore.duels.LastOrDefault(x => x.player1.entity.name.Contains(name) || x.player2.entity.name.Contains(name));
                        if (duelFinder != null && source.Duel == null)
                        {
                            duelFinder.Spectate(source);
                            Server.Notify(source, string.Format("[Duel] You are spectating the duel of {0} !", name));
                        }
                        else
                        {
                            Server.Notify(source, string.Format("[Duel] Invalid Target"));
                        }
                    }
                    else
                    {
                        Server.Notify(source, string.Format("Syntax: /duel spectate [playerName]"));
                    }
                    #endregion
                    break;
                default:
                    if (PluginsCore.pluginsWithCommands.Last().getName() == pluginRef.getName())
                    {
                        Server.Notify(source, string.Format("Type /duel help for more information"));
                    }
                    break;
            }
        }
    }
}
