using Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins
{
    abstract class CommandsBase
    {
        public PluginBase pluginRef;
        protected CommandsBase(PluginBase plugin)
        {
            pluginRef = plugin;
        }
        public virtual Boolean ParseAsCommand(string message,Player source)
        {
            return false;
        }
    }
}
