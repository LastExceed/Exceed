using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins
{
    class CommandsMessage
    {
        public static readonly string baseUnknowCommand = "Unknown command : '{0}'";
        public static readonly string baseNoPermission = "You don't have sufficient permission for this command";
        public static string getMessage(String message)
        {
            return String.Format("[{0}] " + message, "Server");
        }
    }
}
