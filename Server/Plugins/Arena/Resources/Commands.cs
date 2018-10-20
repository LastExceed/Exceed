using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resources;
using Server.Plugins.Arena.Database;
using Server.Plugins.Arena.Resources;
namespace Server.Plugins.Arena.Resources
{
    class Commands : CommandsBase
    {
        public Commands(PluginBase plugin) : base(plugin)
        {
        }
        public override Boolean ParseAsCommand(string message, Player source)
        {
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
            ArenaResponse response = ArenaResponse.Null;
            long[] currentPos = new long[3];
            switch (parameters[0])
            {
                case "setup":
                    #region setup
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsBaseMessages.baseNoPermission));
                        break;
                    }
                    string arenaName = null;
                    if (parameters.Length == 2)
                    {
                        arenaName = parameters[1];
                    }
                    else if (parameters.Length > 2)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupSyntax));
                        break;
                    }
                    response = ((ArenaCore)pluginRef).ArenaDatabase.launchSetupArena(arenaName);
                    if (response == ArenaResponse.SetupLaunched)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupInitialized));
                    }
                    else if (response == ArenaResponse.SetupAlreadyLaunched)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupAlreadyInitialized));
                    }
                    break;
                #endregion
                case "set":
                    #region setup-set
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsBaseMessages.baseNoPermission));
                        break;
                    }
                    if (parameters.Length == 2)
                    {
                        currentPos = new long[3]
                        {
                            source.entity.position.x,
                            source.entity.position.y,
                            source.entity.position.z
                        };
                        switch (parameters[1])
                        {
                            case "player1":
                                response = ((ArenaCore)pluginRef).ArenaDatabase.setPositionSetupArena(currentPos, ArenaPositionName.player1);
                                break;
                            case "player2":
                                response = ((ArenaCore)pluginRef).ArenaDatabase.setPositionSetupArena(currentPos, ArenaPositionName.player2);
                                break;
                            case "spectator":
                                response = ((ArenaCore)pluginRef).ArenaDatabase.setPositionSetupArena(currentPos, ArenaPositionName.spectator);
                                break;
                            default:
                                ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.setSyntax));
                                break;
                        }
                        if (response == ArenaResponse.SetupPositionSet)
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(string.Format(CommandsMessages.setPositionSuccess, parameters[1])));
                        }
                        else if (response == ArenaResponse.SetupEmpty)
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSetupNotInitialized));
                        }
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.setSyntax));
                    }
                    break;
                #endregion
                case "name":
                    #region setup-name
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsBaseMessages.baseNoPermission));
                        break;
                    }
                    if (parameters.Length == 2)
                    {
                        response = ((ArenaCore)pluginRef).ArenaDatabase.setNameArena(parameters[1]);
                        if (response == ArenaResponse.SetupNameSet)
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.nameChangeSuccess));
                        }
                        else if (response == ArenaResponse.SetupEmpty)
                        {
                            ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSetupNotInitialized));
                        }
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSyntax));
                    }
                    break;
                #endregion
                case "create":
                    #region arena-create
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsBaseMessages.baseNoPermission));
                        break;
                    }
                    if (parameters.Length > 1)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSyntax));
                        break;
                    }
                    response = ((ArenaCore)pluginRef).ArenaDatabase.SaveDuelArena();
                    if (response == ArenaResponse.ArenaCreated)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSuccess));
                    }
                    else if (response == ArenaResponse.SetupIncomplete)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.createIncomplete));
                    }
                    else if (response == ArenaResponse.SetupEmpty)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSetupNotInitialized));
                    }
                    break;
                #endregion
                case "delete":
                    #region arena-delete
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsBaseMessages.baseNoPermission));
                        break;
                    }
                    if (parameters.Length == 2)
                    {
                        bool isId = int.TryParse(parameters[1], out int n);
                        if (isId)
                        {
                            var id = Int32.Parse(parameters[1]);
                            response = ((ArenaCore)pluginRef).ArenaDatabase.RemoveDuelArena((uint)id);
                        }
                        else
                        {
                            response = ((ArenaCore)pluginRef).ArenaDatabase.RemoveDuelArena(parameters[1]);
                        }
                        switch (response)
                        {
                            case ArenaResponse.ArenaDeleted:
                                ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteSuccess));
                                break;
                            case ArenaResponse.ArenaNotFound:
                                ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteNotFound));
                                break;
                        }
                    }
                    else
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteSyntax));
                    }
                    break;
                #endregion
                case "reset":
                    #region reset
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsBaseMessages.baseNoPermission));
                        break;
                    }
                    if (parameters.Length > 1)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.resetSyntax));
                        break;
                    }
                    response = ((ArenaCore)pluginRef).ArenaDatabase.ResetArena();
                    if (response == ArenaResponse.ArenaReset)
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.resetSuccess));
                    }
                    #endregion
                    break;
                case "help":
                    #region arena-help
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        ServerCore.Notify(source, CommandsMessages.getMessage(CommandsBaseMessages.baseNoPermission));
                        break;
                    }
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.setSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.nameSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteSyntax));
                    ServerCore.Notify(source, CommandsMessages.getMessage(CommandsMessages.resetSyntax));
                    break;
                #endregion
                default:
                    return false;
            }
            return true;
        }
    }
}
