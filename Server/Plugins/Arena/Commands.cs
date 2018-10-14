using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resources;
using Server.Plugins.Arena.Database;
using Server.Plugins.Arena.Resources;

namespace Server.Plugins.Arena
{
    class Commands
    {
        public static ArenaDatabase ArenaDatabase;
        public static PluginBase pluginRef;
        public static void Init(ArenaDatabase arenaDatabase, PluginBase plugin)
        {
            Server.ChatMessageReceived += ParseAsCommand;
            ArenaDatabase = arenaDatabase;
            pluginRef = plugin;
        }
        private static void ParseAsCommand(string message, Player source)
        {
            message = message.ToLower();
            if (!message.StartsWith("/" + pluginRef.getName().ToLower()))
            {
                if (PluginsCore.pluginsWithCommands.Last().getName() == pluginRef.getName())
                {
                    Server.Notify(source, string.Format("unknown command"));
                }
                return;
            }
            if (message.Length == 1 + pluginRef.getName().Length)
            {
                Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSyntax));
                return;
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
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoPermission));
                        break;
                    }
                    string arenaName = null;
                    if (parameters.Length == 2)
                    {
                        arenaName = parameters[1];
                    }
                    else if(parameters.Length > 2)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupSyntax));
                        break;
                    }
                    response = ArenaDatabase.launchSetupArena(arenaName);
                    if (response == ArenaResponse.SetupLaunched)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupInitialized));
                    }
                    else if (response == ArenaResponse.SetupAlreadyLaunched)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupAlreadyInitialized));
                    }
                    break;
                #endregion
                case "set":
                    #region setup-set
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoPermission));
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
                                response = ArenaDatabase.setPositionSetupArena(currentPos, ArenaPositionName.player1);
                                break;
                            case "player2":
                                response = ArenaDatabase.setPositionSetupArena(currentPos, ArenaPositionName.player2);
                                break;
                            case "spectator":
                                response = ArenaDatabase.setPositionSetupArena(currentPos, ArenaPositionName.spectator);
                                break;
                            default:
                                Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.setSyntax));
                                break;
                        }
                        if (response == ArenaResponse.SetupPositionSet)
                        {
                            Server.Notify(source, CommandsMessages.getMessage(String.Format(CommandsMessages.setPositionSuccess, parameters[1])));
                        }
                        else if (response == ArenaResponse.SetupEmpty)
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSetupNotInitialized));
                        }
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.setSyntax));
                    }
                    break;
                #endregion
                case "name":
                    #region setup-name
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoPermission));
                        break;
                    }
                    if (parameters.Length == 2)
                    {
                        response = ArenaDatabase.setNameArena(parameters[1]);
                        if (response == ArenaResponse.SetupNameSet)
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.nameChangeSuccess));
                        }
                        else if (response == ArenaResponse.SetupEmpty)
                        {
                            Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSetupNotInitialized));
                        }
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSyntax));
                    }
                    break;
                #endregion
                case "create":
                    #region arena-create
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoPermission));
                        break;
                    }
                    if(parameters.Length > 1)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSyntax));
                        break;
                    }
                    response = ArenaDatabase.SaveDuelArena();
                    if (response == ArenaResponse.ArenaCreated)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSuccess));
                    }
                    else if (response == ArenaResponse.SetupIncomplete)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.createIncomplete));
                    }
                    else if (response == ArenaResponse.SetupEmpty)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSetupNotInitialized));
                    }
                    break;
                #endregion
                case "delete":
                    #region arena-delete
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoPermission));
                        break;
                    }
                    if (parameters.Length == 2)
                    {
                        bool isId = int.TryParse(parameters[1], out int n);
                        if (isId)
                        {
                            var id = Int32.Parse(parameters[1]);
                            response = ArenaDatabase.RemoveDuelArena((uint)id);
                        }
                        else
                        {
                            response = ArenaDatabase.RemoveDuelArena(parameters[1]);
                        }
                        switch (response)
                        {
                            case ArenaResponse.ArenaDeleted:
                                Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteSuccess));
                                break;
                            case ArenaResponse.ArenaNotFound:
                                Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteNotFound));
                                break;
                        }
                    }
                    else
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteSyntax));
                    }
                    break;
                #endregion
                case "reset":
                    #region reset
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseNoPermission));
                        break;
                    }
                    if(parameters.Length > 1)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.resetSyntax));
                        break;
                    }
                    response = ArenaDatabase.ResetArena();
                    if (response == ArenaResponse.ArenaReset)
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.resetSuccess));
                    }
                    #endregion
                    break;
                case "help":
                    #region arena-help
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.setupSyntax));
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.setSyntax));
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.nameSyntax));
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.createSyntax));
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.deleteSyntax));
                        Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.resetSyntax));
                    }
                    break;
                #endregion
                default:
                    Server.Notify(source, CommandsMessages.getMessage(CommandsMessages.baseSyntax));
                    break;
            }
        }
    }
}
