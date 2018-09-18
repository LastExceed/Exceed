using System;
using System.Collections.Generic;
using System.Text;
using Resources;
using System.Threading.Tasks;
using System.Threading;

namespace Server.Addon
{
    public class Duel
    {
        private Boolean start;
        public Player player1;
        private long[] storedPosPlayer1;
        private long[] storedPosPlayer2;
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
                        Server.duels.Remove(this);
                        break;
                    default:
                        Server.Notify(this.player1, string.Format("{0} didn't respond within the 30s limit", this.player2.entity.name));
                        Server.duels.Remove(this);
                        break;
                }
            });
        }
        public Duel(Player player1,Player player2,long requestInitialTime)
        {
            this.start = false;
            this.player1 = player1;
            this.storedPosPlayer1 = new long[]  { player1.entity.position.x, player1.entity.position.y, player1.entity.position.z};
            this.player2 = player2;
            this.storedPosPlayer2 = new long[]  { player2.entity.position.x, player2.entity.position.y, player2.entity.position.z};
            WaitingResponse(requestInitialTime);
        }
        private void StartDuel()
        {
            Server.TeleportPlayer(Config.arena1, player1);
            Console.WriteLine("{0}  {1}", Config.arena1[0], player1.entity.position.x);
            // Initialize the duel
        }
        public void Stop()
        {
            player1.entity.position.x = storedPosPlayer1[0];
            player1.entity.position.y = storedPosPlayer1[1];
            player1.entity.position.z = storedPosPlayer1[2];
            player2.entity.position.x = storedPosPlayer2[0];
            player2.entity.position.y = storedPosPlayer2[1];
            player2.entity.position.z = storedPosPlayer2[2];
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
    }
}
