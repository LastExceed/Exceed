using System;
using System.Collections.Generic;
using System.Text;
namespace Server.Plugins
{
    public static class PluginsCore
    {
        public static List<PluginBase> pluginsList;
        public static List<PluginBase> pluginsWithCommands;
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
                    Log.PrintLn("Plugin \"" + pluginName + "\" has been successfully initialized !", ConsoleColor.Green);
                    pluginsList.Add(plugin);
                }
                else
                {
                    Log.PrintLn("Plugin \""+pluginName+"\" cannot be found !", ConsoleColor.Red);
                }
            }
        }
    }
}
