using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins.Arena.Resources
{
    static class CommandsMessages
    {
        // Base Command
        public static readonly string baseSyntax = "Type '/arena help' for further informations about this plugin";
        public static readonly string baseSetupNotInitialized = "The Setup Arena hasn't been initialized";

        // Setup
        public static readonly string setupSyntax = "Syntax: /arena setup";
        public static readonly string setupInitialized = "The Setup Arena has been initialized !";
        public static readonly string setupAlreadyInitialized = "The Setup Arena has already been initialized.";

        // Set
        public static readonly string setSyntax = "Syntax : /arena set player1|player2|spectator";
        public static readonly string setPositionSuccess = "Parameters {0} has been successfully updated";

        // Name
        public static readonly string nameSyntax = "Syntax : /arena name [newName]";
        public static readonly string nameChangeSuccess = "The Setup Arena's name has been changed !";

        // Create
        public static readonly string createSyntax = "Syntax : /arena create";
        public static readonly string createSuccess = "Arena has been successfully saved and is operational !";
        public static readonly string createIncomplete = "Setup Arena is incomplete !";

        // Delete
        public static readonly string deleteSyntax = "Syntax : /duel arena-delete [id] or /duel arena-delete [name] ";
        public static readonly string deleteSuccess = " Arena deleted !";
        public static readonly string deleteNotFound = "Invalid id or arena's name";

        // Reset
        public static readonly string resetSyntax= "Syntax : /arena reset";
        public static readonly string resetSuccess = "Arena's list reset !";

        public static string getMessage(String message)
        {
            return String.Format("[{0}] " + message, ArenaConfig.pluginName);
        }
    }
}
