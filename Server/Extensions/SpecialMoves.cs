using Resources;
using Resources.Datagram;
using Resources.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Extensions {
    public static class SpecialMoves {
        public static void Init() {
            ServerCore.SpecialMoveUsed += OnSpecialMove;
        }

        private static void OnSpecialMove(SpecialMove specialMove, Player source) {
            switch (specialMove.Id) {
                case SpecialMoveID.Taunt:
                    var target = ServerCore.players.FirstOrDefault(p => p.entity.guid == specialMove.Guid);
                    if (target != null) {
                        specialMove.Guid = (ushort)source.entity.guid;
                        ServerCore.SendUDP(specialMove.data, target);
                    }
                    break;
                case SpecialMoveID.SmokeBomb:
                    ServerCore.BroadcastUDP(specialMove.data, source);
                    break;
                case SpecialMoveID.CursedArrow:
                case SpecialMoveID.ArrowRain:
                case SpecialMoveID.Shrapnel:
                case SpecialMoveID.IceWave:
                case SpecialMoveID.Confusion:
                case SpecialMoveID.ShadowStep:
                    ServerCore.BroadcastUDP(specialMove.data);
                    break;
                default:
                    break;
            }
        }
    }
}
