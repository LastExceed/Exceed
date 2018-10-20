using System;
using System.Collections.Generic;
using System.Text;
using Resources;
using System.Threading.Tasks;
using Server.Plugins.Arena.Database;

namespace Server.Plugins.Duel
{
    class DuelSystem
    {
        private PluginBase pluginRef;
        private volatile Boolean ongoing; // status of the duel
        private volatile Boolean stop; // Token for stopping duel/thread
        public Player winner;
        public ArenaEntity arena;
        private List<Player> Spectators;
        public ArenaDatabase ArenaDatabase;
        public Player player1;
        private long[] storedPos1; // initial player1 position
        private float? storedHp1; // initial player1 position
        public Player player2;
        private long[] storedPos2; // initial player2 position
        private float? storedHp2; // initial player2 position
        private volatile int request_state;// 0 waiting | 1 accepted | 2 refused
        private long requestInitialTime;
        private Boolean WaitingResponse()
        {
            #region waitingResponse
            while (DateTimeOffset.UtcNow.ToUnixTimeSeconds() < this.requestInitialTime + DuelConfig.maxRequestTime && this.request_state == 0 && stop == false)
            {
                System.Threading.Thread.Sleep(1000);
            }
            if (stop == true)
            {
                return false;
            }
            switch (this.request_state)
            {
                case 1:
                    NotifyPlayers("[Duel] Duel is starting");
                    this.ongoing = true;
                    break;
                case 2:
                    ServerCore.Notify(this.player1, string.Format("[Duel] {0} refused the duel", this.player2.entity.name));
                    break;
                default:
                    ServerCore.Notify(this.player1, string.Format("[Duel] {0} didn't respond within the 30s limit", this.player2.entity.name));
                    break;
            }
            return ongoing;
            #endregion
        }
        public DuelSystem(PluginBase pluginRef,Player player1, Player player2, long requestInitialTime)
        {
            #region createDuel
            this.pluginRef = pluginRef;
            this.ongoing = false;
            this.player1 = player1;
            this.player2 = player2;
            this.requestInitialTime = requestInitialTime;
            this.ArenaDatabase = new ArenaDatabase();
            this.Spectators = new List<Player>();
            #endregion
        }
        public void PickArena()
        {
            #region PickArena
            this.arena = ArenaDatabase.FetchRandomArena();
            if (this.arena == null)
            {
                NotifyPlayers("[Duel] An error occured : the arena's list is empty");
                this.stop = true;
            }
            #endregion
        }
        public void SetPlayersState()
        {
            #region setPlayersState
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
            // this.player1.Duel = true;
            // this.player2.Duel = true;
            // Server.TeleportPlayer(this.arena.getPosition(ArenaPositionName.player1), this.player1);
            // Server.TeleportPlayer(this.arena.getPosition(ArenaPositionName.player2), this.player2);
            #endregion
        }
        public void LaunchPreparingTime()
        {
            #region launchPreparingTime
            // player1.PreparingTime = true;
            // player2.PreparingTime = true;
            this.player1.entity.hostility = Hostility.Neutral;
            this.player2.entity.hostility = Hostility.Neutral;
            NotifyPlayers("[Duel] Duel is starting in 10sd");
            System.Threading.Thread.Sleep(10000);
            // Server.setHostility(Hostility.Player, this.player1);
            this.player1.entity.hostility = Hostility.Player;
            this.player2.entity.hostility = Hostility.Player;
            // player1.PreparingTime = null;
            // player2.PreparingTime = null;
            NotifyPlayers("[Duel] Go !");
            #endregion
        }
        public void RestorePlayersState()
        {
            #region restorePlayerState
            // Server.setHp(this.storedHp1, this.player1);
            // Server.setHp(this.storedHp2, this.player2);
            // Server.TeleportPlayer(this.storedPos1, this.player1);
            // Server.TeleportPlayer(this.storedPos2, this.player2);
            // this.player1.Duel = null;
            // this.player2.Duel = null;
            #endregion
        }
        private void ManageDuel()
        {
            #region ManageDuel
            PickArena();
            if (this.arena != null)
            {
                SetPlayersState();
                LaunchPreparingTime();
            }
            while (this.ongoing == true && this.stop == false)
            {
                if (this.player1.entity.HP <= 0 || this.player2.entity.HP <= 0)
                {
                    this.winner = this.player1.entity.HP <= 0 ? this.player2 : this.player1;
                    this.ongoing = false;
                }
                System.Threading.Thread.Sleep(1000);
            }
            if (this.stop == false)
            {
                NotifyPlayers(String.Format("[Duel] {0} won this duel", this.winner.entity.name));
                RestorePlayersState();
            }
            else
            {
                if (this.arena != null)
                {
                    RestorePlayersState();
                }
            }
            ((DuelCore)pluginRef).duels.Remove(this);
            #endregion
        }
        public void Stop()
        {
            #region stop
            NotifyPlayers(String.Format("[Duel] The duel is cancelled"));
            this.stop = true;
            #endregion
        }
        public void AcceptDuel()
        {
            this.request_state = 1;
        }
        public void RefuseDuel()
        {
            this.request_state = 2;
        }
        public void Spectate(Player player)
        {
            // Server.TeleportPlayer(this.arena.getPosition(ArenaPositionName.spectator), player);
            this.Spectators.Add(player);
        }
        public void NotifyPlayers(string message)
        {
            ServerCore.Notify(this.player1, message);
            ServerCore.Notify(this.player2, message);
        }
        public void RunDuel()
        {
            #region runDuel
            Boolean player2Accepted = WaitingResponse();
            if (player2Accepted)
            {
                ManageDuel();
            }
            else
            {
                ((DuelCore)pluginRef).duels.Remove(this);
            }
            #endregion
        }
    }
}
