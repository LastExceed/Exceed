using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins.Duel.Resources
{
    static class CommandsMessages
    {
        // Base Command
        public static readonly string baseSyntax = "Type '/duel help' for further informations about this plugin";
        public static readonly string baseInvalidTarget = "Invalid target";
        public static readonly string baseAlreadyDueling = "You're already involved in a duel";
        public static readonly string baseNoDuelRequestFound = "No duel request found";
        // Start
        public static readonly string startSyntax = "Syntax : /duel start [player2]";
        public static readonly string startSelfError = "Unfortunatly, you can't duel yourself";
        public static readonly string startTargetAlreadyDueling = "The target is already involved in a duel";
        public static readonly string startAcceptMessage = "{0} wants to duel you ! /duel accept or /duel refuse";

        // Duel Accept
        public static readonly string acceptSyntax = "Syntax : /duel accept";
        // Duel Refuse
        public static readonly string refuseSyntax = "Syntax : /duel refuse";

        // Duel Stop
        public static readonly string stopSyntax = "Syntax : /duel stop";
        // Spectate
        public static readonly string spectateSyntax = "Syntax : /duel spectate [player]";
        public static readonly string spectateSuccess = "You are spectating the duel of {0} !";

        public static string getMessage(String message)
        {
            return String.Format("[{0}] " + message, DuelConfig.pluginName);
        }
    }
}
