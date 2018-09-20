using System;
using Resources;
using System.Threading.Tasks;
using Server.Database;

namespace Server.Addon
{
    public class Duel
    {
        private Boolean ongoing; // status of the duel
        public Player winner;
        public Arena arena;
        public long[] arenaPosition;
        public ArenaDatabase ArenaDatabase;
        public Player player1;
        private long[] storedPos1; // initial player1 position
        private float? storedHp1; // initial player1 position
        public Player player2;
        private long[] storedPos2; // initial player2 position
        private float? storedHp2; // initial player2 position
        private int request_state;// 0 waiting | 1 accepted | 2 refused
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
                        NotifyPlayers("Duel is starting");
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
            });
        }
        public Duel(Player player1,Player player2,long requestInitialTime)
        {
            this.ongoing = false;
            this.player1 = player1;
            this.player2 = player2;
            WaitingResponse(requestInitialTime);
        }
        public void ChooseArena()
        {
            this.arena = ArenaDatabase.FindArena();
            this.arenaPosition = new long[]{
                this.arena.X,
                this.arena.Y,
                this.arena.Z
            };
        }
        private void StartDuel()
        {
            this.storedPos1 = new long[]{
                this.player1.entity.position.x,
                this.player1.entity.position.y,
                this.player1.entity.position.z,
             };
            this.storedPos2 = new long[]{
                this.player2.entity.position.x,
                this.player2.entity.position.y,
                this.player2.entity.position.z,
             };
            this.storedHp1 = this.player1.entity.HP;
            this.storedHp2 = this.player2.entity.HP;
            this.player1.Duel = true;
            this.player2.Duel = true;
            Server.TeleportPlayer(arenaPosition,this.player1);
            Server.TeleportPlayer(arenaPosition,this.player2);
            while(this.ongoing == true)
            {
                if(this.player1.entity.HP <= 0 || this.player2.entity.HP <= 0)
                {
                    this.winner = this.player1.entity.HP <= 0 ? this.player2 : this.player1;
                    this.ongoing = false;
                }
                System.Threading.Thread.Sleep(1000);
            }
            NotifyPlayers(String.Format("{0} won this duel", this.winner.entity.name));
            Server.setHp(this.storedHp1, this.player1);
            Server.setHp(this.storedHp2, this.player2);
            Server.TeleportPlayer(this.storedPos1, this.player1);
            Server.TeleportPlayer(this.storedPos2, this.player2);
            this.player1.Duel = null;
            this.player2.Duel = null;
            Server.duels.Remove(this);
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
            Server.Notify(this.player1, message);
            Server.Notify(this.player2, message);
        }
    }
}
