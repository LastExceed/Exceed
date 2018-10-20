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
    class Commands : CommandsBase
    {

        public Commands(PluginBase plugin) : base(plugin)
        {
        }
        public override Boolean ParseAsCommand(string message, Player sourceTemp)
        {
            PlayerDuel target;
            PlayerDuel source = ((DuelCore)pluginRef).players.FirstOrDefault(x => x.entity.guid == sourceTemp.entity.guid);
            DuelSystem duelFinder;
            message = message.ToLower();
            var commandName = message.Substring(1).Split(" ")[0];
            if (!(commandName == pluginRef.getName().ToLower()))
            {
                return false;
            }
            if (message.Length == 1 + pluginRef.getName().Length)
            {
                ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSyntax));
                return true;
            }
            var parameters = message.Substring(2 + pluginRef.getName().Length).Split(" ");
            switch (parameters[0])
            {
                case "help":
                    #region help
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.startSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.acceptSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.refuseSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.stopSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.spectateSyntax));
                    break;
                #endregion
                case "stop":
                    #region stop
                    duelFinder = ((DuelCore)pluginRef).duels.LastOrDefault(x => x.player1.entity.name.Contains(source.entity.name) || x.player2.entity.name.Contains(source.entity.name));
                    if (duelFinder != null)
                    {
                        duelFinder.Stop();
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoDuelRequestFound));
                    }
                    break;
                #endregion
                case "start":
                    #region start
                    if (parameters.Length == 3)
                    {
                        target = ((DuelCore)pluginRef).players.FirstOrDefault(x => x.entity.name.Contains(parameters[1]));
                        if (target == null)
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseInvalidTarget));
                            break;
                        }
                        else if (target == source && source.entity.name != "BLIZZY")
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.startSelfError));
                            break;
                        }
                        else if (source.Duel == true)
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseAlreadyDueling));
                            break;
                        }
                        else if (target.Duel == true)
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.startTargetAlreadyDueling));
                            break;
                        }
                        ServerCore.Notify(source, CommandsMessages.getMessage(String.Format(CommandsMessages.startAcceptMessage,source.entity.name)));
                        var duel = new DuelSystem(pluginRef,source, target, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                        ((DuelCore)pluginRef).duels.Add(duel);
                        new Thread(() => duel.RunDuel()).Start();
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.startSyntax));
                    }
                    break;
                #endregion
                case "accept":
                    #region accept
                    duelFinder = ((DuelCore)pluginRef).duels.LastOrDefault(x => x.player2.entity.name.Contains(source.entity.name));
                    if (duelFinder != null && source.Duel == null)
                    {
                        duelFinder.AcceptDuel();
                    }
                    else if (source.Duel == true)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseAlreadyDueling));
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoDuelRequestFound));
                    }
                    break;
                #endregion
                case "refuse":
                    #region refuse
                    duelFinder = ((DuelCore)pluginRef).duels.LastOrDefault(x => x.player2.entity.name.Contains(source.entity.name));
                    if (duelFinder != null && source.Duel == null)
                    {
                        duelFinder.RefuseDuel();
                    }
                    else if (source.Duel == true)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseAlreadyDueling));
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoDuelRequestFound));
                    }
                    break;
                #endregion
                case "spectate":
                    #region spectate
                    if (parameters.Length == 2)
                    {
                        var name = parameters[1];
                        duelFinder = ((DuelCore)pluginRef).duels.LastOrDefault(x => x.player1.entity.name.Contains(name) || x.player2.entity.name.Contains(name));
                        if (duelFinder != null && source.Duel == null)
                        {
                            duelFinder.Spectate(source);
                            ServerCore.Notify(source, CommandsMessages.getMessage(String.Format(CommandsMessages.spectateSuccess, name)));
                        }
                        else
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseInvalidTarget));
                        }
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.spectateSyntax));
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
