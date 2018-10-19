using Resources;
using Server.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Extensions
{
    static class ConsoleCommands
    {
        public static void analyzeCommand(string consoleCommands)
        {
            var parameters = consoleCommands.Split(" ");
            switch (parameters[0])
            {
                case "promote":
                    if(parameters.Length == 3)
                    {
                        int role = int.Parse(parameters[2]);
                        if (0 <= role && role <= Enum.GetValues(typeof(RoleID)).Length - 1)
                        {
                            PromoteResponse response = ServerCore.userDatabase.PromoteUser(parameters[1], (RoleID)role);
                            if(response == PromoteResponse.Success)
                            {
                                var player = ServerCore.players.FirstOrDefault(x => x.entity?.name == parameters[1]);
                                if (player != null)
                                {
                                    player.Permission = (byte)role;
                                }
                                Log.PrintLn(String.Format("User '{0}' promoted to role {1}!", parameters[1], parameters[2]));
                            }
                            else if(response == PromoteResponse.InvalidTarget)
                            {
                                Log.PrintLn(String.Format("Error : Invalid target"));
                            }
                        }
                        else
                        {
                            Log.PrintLn(String.Format("Error : RoleId out of Range"));
                        }
                    }
                    else
                    {
                        Log.PrintLn(String.Format("Syntax : promote [player] [roleId]"));
                    }
                    break;
                case "default":
                    break;
            }
        }
    }
}