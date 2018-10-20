using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Server.Extensions;

namespace Server.Plugins
{
    public static class PluginsCore
    {
        public static List<PluginBase> pluginsList;
        public static List<PluginBase> pluginsWithCommands;
        public static List<string> DependenciesList;
        public static void Init()
        {
            pluginsList = new List<PluginBase>();
            pluginsWithCommands = new List<PluginBase>();
            foreach (String pluginName in PluginsConfig.pluginsName)
            {
                var pluginPath = Type.GetType("Server.Plugins." + pluginName + "." + pluginName + "Core");
                if (pluginPath != null)
                {
                    var plugin = (PluginBase)Activator.CreateInstance(pluginPath);
                    if (plugin.hasCommands())
                    {
                        pluginsWithCommands.Add(plugin);
                    }
                    Log.PrintLn(String.Format("Plugin '{0}' has been successfully initialized !",pluginName), ConsoleColor.Green);
                    pluginsList.Add(plugin);
                }
                else
                {
                    Log.PrintLn(String.Format("Plugin '{0}' cannot be found !", pluginName), ConsoleColor.Red);
                }
            }
            foreach(PluginBase plugin in pluginsList)
            {
                var dependencies = plugin.checkDependencies();
                foreach(string dependency in dependencies)
                {
                    if(!pluginsList.Select(p => p.pluginName).Contains(dependency))
                    {
                        Log.PrintLn(String.Format("Plugin '{0}' require the initialization of plugin '{1}' !", plugin.pluginName,dependency), ConsoleColor.Red);
                    }
                }
            }
            if(pluginsWithCommands.Count > 0)
            {
                ChatMessage.Desactivate();
            }
        }
    }
}
