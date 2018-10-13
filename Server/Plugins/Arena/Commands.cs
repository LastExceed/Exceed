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
        public static Plugin pluginRef;
        public static void Init(ArenaDatabase arenaDatabase,Plugin plugin)
        {
            Server.ChatMessageReceived += ParseAsCommand;
            ArenaDatabase = arenaDatabase;
            pluginRef = plugin;
        }
        private static void ParseAsCommand(string message, Player source)
        {
            if (!message.StartsWith("/"+pluginRef.getName()))
            {
                return;
            }
            else if(message.Length == 1 + pluginRef.getName().Length)
            {
                Server.Notify(source, string.Format("Type /"+pluginRef.getName()+" help for further informations about this command"));
                return;
            }
            var parameters = message.Substring(2+pluginRef.getName().Length).Split(" ");
            foreach (var item in parameters)
            {
                Console.WriteLine(item.ToString());
            }
            var command = parameters[0].ToLower();
            ArenaResponse response = ArenaResponse.Null;
            long[] currentPos = new long[3];
            switch (command)
            {
                case "setup":
                    #region setup
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, "[Arena] no permission");
                        break;
                    }
                    string arenaName = null;
                    if (parameters.Length == 3)
                    {
                        arenaName = parameters[2].ToLower();
                    }
                    response = ArenaDatabase.launchSetupArena(arenaName);
                    if (response == ArenaResponse.SetupLaunched)
                    {
                        Server.Notify(source, String.Format("[Arena] The Setup Arena has been initialized !"));
                    }
                    else if (response == ArenaResponse.SetupAlreadyLaunched)
                    {
                        Server.Notify(source, String.Format("[Arena] The Setup Arena has already been initialized."));
                    }
                    break;
                #endregion
                /*case "list":
                    #region arena-list
                    ArenaDatabase.listArena();
                    break;
                    #endregion
               */
                case "set":
                    #region setup-set
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, "[Arena] no permission");
                        break;
                    }
                    if (parameters.Length == 3)
                    {
                        currentPos = new long[3]
                        {
                                                    source.entity.position.x,
                                                    source.entity.position.y,
                                                    source.entity.position.z
                        };
                        switch (parameters[2].ToLower())
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
                                Server.Notify(source, String.Format("Syntax : /arena set player1|player2|spectator"));
                                break;
                        }
                        if (response == ArenaResponse.SetupPositionSet)
                        {
                            Server.Notify(source, String.Format("[Arena] Parameters {0} has been successfully updated", parameters[2].ToLower()));
                        }
                        else if (response == ArenaResponse.SetupEmpty)
                        {
                            Server.Notify(source, String.Format("[Arena] The Setup Arena hasn't been initialized"));
                        }
                    }
                    else
                    {
                        Server.Notify(source, String.Format("Syntax : /arena set player1|player2|spectator"));
                    }
                    break;
                #endregion
                case "name":
                    #region setup-name
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, "[Arena] no permission");
                        break;
                    }
                    if (parameters.Length == 3)
                    {
                        response = ArenaDatabase.setNameArena(parameters[2].ToLower());
                        if (response == ArenaResponse.SetupNameSet)
                        {
                            Server.Notify(source, String.Format("[Arena] The Setup Arena's name has been changed !"));
                        }
                        else if (response == ArenaResponse.SetupEmpty)
                        {
                            Server.Notify(source, String.Format("[Arena] The Setup Arena hasn't been initialized"));
                        }
                    }
                    else
                    {
                        Server.Notify(source, String.Format("Syntax : /arena name [newName]"));
                    }
                    break;
                #endregion
                case "create":
                    #region arena-create
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, "[Arena] no permission");
                        break;
                    }
                    response = ArenaDatabase.SaveDuelArena();
                    if (response == ArenaResponse.ArenaCreated)
                    {
                        Server.Notify(source, string.Format("[Arena] Arena has been successfully saved and is operational !"));
                    }
                    else if (response == ArenaResponse.SetupIncomplete)
                    {
                        Server.Notify(source, string.Format("[Arena] Setup Arena is incomplete !"));
                    }
                    else if (response == ArenaResponse.SetupEmpty)
                    {
                        Server.Notify(source, String.Format("[Arena] The Setup Arena hasn't been initialized"));
                    }
                    break;
                #endregion
                case "delete":
                    #region arena-delete
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, "[Arena] no permission");
                        break;
                    }
                    if (parameters.Length == 3)
                    {
                        bool isId = int.TryParse(parameters[2].ToLower(), out int n);
                        if (isId)
                        {
                            var id = Int32.Parse(parameters[2].ToLower());
                            response = ArenaDatabase.RemoveDuelArena((uint)id);
                        }
                        else
                        {
                            response = ArenaDatabase.RemoveDuelArena(parameters[2].ToLower());
                        }
                        switch (response)
                        {
                            case ArenaResponse.ArenaDeleted:
                                Server.Notify(source, string.Format("[Arena] Arena deleted !"));
                                break;
                            case ArenaResponse.ArenaNotFound:
                                Server.Notify(source, string.Format("[Arena] Invalid id or arena's name"));
                                break;
                        }
                    }
                    else
                    {
                        Server.Notify(source, string.Format("Syntax : /duel arena-delete [id] or /duel arena-delete [name] "));
                    }
                    break;
                #endregion
                case "reset":
                    #region reset
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, "[Arena] no permission");
                        break;
                    }
                    response = ArenaDatabase.ResetArena();
                    if (response == ArenaResponse.ArenaReset)
                    {
                        Server.Notify(source, string.Format("[Arena] Arena's list reseted !"));
                    }
                    #endregion
                    break;
                case "help":
                    #region arena-help
                    if (source.entity.name != "BLACKROCK" && source.entity.name != "BLIZZY")
                    {
                        Server.Notify(source, string.Format("/arena setup"));
                        Server.Notify(source, string.Format("/arena set player1|player2|spectator"));
                        Server.Notify(source, string.Format("/arena name [newName]"));
                        Server.Notify(source, string.Format("/arena create"));
                        Server.Notify(source, string.Format("/arena delete [ArenaId] or /arena delete [Name]"));
                    }
                    Server.Notify(source, string.Format("/arena list"));
                    break;
                #endregion
                default:
                    if (PluginsCore.pluginsWithCommands.Last().getName() == pluginRef.getName())
                    {
                        Server.Notify(source, string.Format("unknown command '{0}'", parameters[0]));
                    }
                    break;
            }
        }
    }
}
