using System;
using System.Collections.Generic;
using System.Text;
using Resources;
using System.Threading.Tasks;
namespace Server.Addon
{
    public class Duel
    {
        private Boolean ongoing;
        public Player winner; 
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
                        this.ongoing = true;
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
            this.ongoing = false;
            this.player1 = player1;
            this.player2 = player2;
            WaitingResponse(requestInitialTime);
        }
        private void StartDuel()
        {
            long[] arena1 = new long[]{ 550299340623, 550298079857, 9770177 };
            Server.TeleportPlayer(arena1,player1);
            Server.TeleportPlayer(arena1, player2);
            while(this.ongoing == true)
            {
                if(player1.entity.HP <= 0)
                {
                    this.winner = player2;
                    this.ongoing = false;
                }
                else if(player2.entity.HP <= 0)
                {
                    this.winner = player1;
                    this.ongoing = false;
                }
                System.Threading.Thread.Sleep(1000);
            }
            NotifyPlayers(String.Format("{0} won this duel", winner.entity.name));
            // Initialize the duel
        }
        public void Stop()
        {
            if (this.ongoing == true)
            {
                NotifyPlayers(String.Format("The duel was cancelled"));
            }
            Server.duels.Remove(this);
        }
        public void AcceptDuel()
        {
            this.request_state = 1;
        }
        public void RefuseDuel()
        {
            this.request_state = 2;
        }
        public void NotifyPlayers(string message)
        {
            Server.Notify(player1, message);
            Server.Notify(player2, message);
        }
    }
}
