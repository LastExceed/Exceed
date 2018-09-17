using System;
using System.Collections.Generic;
using System.Text;
using Resources;
using System.Threading.Tasks;
namespace Server.Addon
{
    public class Duel
    {
        private Boolean start;
        public Player player1;
        public Player player2;
        public volatile int request_state;// 0 waiting | 1 accepted | 2 refused
        private async Task WaitingResponse(long requestInitialTime)
        {
            await Task.Run(() =>
            {
                while (DateTimeOffset.UtcNow.ToUnixTimeSeconds() < requestInitialTime + Config.maxRequestTime && this.request_state == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                switch (this.request_state)
                {
                    case 1:
                        Server.Notify(this.player1, string.Format("Duel is starting"));
                        Server.Notify(this.player2, string.Format("Duel is starting"));
                        this.start = true;
                        StartDuel();
                        break;
                    case 2:
                        Server.Notify(this.player1, string.Format("{0} refused the duel", this.player2.entity.name));
                        break;
                    default:
                        Server.Notify(this.player1, string.Format("{0} didn't respond within the 30s limit", this.player2.entity.name));
                        break;
                }
                Server.duels.Remove(this);
            });
        }
        public Duel(Player player1,Player player2,long requestInitialTime)
        {
            this.start = false;
            this.player1 = player1;
            this.player2 = player2;
            WaitingResponse(requestInitialTime);
        }
        private void StartDuel()
        {
            // Initialize the duel
        }
        public void AcceptDuel()
        {
            this.request_state = 1;
        }
        public void RefuseDuel()
        {
            this.request_state = 2;
        }
    }
}
