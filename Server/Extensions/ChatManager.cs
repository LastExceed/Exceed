using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resources;
using Resources.Datagram;
using Server.Plugins;

namespace Server.Extensions
{
    class ChatManager
    {
        public static void Init()
        {
            Server.ChatMessageReceived += analyzeMessage;
        }
        private static void analyzeMessage(Chat chat, Player source)
        {
            var message = chat.Text;
            Log.Print(Server.dynamicEntities[chat.Sender].name + ": ", ConsoleColor.Cyan);
            Log.PrintLn(chat.Text, ConsoleColor.White, false);
            if (!message.StartsWith("/"))
            {
                Server.BroadcastUDP(chat.data, null); //pass to all players
                return;
            }
            Boolean CommandFound;
            CommandFound = ChatCommands.ParseAsCommand(message, source);
            if (!CommandFound)
            {
                foreach (PluginBase plugin in PluginsCore.pluginsWithCommands)
                {
                    CommandFound = plugin.analyzeCommand(message, source);
                    if (CommandFound == true)
                    {
                        break;
                    }
                }
                if (!CommandFound)
                {
                    var commandName = message.Substring(1).Split(" ")[0];
                    Server.Notify(source, CommandsBaseMessages.getMessage(String.Format(CommandsBaseMessages.baseUnknowCommand, commandName)));
                }
            }
        }
    }
}
