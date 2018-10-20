using System;
using Resources;
using Resources.Datagram;

namespace Server.Plugins.BaseServer.Scripts
{
    public class ChatManager
    {
        public ChatManager()
        {
            ServerCore.ChatMessageReceived += analyzeMessage;
        }
        private static void analyzeMessage(Chat chat, Player source)
        {
            var message = chat.Text;
            Log.Print(ServerCore.dynamicEntities[chat.Sender].name + ": ", ConsoleColor.Cyan);
            Log.PrintLn(chat.Text, ConsoleColor.White, false);
            if (!message.StartsWith("/"))
            {
                ServerCore.BroadcastUDP(chat.data, null); //pass to all players
                return;
            }
            Boolean CommandFound = false;
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
                ServerCore.Notify(source, CommandsBaseMessages.getMessage(String.Format(CommandsBaseMessages.baseUnknowCommand, commandName)));
            }
        }
    }
}
