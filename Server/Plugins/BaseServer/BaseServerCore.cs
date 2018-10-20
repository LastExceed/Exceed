using System;
using Resources;
using Server.Plugins.BaseServer.Resources;
using Server.Plugins.BaseServer.Scripts;
namespace Server.Plugins.BaseServer
{
    class BaseServerCore : PluginBase
    {
        CommandsBase command;
        public BaseServerCore()
        {
            command = new Commands(this);
            ChatManager chatManager = new ChatManager();
            pluginName = BaseServerConfig.pluginName;
        }
        public override Boolean hasCommands()
        {
            return true;
        }
        public override Boolean analyzeCommand(string message,Player source)
        {
            return command.ParseAsCommand(message, source);
        }
    }
}
