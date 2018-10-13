using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins
{
    public class Plugin
    {
        protected String pluginName;
        public virtual Boolean hasCommands()
        {
            Console.WriteLine("test");
            return false;
        }
        public String getName()
        {
            return pluginName;
        }
    }
}
