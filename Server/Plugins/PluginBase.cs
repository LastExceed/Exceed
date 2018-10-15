using Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins
{
    public class PluginBase
    {
        protected String pluginName;
        public virtual Boolean hasCommands()
        {
            return false;
        }
        public String getName()
        {
            return pluginName;
        }
        public virtual Boolean analyzeCommand(string message, Player source)
        {
            return false;
        }
    }
}
