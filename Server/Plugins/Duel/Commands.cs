using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Resources;
using Server.Plugins.Duel.Resources;
using CommandsBaseMessages = Server.Plugins.CommandsBaseMessages;
namespace Server.Plugins.Duel
{
    class Commands
    {
        public static PluginBase pluginRef;
        public static void Init(PluginBase plugin)
        {
            pluginRef = plugin;
        }
        private static Boolean ParseAsCommand(string message, Player sourceTemp)
        {
            PlayerDuel target;
            PlayerDuel source = DuelCore.players.FirstOrDefault(x => x.entity.guid == sourceTemp.entity.guid);
            DuelSystem duelFinder;
            message = message.ToLower();
            var commandName = message.Substring(1).Split(" ")[0];
            if (!(commandName == pluginRef.getName().ToLower()))
            {
                return false;
            }
            if (message.Length == 1 + pluginRef.getName().Length)
            {
                Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSyntax));
                return true;
            }
            var parameters = message.Substring(2 + pluginRef.getName().Length).Split(" ");
            switch (parameters[0])
            {
                case "help":
                    #region help
                    Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.startSyntax));
                    Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.acceptSyntax));
                    Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.refuseSyntax));
                    Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.stopSyntax));
                    Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.spectateSyntax));
                    break;
                #endregion
                case "stop":
                    #region stop
                    duelFinder = DuelCore.duels.LastOrDefault(x => x.player1.entity.name.Contains(source.entity.name) || x.player2.entity.name.Contains(source.entity.name));
                    if (duelFinder != null)
                    {
                        duelFinder.Stop();
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoDuelRequestFound));
                    }
                    break;
                #endregion
                case "start":
                    #region start
                    if (parameters.Length == 3)
                    {
                        target = DuelCore.players.FirstOrDefault(x => x.entity.name.Contains(parameters[1]));
                        if (target == null)
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseInvalidTarget));
                            break;
                        }
                        else if (target == source && source.entity.name != "BLIZZY")
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.startSelfError));
                            break;
                        }
                        else if (source.Duel == true)
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseAlreadyDueling));
                            break;
                        }
                        else if (target.Duel == true)
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.startTargetAlreadyDueling));
                            break;
                        }
                        Server.Notify(source, CommandsMessages.getMessage(String.Format(CommandsMessages.startAcceptMessage,source.entity.name)));
                        var duel = new DuelSystem(source, target, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                        DuelCore.duels.Add(duel);
                        new Thread(() => duel.RunDuel()).Start();
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.startSyntax));
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
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseAlreadyDueling));
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoDuelRequestFound));
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
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseAlreadyDueling));
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoDuelRequestFound));
                    }
                    break;
                #endregion
                case "spectate":
                    #region spectate
                    if (parameters.Length == 2)
                    {
                        var name = parameters[1];
                        duelFinder = DuelCore.duels.LastOrDefault(x => x.player1.entity.name.Contains(name) || x.player2.entity.name.Contains(name));
                        if (duelFinder != null && source.Duel == null)
                        {
                            duelFinder.Spectate(source);
                            Server.Notify(source, CommandsMessages.getMessage(String.Format(CommandsMessages.spectateSuccess, name)));
                        }
                        else
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseInvalidTarget));
                        }
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.spectateSyntax));
                    }
                    #endregion
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
