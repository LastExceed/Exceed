using Resources;
using Resources.Datagram;
using Resources.Packet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Extensions {
    public static class Logging {
        public static void Init() {
            BridgeCore.ChatMessageReceived += ChatLog;
            BridgeCore.ClientConnected += LogConnect;
            BridgeCore.ClientDisconnected += LogDisconnect;

            BridgeCore.EntityUpdateReceived += CheckForNewName;
            BridgeCore.EntityUpdateSent += CheckForNewName;
            BridgeCore.DynamicEntityRemoved += RemoveNameFromList;
            BridgeCore.ClientDisconnected += RefreshPlayerlist;
        }

        private static void ChatLog(Chat chat) {
            if (chat.Sender == 0) {
                BridgeCore.form.Log(chat.Text + "\n", Color.Magenta);
            }
            else {
                BridgeCore.form.Log(BridgeCore.dynamicEntities[chat.Sender].name + ": ", Color.Cyan);
                BridgeCore.form.Log(chat.Text + "\n", Color.White);
            }
        }
        private static void LogConnect() {
            BridgeCore.form.Log("client connected\n", Color.Green);
        }
        private static void LogDisconnect() {
            BridgeCore.form.Log("client disconnected\n", Color.Red);
        }

        private static void CheckForNewName(EntityUpdate entityUpdate) {
            if (entityUpdate.name != null) {
                RefreshPlayerlist();
            }
        }
        private static void RemoveNameFromList(RemoveDynamicEntity rde) {
            RefreshPlayerlist();
        }
        private static void RefreshPlayerlist() {
            BridgeCore.form.Invoke((Action)BridgeCore.form.listBoxPlayers.Items.Clear);
            foreach (var dynamicEntity in BridgeCore.dynamicEntities.Values.ToList()) {
                if (dynamicEntity.hostility == Hostility.Player) {
                    BridgeCore.form.Invoke(new Action(() => BridgeCore.form.listBoxPlayers.Items.Add(dynamicEntity.name)));
                }
            }
        }
    }
}
